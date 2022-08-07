using System;

namespace CitrusDammakuCount
{
    [Serializable]
    public class IntervalCountSave
    {
        public float sec;
        public int count;

        public IntervalCountSave(float sec, int count)
        {
            this.sec = sec;
            this.count = count;
        }
    }
}