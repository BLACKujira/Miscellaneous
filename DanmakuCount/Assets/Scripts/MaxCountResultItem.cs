namespace CitrusDammakuCount
{
    [System.Serializable]
    public class MaxCountResultItem<T>
    {
        public int chapter;
        public float startTime;
        public float endTime;
        public T value;

        public MaxCountResultItem(float startTime, float endTime, T value)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.value = value;
        }
    }
}