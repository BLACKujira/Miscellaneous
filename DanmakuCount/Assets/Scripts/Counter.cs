using CitrusDammakuCount.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace CitrusDammakuCount
{
    public class Counter : MonoBehaviour
    {
        public List<DisplayBehaviour> displayWindow;
        public LogWindow logWindow;
        public PerecntBar perecntBar;
        [Header("Settings")]
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

        List<ChapterDanmaku> chapterDanmakus => chapterDanmakuSet.chapterDanmakus;

        List<DanmakuSet> danmakuSets = new List<DanmakuSet>();
        List<ChapterCountResult> chapterCountResults = new List<ChapterCountResult>();
        CountResult countResult;

        private void Awake()
        {
            string filename = chapterDanmakuSet.name;
            Load();
            Thread thread = new Thread(() =>
            {
                Count();
                string json = JsonUtility.ToJson(new CountResultSave(countResult));
                File.WriteAllText(Path.Combine(savePath, $"{filename}.json"), json);
                AddMessage($"完成");
                priority = 1;
            });
            thread.Start();
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
            messageQueue.Enqueue(message);
        }

        void Load()
        {
            for (int i = 0; i < chapterDanmakus.Count; i++)
            {
                ChapterDanmaku chapterDanmaku = chapterDanmakus[i];
                
                AddMessage($"读取弹幕 {i+1}");
                priority = (float)i / chapterDanmakus.Count;
                
                List<XmlDocument> xmlDocuments = new List<XmlDocument>();
                foreach (var textAsset in chapterDanmaku.xmls)                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(textAsset.text);
                    xmlDocuments.Add(xmlDocument);
                }
                DanmakuSet danmakuSet = new DanmakuSet(xmlDocuments.ToArray());
                danmakuSets.Add(danmakuSet);
            }
        }

        void Count()
        {
            DateTime startDateTime = DateTime.Parse(this.startDateTime);
            for (int i = 0; i < danmakuSets.Count; i++)
            {
                DanmakuSet danmakuSet = danmakuSets[i];

                AddMessage($"分段统计 {i + 1}");
                priority = (float)i / chapterDanmakus.Count;

                HashSet<Danmaku> sampleDanmakus = new HashSet<Danmaku>();
                List<IntervalCount> intervalCountsOrange = new List<IntervalCount>();
                List<IntervalCount> intervalCountsGreen = new List<IntervalCount>();
                foreach (var danmaku in danmakuSet.danmakus)
                {
                    if (danmaku.sendTime < startDateTime)
                        continue;

                    while (danmaku.time >= intervalCountsOrange.Count*interval)
                    {
                        intervalCountsOrange.Add(new IntervalCount(intervalCountsOrange.Count * interval));
                        intervalCountsGreen.Add(new IntervalCount(intervalCountsGreen.Count * interval));
                    }

                    sampleDanmakus.Add(danmaku);

                    Color danmakuColor = danmaku.color;
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
                        danmakuSet.danmakus.Count,
                        sampleDanmakus,
                        intervalCountsOrange.ToArray(),
                        intervalCountsGreen.ToArray()
                    ));
            }
            countResult = new CountResult(chapterDanmakuSet.animeName,danmakuSets,chapterCountResults.ToArray());
            countResult.maxCountResult = MaxCount(countResult);
        }

        class MaxCountDanmakuSet
        {
            public readonly float time;
            public HashSet<Danmaku> danmakuSet = new HashSet<Danmaku>();
            HashSet<Danmaku> needDanmakuSet;

            public int NeedDanmakuCount
            {
                get
                {
                    HashSet<Danmaku> danmakus = new HashSet<Danmaku>(danmakuSet);
                    danmakus.IntersectWith(needDanmakuSet);
                    return danmakus.Count;
                }
            }
            public float NeedDanmakuPercent => (float)NeedDanmakuCount / danmakuSet.Count;

            public MaxCountDanmakuSet(float time, HashSet<Danmaku> needDanmakuSet)
            {
                this.time = time;
                this.needDanmakuSet = needDanmakuSet;
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
            for (int time = 0; time < endTime - intervalMaxCount; time++)
            {
                MaxCountDanmakuSet maxCountDanmakuSet = new MaxCountDanmakuSet(time, needDanmakuSet);
                maxCountDanmakuSet.danmakuSet = new HashSet<Danmaku>(
                    from Danmaku d in chapterCountResult.sampleDanmakus
                    where d.time >= time && d.time < time + intervalMaxCount
                    select d);
                maxCountDanmakus.Add(maxCountDanmakuSet);
            }

            MaxCountResultItem<int> maxCount = green ? maxCountResult.maxCount_Green : maxCountResult.maxCount_Orange;
            MaxCountResultItem<float> maxPerecnt = green ? maxCountResult.maxPercent_Green : maxCountResult.maxPercent_Orange;
            foreach (var maxCountDanmakuSet in maxCountDanmakus)
            {
                if (maxCountDanmakuSet.NeedDanmakuCount > maxCount.value)
                {
                    maxCount.chapter = chapter;
                    maxCount.startTime = maxCountDanmakuSet.time;
                    maxCount.endTime = maxCountDanmakuSet.time + intervalMaxCount;
                    maxCount.value = maxCountDanmakuSet.NeedDanmakuCount;
                }
                if (maxCountDanmakuSet.NeedDanmakuPercent > maxPerecnt.value)
                {
                    maxPerecnt.chapter = chapter;
                    maxPerecnt.startTime = maxCountDanmakuSet.time;
                    maxPerecnt.endTime = maxCountDanmakuSet.time + intervalMaxCount;
                    maxPerecnt.value = maxCountDanmakuSet.NeedDanmakuPercent;
                }
            }
        }

        MaxCountResult MaxCount(CountResult countResult)
        {
            MaxCountResult maxCountResult = new MaxCountResult();
            for (int i = 0; i < countResult.chapterCountResults.Length; i++)
            {
                ChapterCountResult chapterCountResult = countResult.chapterCountResults[i];

                AddMessage($"最大值统计 {i + 1}");
                priority = (float)i / chapterDanmakus.Count;

                MaxCountSingle(maxCountResult, chapterCountResult, i + 1, false);
                MaxCountSingle(maxCountResult, chapterCountResult, i + 1, true);
            }
            return maxCountResult;
        }
    }
}