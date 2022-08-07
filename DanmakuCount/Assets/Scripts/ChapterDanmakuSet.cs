using System.Collections.Generic;
using UnityEngine;

namespace CitrusDammakuCount
{
    [CreateAssetMenu(menuName = "Citrus/ChapterDanmakuSet")]
    public class ChapterDanmakuSet:ScriptableObject
    {
        public string animeName;
        public List<ChapterDanmaku> chapterDanmakus = new List<ChapterDanmaku>();
    }
}