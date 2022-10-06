using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace CitrusDammakuCount.Downloader
{
    public class Downloader : MonoBehaviour
    {
        public List<Season> seasons;
        public string savePath;
        public string cookieFilePath;
        public string endDate;
        public float delay = 5;
        public float retryDelay = 5;
        public bool skipExistChapter;
        [Header("AutoCount")]
        public Counter counter;
        public bool countAfterDownload;

        const int fragLength = 60 * 6 * 1000;

        [Serializable]
        public class Season
        {
            public string season_id;
            public string fileName;
        }

#if UNITY_EDITOR
        private void Awake()
        {
            Application.targetFrameRate = 60;
            StartCoroutine(Process());
        }

        IEnumerator Process()
        {
            foreach (var season in seasons)
            {
                yield return Download(season);
                Debug.Log($"{season.fileName} 下载完成");
                if (countAfterDownload)
                    yield return counter.Count(
                        AssetDatabase.LoadAssetAtPath<ChapterDanmakuSet>
                            (Path.Combine(savePath, season.fileName, season.fileName + ".asset"))
                            ,(msg)=>Debug.Log(msg));
            }
            Debug.Log("全部完成");
        }

        IEnumerator DownloadFile(string url, string tempFile, string cookie)
        {
            while (true)
            {
                using (UnityWebRequest getRequest = UnityWebRequest.Get(url))
                {
                    getRequest.downloadHandler = new DownloadHandlerFile(tempFile, false);
                    getRequest.SetRequestHeader("cookie", cookie);
                    yield return getRequest.SendWebRequest();
                    if (getRequest.error != null)
                    {
                        Debug.LogError(getRequest.error);
                    }
                    else
                        break;
                    yield return new WaitForSeconds(retryDelay);
                }
            }
        }

        IEnumerator Download(Season season)
        {
            string tempFile = Path.GetTempFileName();
            string cookie = File.ReadAllText(cookieFilePath);
            string urlSeason = $"http://api.bilibili.com/pgc/view/web/season?season_id={season.season_id}";
            DateTime endDate = DateTime.Parse(this.endDate);
            string folder = Path.Combine(savePath, season.fileName);
            Directory.CreateDirectory(folder);

            BilibiliAPI.Season.Rootobject seasonRoot;
            while (true)
            {
                using (UnityWebRequest getRequest = UnityWebRequest.Get(urlSeason))
                {
                    getRequest.downloadHandler = new DownloadHandlerFile(tempFile, false);
                    getRequest.SetRequestHeader("cookie", cookie);
                    yield return getRequest.SendWebRequest();
                    if (getRequest.error == null)
                    {
                        seasonRoot = JsonUtility.FromJson<BilibiliAPI.Season.Rootobject>(File.ReadAllText(tempFile));
                        break;
                    }
                    else
                    {
                        Debug.LogError(getRequest.error);
                        yield return new WaitForSeconds(retryDelay);
                    }
                }
            }

            if (seasonRoot.code != 0)
            {
                Debug.LogError(seasonRoot.message);
                yield break;
            }

            for (int ep = 0; ep < seasonRoot.result.episodes.Length; ep++)
            {
                BilibiliAPI.Season.Episode episode = seasonRoot.result.episodes[ep];
                string file = Path.Combine(folder, $"{season.fileName}_{ep + 1}.bytes");
                if (skipExistChapter && File.Exists(file))
                {
                    Debug.Log($"{file} 已存在，跳过章节");
                    continue;
                }

                HashSet<Danmaku> danmakus = new HashSet<Danmaku>();

                //获取实时弹幕
                int packId = 1;
                for (int i = 0; i < episode.duration; i += fragLength)
                {
                    Debug.Log(packId);
                    string urlRealtimeDM = $"http://api.bilibili.com/x/v2/dm/web/seg.so?type=1&oid={episode.cid}&segment_index={packId}";
                    packId++;
                    while (true)
                    {
                        yield return DownloadFile(urlRealtimeDM, tempFile, cookie);
                        byte[] protobufBytes = File.ReadAllBytes(tempFile);
                        try
                        {
                            AddProtobufDmToSet(protobufBytes, danmakus);
                            break;
                        }
                        catch
                        {
                            Debug.LogError(System.Text.Encoding.UTF8.GetString(protobufBytes));
                        }
                        yield return new WaitForSeconds(retryDelay);
                    }
                    yield return new WaitForSeconds(delay);
                }

                DateTime seqDate = endDate;
                //获取历史弹幕
                while (true)
                {
                    string urlHistoryDM = $"http://api.bilibili.com/x/v2/dm/web/history/seg.so?type=1&oid={episode.cid}&date={seqDate:yyyy-MM-dd}";
                    HashSet<Danmaku> hisDms;
                    Debug.Log(seqDate);
                    while (true)
                    {
                        yield return DownloadFile(urlHistoryDM, tempFile, cookie);
                        hisDms = new HashSet<Danmaku>();
                        byte[] protobufBytes = File.ReadAllBytes(tempFile);
                        try
                        { 
                            AddProtobufDmToSet(protobufBytes, hisDms);
                            break;
                        }
                        catch
                        { 
                            Debug.LogError(System.Text.Encoding.UTF8.GetString(protobufBytes));
                        }
                        yield return new WaitForSeconds(retryDelay);
                    }
                    DateTime firstDateInSet = seqDate;
                    foreach (var danmaku in hisDms)
                    {
                        if (danmaku.SendTime < seqDate)
                            firstDateInSet = danmaku.SendTime;
                    }
                    danmakus.UnionWith(hisDms);
                    if (firstDateInSet.Date == seqDate.Date)
                        break;
                    else
                        seqDate = firstDateInSet;
                    yield return new WaitForSeconds(delay);
                }

                FileStream fileStream = File.Create(file);
                GetProtobufDm(danmakus).WriteTo(fileStream);
                fileStream.Close();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log(danmakus.Count);
            }

            ChapterDanmakuSet chapterDanmakuSet = ScriptableObject.CreateInstance<ChapterDanmakuSet>();
            for (int ep = 0; ep < seasonRoot.result.episodes.Length; ep++)
            {
                BilibiliAPI.Season.Episode episode = seasonRoot.result.episodes[ep];
                string file = Path.Combine(folder, $"{season.fileName}_{ep + 1}.bytes");
                chapterDanmakuSet.chapterDanmakus.Add(new ChapterDanmaku($"第{ep + 1}话", episode.long_title, (float)episode.duration / 1000,
                new List<TextAsset> { AssetDatabase.LoadAssetAtPath<TextAsset>(file) }));
            }

            AssetDatabase.CreateAsset(chapterDanmakuSet, Path.Combine(folder, $"{season.fileName}.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void AddProtobufDmToSet(byte[] protobufBytes,HashSet<Danmaku> set)
        {
            Bilibili.Community.Service.Dm.V1.DmSegMobileReply dmSegMobileReply = Bilibili.Community.Service.Dm.V1.DmSegMobileReply.Parser.ParseFrom(protobufBytes);
            foreach (Bilibili.Community.Service.Dm.V1.DanmakuElem danmakuElem in dmSegMobileReply.Elems)
            {
                Danmaku danmaku = new Danmaku(danmakuElem);
                if (set.Contains(danmaku))
                    Debug.Log(danmaku.content);
                set.Add(danmaku);   
            }
        }

        Bilibili.Community.Service.Dm.V1.DmSegMobileReply GetProtobufDm(HashSet<Danmaku> set)
        {
            Bilibili.Community.Service.Dm.V1.DmSegMobileReply dmSegMobileReply = new Bilibili.Community.Service.Dm.V1.DmSegMobileReply();
            List<Danmaku> danmakus = new List<Danmaku>(set);
            danmakus.Sort((x, y) => x.time.CompareTo(y.time));
            foreach (var danmaku in danmakus)
            {
                dmSegMobileReply.Elems.Add(danmaku.ToDanmakuElem());
            }
            return dmSegMobileReply;
        }
#endif
    }
}
