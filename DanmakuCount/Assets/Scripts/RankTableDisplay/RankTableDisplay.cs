using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CitrusDammakuCount.UI;
using DG.Tweening;

namespace CitrusDammakuCount.RankTableDisplay
{
    public class RankTableDisplay : MonoBehaviour
    {
        public RankTable rankTable;
        public UniversalGeneratorV2 universalGenerator;
        public AutoScrollReverse autoScroll;
        public GraphicsAlphaController alphaController_Title;
        [Header("Settings")]
        public float turnOnDelay;
        public float turnOffAfter;
        public float fadeTime = 1;
        [Header("Prefab")]
        public RankTableDisplay_Item itemPrefab;

        public void Awake()
        {
            alphaController_Title.Alpha = 0;
            for (int i = 0; i < rankTable.rankTableItems.Length; i++)
            {
                int id = i;
                RankTable.RankTableItem rankTableItem = rankTable.rankTableItems[i];
                universalGenerator.AddItem(itemPrefab.gameObject, (gobj) => 
                {
                    RankTableDisplay_Item rankTableDisplay_Item = gobj.GetComponent<RankTableDisplay_Item>();
                    rankTableDisplay_Item.Initialize(id+1,rankTableItem);
                });
            }

            StartCoroutine(Play());
        }

        IEnumerator Play()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    break;
                yield return 1;
            }
            StartCoroutine(TurnOn());
            DOTween.To(() => alphaController_Title.Alpha, v => alphaController_Title.Alpha = v, 1, fadeTime);
            yield return 1;
            yield return autoScroll.IPlay();
            DOTween.To(() => alphaController_Title.Alpha, v => alphaController_Title.Alpha = v, 0, fadeTime);
        }
        IEnumerator TurnOn()
        {
            for (int i = universalGenerator.Items.Count - 1; i >= 0; i--)
            {
                RectTransform item = universalGenerator.Items[i];
                RankTableDisplay_Item rankTableDisplay_Item = item.GetComponent<RankTableDisplay_Item>();
                rankTableDisplay_Item.TurnOn();
                StartCoroutine(TurnOff(rankTableDisplay_Item));
                yield return new WaitForSeconds(turnOnDelay);
            }
        }

        IEnumerator TurnOff(RankTableDisplay_Item item)
        {
            yield return new WaitForSeconds(turnOffAfter);
            item.TurnOff();
        }
    }
}