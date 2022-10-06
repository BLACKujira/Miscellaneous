using CitrusDammakuCount.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace CitrusDammakuCount
{
    public class Counter : MonoBehaviour
    {
        public enum LoadMode { XmlDocument, XmlRegex, Protobuf }

        public List<DisplayBehaviour> displayWindow;
        public LogWindow logWindow;
        public PerecntBar perecntBar;
        [Header("Settings")]
        public LoadMode loadMode;
        public float orangeDanmakuRange = 0.05f;
        public float greenDanmakuRange = 0.05f;
        public Color[] orangeColors;
        public Color[] greenColors;
        public float interval = 10;
        public float intervalMaxCount = 60;
        public string startDateTime;
        public string savePath;
        [Header("Danmaku")]
        public ChapterDanmakuSet chapterDanmakuSet;

        private void Awake()
        {
            StartCoroutine(Count(chapterDanmakuSet,AddMessage));
        }

        public IEnumerator Count(ChapterDanmakuSet chapterDanmakuSet,Action<string> showMessage)
        {
            string filename = chapterDanmakuSet.name;
            List<DanmakuSet> danmakuSets;
            switch (loadMode)
            {
                case LoadMode.XmlDocument:
                    danmakuSets = Load_XML(chapterDanmakuSet,showMessage);
                    break;
                case LoadMode.XmlRegex:
                    danmakuSets = Load_Regex(chapterDanmakuSet,showMessage);
                    break;
                case LoadMode.Protobuf:
                    danmakuSets = Load_Protobuf(chapterDanmakuSet,showMessage);
                    break;
                default:
                    yield break;
            }

            bool keepWaiting = true;
            Thread thread = new Thread(() =>
            {
                CountResult countResult = Count(chapterDanmakuSet,danmakuSets,showMessage);
                string json = JsonUtility.ToJson(new CountResultSave(countResult), true);
                File.WriteAllText(Path.Combine(savePath, $"{filename}.json"), json);
                showMessage($"完成");
                priority = 1;
                keepWaiting = false;
            });
            thread.Start();
            while (keepWaiting)
            {
                yield return 1;
            }
        }

        private void Update()
        {
            while (messageQueue.Count>0)
            {
                logWindow.message += $"{DateTime.Now:T} {messageQueue.Dequeue()}\n";
            }
            perecntBar.priority = priority;
        }

        float priority = 0;
        Queue<string> messageQueue = new Queue<string>();
        void AddMessage(string message)
        {
            messageQueue.Enqueue($"{DateTime.Now:T} {message}\n");
        }

        List<DanmakuSet> Load_XML(ChapterDanmakuSet chapterDanmakuSet, Action<string> showMessage)
        {
            List<DanmakuSet> danmakuSets = new List<DanmakuSet>();
            for (int i = 0; i < chapterDanmakuSet.chapterDanmakus.Count; i++)
            {
                ChapterDanmaku chapterDanmaku = chapterDanmakuSet.chapterDanmakus[i];
                
                showMessage($"读取弹幕 {i+1}");
                priority = (float)i / chapterDanmakuSet.chapterDanmakus.Count;
                
                List<XmlDocument> xmlDocuments = new List<XmlDocument>();
                foreach (var textAsset in chapterDanmaku.files)                
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(textAsset.text);
                    xmlDocuments.Add(xmlDocument);
                }
                DanmakuSet danmakuSet = new DanmakuSet(xmlDocuments.ToArray());
                danmakuSets.Add(danmakuSet);
            }
            return danmakuSets;
        }

        Regex regex_Danmaku = new Regex(" *<d p=\".*,.*,.*,.*,.*,.*,.*,.*,.*\">.*</d> *");
        List<DanmakuSet> Load_Regex(ChapterDanmakuSet chapterDanmakuSet, Action<string> showMessage)
        {
            List<DanmakuSet> danmakuSets = new List<DanmakuSet>();
            for (int i = 0; i < chapterDanmakuSet.chapterDanmakus.Count; i++)
            {
                ChapterDanmaku chapterDanmaku = chapterDanmakuSet.chapterDanmakus[i];

                showMessage($"读取弹幕 {i + 1}");
                priority = (float)i / chapterDanmakuSet.chapterDanmakus.Count;

                HashSet<Danmaku> danmakus = new HashSet<Danmaku>();
                foreach (var textAsset in chapterDanmaku.files)
                {
                    using (MemoryStream stream = new MemoryStream(textAsset.bytes))
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            string line = sr.ReadLine();
                            while (line != null)
                            {
                                if (regex_Danmaku.IsMatch(line))
                                {
                                    const string param_start_str = "<d p=\"";
                                    int param_start = line.IndexOf(param_start_str);
                                    if (param_start == -1) continue;
                                    param_start += param_start_str.Length;

                                    const string param_end_str = "\">";
                                    int param_end = line.IndexOf(param_end_str, param_start);
                                    if (param_end == -1) continue;

                                    int content_start = param_end + param_end_str.Length;

                                    int content_end = line.LastIndexOf("</d>");
                                    if (content_end == -1) continue;

                                    string content = line.Substring(content_start, content_end - content_start);
                                    string parameters = line.Substring(param_start, param_end - param_start);
                                    Danmaku danmaku = new Danmaku(content, parameters);

                                    if (danmakus.Contains(danmaku))
                                        Debug.Log(danmaku.content);
                                    danmakus.Add(danmaku);
                                }
                                line = sr.ReadLine();
                            };
                        }
                    }
                }
                DanmakuSet danmakuSet = new DanmakuSet(danmakus);
                danmakuSets.Add(danmakuSet);
            }
            return danmakuSets;
        }

        List<DanmakuSet> Load_Protobuf(ChapterDanmakuSet chapterDanmakuSet, Action<string> showMessage)
        {
            List<DanmakuSet> danmakuSets = new List<DanmakuSet>();
            for (int i = 0; i < chapterDanmakuSet.chapterDanmakus.Count; i++)
            {
                ChapterDanmaku chapterDanmaku = chapterDanmakuSet.chapterDanmakus[i];

                showMessage($"读取弹幕 {i + 1}");
                priority = (float)i / chapterDanmakuSet.chapterDanmakus.Count;

                HashSet<Danmaku> danmakus = new HashSet<Danmaku>();
                foreach (var textAsset in chapterDanmaku.files)
                {
                    Bilibili.Community.Service.Dm.V1.DmSegMobileReply dmSegMobileReply = Bilibili.Community.Service.Dm.V1.DmSegMobileReply.Parser.ParseFrom(textAsset.bytes);
                    danmakus.UnionWith(
                        from Bilibili.Community.Service.Dm.V1.DanmakuElem elem in dmSegMobileReply.Elems
                        select new Danmaku(elem));
                }
                DanmakuSet danmakuSet = new DanmakuSet(danmakus);
                danmakuSets.Add(danmakuSet);
            }
            return danmakuSets;
        }

        CountResult Count(ChapterDanmakuSet chapterDanmakuSet, List<DanmakuSet> danmakuSets, Action<string> showMessage)
        {
            DateTime startDateTime = DateTime.Parse(this.startDateTime);
            List<ChapterCountResult> chapterCountResults = new List<ChapterCountResult>();
            for (int i = 0; i < danmakuSets.Count; i++)
            {
                DanmakuSet danmakuSet = danmakuSets[i];

                showMessage($"分段统计 {i + 1}");
                priority = (float)i / chapterDanmakuSet.chapterDanmakus.Count;

                HashSet<Danmaku> sampleDanmakus = new HashSet<Danmaku>();
                List<IntervalCount> intervalCountsOrange = new List<IntervalCount>();
                List<IntervalCount> intervalCountsGreen = new List<IntervalCount>();
                foreach (var danmaku in danmakuSet.danmakus)
                {
                    if (danmaku.SendTime < startDateTime)
                        continue;

                    while (danmaku.time >= intervalCountsOrange.Count*interval)
                    {
                        intervalCountsOrange.Add(new IntervalCount(intervalCountsOrange.Count * interval));
                        intervalCountsGreen.Add(new IntervalCount(intervalCountsGreen.Count * interval));
                    }

                    sampleDanmakus.Add(danmaku);

                    Color danmakuColor = danmaku.Color;
                    Vector3 vector3DanmakuColor = new Vector3(danmakuColor.r, danmakuColor.g, danmakuColor.b);

                    foreach (var color in orangeColors)
                    {
                        Vector3 vector3ColorA = new Vector3(color.r, color.g, color.b);
                        if((vector3ColorA - vector3DanmakuColor).magnitude<orangeDanmakuRange)
                        {
                            intervalCountsOrange[(int)(danmaku.time / interval)].danmakus.Add(danmaku);
                            break;
                        }
                    }
                    foreach (var color in greenColors)
                    {
                        Vector3 vector3ColorA = new Vector3(color.r, color.g, color.b);
                        if ((vector3ColorA - vector3DanmakuColor).magnitude < greenDanmakuRange)
                        {
                            intervalCountsGreen[(int)(danmaku.time / interval)].danmakus.Add(danmaku);
                            break;
                        }
                    }
                }

                chapterCountResults.Add(new ChapterCountResult
                    (
                        new ChapterMetadata(chapterDanmakuSet.chapterDanmakus[i]),
                        danmakuSet.danmakus.Count,
                        sampleDanmakus,
                        intervalCountsOrange.ToArray(),
                        intervalCountsGreen.ToArray()
                    ));
            }
            CountResult countResult = new CountResult(chapterDanmakuSet.animeName,danmakuSets,chapterCountResults.ToArray());
            countResult.maxCountResult = MaxCount(countResult,showMessage);
            return countResult;
        }

        class MaxCountDanmakuSet
        {
            public readonly float time;
            public readonly int needDanmakuCount;
            public readonly float needDanmakuPercent;

            public MaxCountDanmakuSet(float time, IEnumerable<Danmaku> danmakuSet, HashSet<Danmaku> needDanmakuSet)
            {
                this.time = time;
                HashSet<Danmaku> danmakus = new HashSet<Danmaku>(danmakuSet);
                danmakus.IntersectWith(needDanmakuSet);
                needDanmakuCount = danmakus.Count;
                needDanmakuPercent = (float)needDanmakuCount / danmakuSet.Count();
            }
        }

        void MaxCountSingle(MaxCountResult maxCountResult, ChapterCountResult chapterCountResult, int chapter, bool green = false)
        {
            Comparison<Danmaku> comparison = (x, y) => x.time.CompareTo(y.time);

            int id = 0;
            HashSet<Danmaku> needDanmakuSet =
                green ? chapterCountResult.GreenDanmakus : chapterCountResult.OrangeDanmakus;

            float endTime = 0;
            foreach (var danmaku in chapterCountResult.sampleDanmakus)
            {
                if (danmaku.time > endTime) endTime = danmaku.time;
            }

            List<MaxCountDanmakuSet> maxCountDanmakus = new List<MaxCountDanmakuSet>();

            List<Danmaku> danmakusByTime = new List<Danmaku>(chapterCountResult.sampleDanmakus);
            danmakusByTime.Sort(comparison);
            Queue<Danmaku> unusedDanmaku = new Queue<Danmaku>(danmakusByTime);
            Queue<Danmaku> usingDanmaku = new Queue<Danmaku>();
            for (int time = 0; time < endTime - intervalMaxCount; time++)
            {
                while (usingDanmaku.Count > 0 && usingDanmaku.Peek().time < time)
                    usingDanmaku.Dequeue();
                while (unusedDanmaku.Count > 0 && unusedDanmaku.Peek().time < time + intervalMaxCount)
                    usingDanmaku.Enqueue(unusedDanmaku.Dequeue());
                if (usingDanmaku.Count <= 0)
                    continue;

                MaxCountDanmakuSet maxCountDanmakuSet = new MaxCountDanmakuSet(time, usingDanmaku, needDanmakuSet);
                maxCountDanmakus.Add(maxCountDanmakuSet);
            }
            MaxCountResultItem<int> maxCount = green ? maxCountResult.maxCount_Green : maxCountResult.maxCount_Orange;
            MaxCountResultItem<float> maxPerecnt = green ? maxCountResult.maxPercent_Green : maxCountResult.maxPercent_Orange;
            foreach (var maxCountDanmakuSet in maxCountDanmakus)
            {
                if (maxCountDanmakuSet.needDanmakuCount > maxCount.value)
                {
                    maxCount.chapter = chapter;
                    maxCount.startTime = maxCountDanmakuSet.time;
                    maxCount.endTime = maxCountDanmakuSet.time + intervalMaxCount;
                    maxCount.value = maxCountDanmakuSet.needDanmakuCount;
                }
                if (maxCountDanmakuSet.needDanmakuPercent > maxPerecnt.value)
                {
                    maxPerecnt.chapter = chapter;
                    maxPerecnt.startTime = maxCountDanmakuSet.time;
                    maxPerecnt.endTime = maxCountDanmakuSet.time + intervalMaxCount;
                    maxPerecnt.value = maxCountDanmakuSet.needDanmakuPercent;
                }
            }
        }

        MaxCountResult MaxCount(CountResult countResult, Action<string> showMessage)
        {
            MaxCountResult maxCountResult = new MaxCountResult();
            for (int i = 0; i < countResult.chapterCountResults.Length; i++)
            {
                ChapterCountResult chapterCountResult = countResult.chapterCountResults[i];

                priority = (float)i / countResult.chapterCountResults.Length;
                showMessage($"最大值统计 {i + 1} 橙色");
                MaxCountSingle(maxCountResult, chapterCountResult, i + 1, false);

                priority = (float)(0.5f + i) / countResult.chapterCountResults.Length;
                showMessage($"最大值统计 {i + 1} 绿色");
                MaxCountSingle(maxCountResult, chapterCountResult, i + 1, true);
            }
            return maxCountResult;
        }
    }
}