using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CitrusDammakuCount.AnimeWindow
{
    public class AnimeWindow_Page3 : MonoBehaviour
    {
        public Text text_MaxCount_Orange;
        public Text text_MaxCount_Green;
        public Text text_MaxPercent_Orange;
        public Text text_MaxPercent_Green;
        [Header("Settings")]
        public int interval = 60;

        string GetTime(int sec)
        {
            return $"{sec/60}:{sec%60}";
        }

        public void Initialize(CountResultSave countResult)
        {
            MaxCountResult maxCountResult = countResult.maxCountResult;

            text_MaxCount_Orange.text = $@"在第 {maxCountResult.maxCount_Orange.chapter} 话 {GetTime((int)maxCountResult.maxCount_Orange.startTime)} - {GetTime((int)maxCountResult.maxCount_Orange.endTime)} 之间
共统计到 {maxCountResult.maxCount_Orange.value} 条橘色弹幕";

            text_MaxCount_Green.text = $@"在第 {maxCountResult.maxCount_Green.chapter} 话 {GetTime((int)maxCountResult.maxCount_Green.startTime)} - {GetTime((int)maxCountResult.maxCount_Green.endTime)} 之间
共统计到 {maxCountResult.maxCount_Green.value} 条绿色弹幕";

            text_MaxPercent_Orange.text = $@"在第 {maxCountResult.maxPercent_Orange.chapter} 话 {GetTime((int)maxCountResult.maxPercent_Orange.startTime)} - {GetTime((int)maxCountResult.maxPercent_Orange.endTime)} 之间
{maxCountResult.maxPercent_Orange.value*100f:0.000}% 的弹幕为橘色";

            text_MaxPercent_Green.text = $@"在第 {maxCountResult.maxPercent_Green.chapter} 话 {GetTime((int)maxCountResult.maxPercent_Green.startTime)} - {GetTime((int)maxCountResult.maxPercent_Green.endTime)} 之间
弹幕中有 {maxCountResult.maxPercent_Green.value*100f:0.000}% 的弹幕为绿色";
        }
    }
}