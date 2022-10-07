using System;
using System.Collections.Generic;
using System.Linq;

namespace CitrusDammakuCount
{
    [Serializable]
    public class CountResultSave
    {
        public string animeName;
        public ChapterCountResultSave[] chapterCountResults;
        public int collectedDanmakuCount;
        public int SampleDanmakuCount=>
            (from ChapterCountResultSave ccrs in chapterCountResults
            select ccrs.sampleDanmakuCount).Sum();
        public int OrangeDanmakuCount =>
            (from ChapterCountResultSave ccrs in chapterCountResults
             select ccrs.OrangeDanmakuCount).Sum();
        public int GreenDanmakuCount =>
                (from ChapterCountResultSave ccrs in chapterCountResults
                 select ccrs.GreenDanmakuCount).Sum();
        public MaxCountResult maxCountResult;

        public int MaxValue =>
            (from ChapterCountResultSave ccrs in chapterCountResults
             select ccrs.MaxValue).Max();

        public CountResultSave(CountResult countResult)
        {
            animeName = countResult.animeName;
            chapterCountResults = new List<ChapterCountResultSave>(
                from ChapterCountResult ccr in countResult.chapterCountResults
                select new ChapterCountResultSave(ccr)).ToArray();
            collectedDanmakuCount = countResult.CollectedDanmakuCount;
            maxCountResult = countResult.maxCountResult;
        }
    }
}