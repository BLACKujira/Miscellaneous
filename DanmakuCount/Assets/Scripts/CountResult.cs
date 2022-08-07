using System.Collections.Generic;
using System.Linq;

namespace CitrusDammakuCount
{
    public class CountResult
    {
        public readonly string animeName;
        public readonly ChapterCountResult[] chapterCountResults;
        public readonly List<DanmakuSet> collectedDanmaku = new List<DanmakuSet>();

        public int CollectedDanmakuCount => 
            (from DanmakuSet ds in collectedDanmaku
            select ds.danmakus.Count).Sum();
        public int SampleDanmakuCount =>
            (from ChapterCountResult ccr in chapterCountResults
             select ccr.SampleDanmakuCount).Sum();
        public int OrangeDanmakuCount =>
            (from ChapterCountResult ccr in chapterCountResults
             select ccr.OrangeDanmakuCount).Sum();
        public int GreenDanmakuCount =>
            (from ChapterCountResult ccr in chapterCountResults
            select ccr.GreenDanmakuCount).Sum();

        public MaxCountResult maxCountResult;

        public CountResult(string animeName, List<DanmakuSet> collectedDanmaku, ChapterCountResult[] chapterCountResults)
        {
            this.animeName = animeName;
            this.collectedDanmaku = collectedDanmaku;
            this.chapterCountResults = chapterCountResults;
        }
    }
}