using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CitrusDammakuCount
{
    [Serializable]
    public class ChapterCountResultSave
    {
        public ChapterMetadata chapterMetadata;
        public int collectedDanmakuCount;
        public int sampleDanmakuCount;
        public int OrangeDanmakuCount =>
            (from IntervalCountSave ics in intervalCountOrange
             select ics.count).Sum();
        public int GreenDanmakuCount =>
            (from IntervalCountSave ics in intervalCountGreen
             select ics.count).Sum();
        public float OrangeDanmakuPercent => (float)OrangeDanmakuCount / sampleDanmakuCount;
        public float GreenDanmakuPercent => (float)GreenDanmakuCount / sampleDanmakuCount;

        public int MaxValue
        {
            get
            {
                int maxO = (from IntervalCountSave ics in intervalCountOrange
                            select ics.count).Max();
                int maxG = (from IntervalCountSave ics in intervalCountGreen
                            select ics.count).Max();
                return Mathf.Max(maxO, maxG);
            }
        }

        public IntervalCountSave[] intervalCountOrange;
        public IntervalCountSave[] intervalCountGreen;

        public ChapterCountResultSave(ChapterCountResult chapterCountResult)
        {
            chapterMetadata = chapterCountResult.chapterMetadata;
            collectedDanmakuCount = chapterCountResult.collectedDanmakuCount;
            sampleDanmakuCount = chapterCountResult.SampleDanmakuCount;
            intervalCountOrange = new List<IntervalCountSave>(
                from IntervalCount ic in chapterCountResult.intervalCountOrange
                select new IntervalCountSave(ic.sec, ic.Count)).ToArray();
            intervalCountGreen = new List<IntervalCountSave>(
                    from IntervalCount ic in chapterCountResult.intervalCountGreen
                    select new IntervalCountSave(ic.sec, ic.Count)).ToArray();
        }
    }
}