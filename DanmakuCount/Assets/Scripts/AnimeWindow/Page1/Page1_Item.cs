using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;
using DG.Tweening;

namespace CitrusDammakuCount.AnimeWindow.Page1
{
    public class Page1_Item : MonoBehaviour
    {
        public RectTransform rectTransform;
        public Image mask;
        public Text textTitle;
        public Text textOrange;
        public Text textGreen;
        public Text textMax;
        public Text textHalf;
        public LineChart lineChart;
        [Header("Settings")]
        public float fadeInTime = 1;
        public float moveTime = 1;

        private void Awake()
        {
            rectTransform.gameObject.SetActive(false);
        }

        public void TurnOn()
        {
            rectTransform.gameObject.SetActive(true);
            mask.DOFade(0, fadeInTime);
            rectTransform.DOAnchorPosX(0, moveTime);
        }

        public void Initialize(ChapterCountResultSave chapterCountResult,int chapter,int maxValue)
        {
            Serie serieOrange = lineChart.series[0];
            Serie serieGreen = lineChart.series[1];
            serieOrange.ClearData();
            serieGreen.ClearData();

            //int maxValue = 0;
            foreach (var intervalCount in chapterCountResult.intervalCountOrange)
            {
                serieOrange.AddXYData(intervalCount.sec, intervalCount.count);
                //if (intervalCount.count > maxValue) maxValue = intervalCount.count;
            }
            foreach (var intervalCount in chapterCountResult.intervalCountGreen)
            {
                serieGreen.AddXYData(intervalCount.sec, intervalCount.count);
                //if (intervalCount.count > maxValue) maxValue = intervalCount.count;
            }

            XAxis xAxis = lineChart.GetChartComponent<XAxis>();
            xAxis.max = chapterCountResult.intervalCountOrange[chapterCountResult.intervalCountOrange.Length - 1].sec;

            YAxis yAxis = lineChart.GetChartComponent<YAxis>();
            yAxis.max = maxValue;

            textMax.text = maxValue.ToString();
            textHalf.text = (maxValue / 2).ToString();

            textTitle.text = $"µÚ{chapter}»°  µ¯Ä»£º{chapterCountResult.sampleDanmakuCount}";
            textOrange.text = $"éÙÉ«µ¯Ä»£º{100f * (float)chapterCountResult.OrangeDanmakuCount / chapterCountResult.sampleDanmakuCount:0.000}%£¨{chapterCountResult.OrangeDanmakuCount}£©";
            textGreen.text = $"ÂÌÉ«µ¯Ä»£º{100f * (float)chapterCountResult.GreenDanmakuCount / chapterCountResult.sampleDanmakuCount:0.000}%£¨{chapterCountResult.GreenDanmakuCount}£©";
        }
    }
}