using System;

namespace CitrusDammakuCount
{
    [Serializable]
    public class ChapterMetadata
    {
        public string chapter;
        public string title;
        public float duration;

        public ChapterMetadata(ChapterDanmaku chapterDanmaku)
        {
            chapter = chapterDanmaku.chapter;
            title = chapterDanmaku.title;
            duration = chapterDanmaku.duration;
        }
    }
}