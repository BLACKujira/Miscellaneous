using System.Collections.Generic;

namespace CitrusDammakuCount
{
    public class ChapterCountResult
    {
        public ChapterMetadata chapterMetadata;
        public readonly int collectedDanmakuCount;
        public int SampleDanmakuCount => sampleDanmakus.Count;
        public int OrangeDanmakuCount
        {
            get
            {
                int count = 0;
                foreach (var intervalCount in intervalCountOrange)
                {
                    count += intervalCount.Count;
                }
                return count;
            }
        }
        public int GreenDanmakuCount
        {
            get
            {
                int count = 0;
                foreach (var intervalCount in intervalCountGreen)
                {
                    count += intervalCount.Count;
                }
                return count;
            }
        }

        public readonly HashSet<Danmaku> sampleDanmakus;
        public HashSet<Danmaku> OrangeDanmakus
        {
            get
            {
                HashSet<Danmaku> danmakus = new HashSet<Danmaku>();
                foreach (var intervalCount in intervalCountOrange)
                {
                    danmakus.UnionWith(intervalCount.danmakus);
                }
                return danmakus;
            }
        }
        public HashSet<Danmaku> GreenDanmakus
        {
            get
            {
                HashSet<Danmaku> danmakus = new HashSet<Danmaku>();
                foreach (var intervalCount in intervalCountGreen)
                {
                    danmakus.UnionWith(intervalCount.danmakus);
                }
                return danmakus;
            }
        }

        public readonly IntervalCount[] intervalCountOrange;
        public readonly IntervalCount[] intervalCountGreen;

        public ChapterCountResult(ChapterMetadata chapterMetadata, int collectedDanmakuCount, HashSet<Danmaku> sampleDanmakus, IntervalCount[] intervalCountOrange, IntervalCount[] intervalCountGreen)
        {
            this.chapterMetadata = chapterMetadata;
            this.collectedDanmakuCount = collectedDanmakuCount;
            this.sampleDanmakus = sampleDanmakus;
            this.intervalCountOrange = intervalCountOrange;
            this.intervalCountGreen = intervalCountGreen;
        }
    }
}