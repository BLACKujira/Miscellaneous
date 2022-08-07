using UnityEngine;
using XCharts.Runtime;

namespace CitrusDammakuCount.AnimeWindow
{
    public class AnimeWindow_Page2 : MonoBehaviour
    {
        public PieChart pieChart_Orange;
        public PieChart pieChart_Green;
        public BarChart barChart;

        public void Initialize(CountResultSave countResult)
        {
            pieChart_Orange.series[0].ClearData();
            pieChart_Green.series[0].ClearData();
            barChart.series[0].ClearData();
            barChart.series[1].ClearData();
            XAxis barXAxis = barChart.GetChartComponent<XAxis>();
            barXAxis.ClearData();

            for (int i = 0; i < countResult.chapterCountResults.Length; i++)
            {
                ChapterCountResultSave chapterCountResult = countResult.chapterCountResults[i];
                string name = $"第{i + 1}话";
                pieChart_Orange.series[0].AddXYData(i + 1, chapterCountResult.OrangeDanmakuCount).name = name;
                pieChart_Green.series[0].AddXYData(i + 1, chapterCountResult.GreenDanmakuCount).name = name;
                barChart.series[0].AddXYData(i + 1, 
                        (float) chapterCountResult.OrangeDanmakuCount / chapterCountResult.sampleDanmakuCount).name = name;
                barChart.series[1].AddXYData(i + 1,
                        (float)chapterCountResult.GreenDanmakuCount / chapterCountResult.sampleDanmakuCount).name = name;
                barXAxis.AddData(name);
            }
        }
    }
}