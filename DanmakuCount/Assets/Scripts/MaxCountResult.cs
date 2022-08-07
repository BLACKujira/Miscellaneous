namespace CitrusDammakuCount
{
    [System.Serializable]
    public class MaxCountResult
    {
        public MaxCountResultItem<int> maxCount_Orange = new MaxCountResultItem<int>(0, 0, 0);
        public MaxCountResultItem<int> maxCount_Green = new MaxCountResultItem<int>(0, 0, 0);
        public MaxCountResultItem<float> maxPercent_Orange = new MaxCountResultItem<float>(0, 0, 0);
        public MaxCountResultItem<float> maxPercent_Green = new MaxCountResultItem<float>(0, 0, 0);
    }
}