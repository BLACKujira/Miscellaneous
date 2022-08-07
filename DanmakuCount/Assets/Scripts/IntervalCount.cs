using System.Collections.Generic;

namespace CitrusDammakuCount
{
    public class IntervalCount
    {
        public float sec;

        public IntervalCount(float sec)
        {
            this.sec = sec;
        }

        public HashSet<Danmaku> danmakus = new HashSet<Danmaku>();
        public int Count => danmakus.Count;
    }
}