using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitrusDammakuCount.UI;
using CitrusDammakuCount;
using CitrusDammakuCount.AnimeWindow.Page1;
using UnityEngine.UI;

namespace CitrusDammakuCount.AnimeWindow
{

    public class AnimeWindow_Page1 : MonoBehaviour
    {
        public UniversalGeneratorV2 universalGenerator;
        public AutoScroll autoScroll;
        public Text text_OrangeTotal;
        public Text text_GreenTotal;
        [Header("Settings")]
        public float showItemDelay = 1.5f;
        [Header("Prefab")]
        public Page1_Item itemPrefab;

        protected virtual IEnumerator ShowItem()
        {
            foreach (var item in universalGenerator.Items)
            {
                item.GetComponent<Page1_Item>().TurnOn();
                yield return new WaitForSeconds(showItemDelay);
            }
        }

        public void Initialize(CountResultSave countResult)
        {
            for (int i = 0; i < countResult.chapterCountResults.Length; i++)
            {
                ChapterCountResultSave chapterCountResult = countResult.chapterCountResults[i];
                universalGenerator.AddItem(itemPrefab.gameObject, (gobj) =>
                 {
                     Page1_Item page1_Item = gobj.GetComponent<Page1_Item>();
                     page1_Item.Initialize(chapterCountResult,i + 1,countResult.MaxValue);
                 });
            }
            text_OrangeTotal.text = $"橘色弹幕总计：{countResult.OrangeDanmakuCount}（{100f * countResult.OrangeDanmakuCount/countResult.SampleDanmakuCount:0.000}%）";
            text_GreenTotal.text = $"绿色弹幕总计：{countResult.GreenDanmakuCount}（{100f * countResult.GreenDanmakuCount/countResult.SampleDanmakuCount:0.000}%）";
        }

        public IEnumerator Play()
        {
            yield return 1;
            StartCoroutine(ShowItem());
            yield return autoScroll.IPlay();
        }
    }
}