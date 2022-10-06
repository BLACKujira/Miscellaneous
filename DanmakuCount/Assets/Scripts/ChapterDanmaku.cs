using System;
using System.Collections.Generic;
using UnityEngine;

namespace CitrusDammakuCount
{
    [Serializable]
    public class ChapterDanmaku
    {
        public string chapter;
        public string title;
        public float duration;
        public List<TextAsset> files = new List<TextAsset>();

        public ChapterDanmaku(string chapter, string title, float duration)
        {
            this.chapter = chapter;
            this.title = title;
            this.duration = duration;
        }

        public ChapterDanmaku(string chapter, string title, float duration, List<TextAsset> files) : this(chapter, title,duration)
        {
            this.files = files;
        }
    }
}