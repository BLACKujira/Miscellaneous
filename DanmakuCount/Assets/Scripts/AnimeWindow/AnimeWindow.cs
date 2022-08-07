using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

namespace CitrusDammakuCount.AnimeWindow
{
    public class AnimeWindow : DisplayBehaviour
    {
        public AnimeWindow_Page1 page1;
        public AnimeWindow_Page2 page2;
        public AnimeWindow_Page3 page3;
        public Image maskImage;
        public Text collectedDanmaku;
        public Text sampleDanmaku;
        [Header("Settings")]
        public float page2Staytime;
        public float page3Staytime;
        public float pageFadetime = 1;
        public TextAsset countResultSave;

        private void Awake()
        {
            maskImage.color = new Color(1, 1, 1, 0);
            Initialize(JsonUtility.FromJson<CountResultSave>(countResultSave.text));
        }

        void ChangePage(GameObject pagePrev,GameObject pageNext)
        {
            maskImage.DOFade(1, pageFadetime / 2).OnComplete(()=>
            {
                pagePrev.gameObject.SetActive(false);
                pageNext.gameObject.SetActive(true);
                maskImage.DOFade(0, pageFadetime / 2);
            });
        }

        public void Initialize(CountResultSave countResult)
        {
            page1.Initialize(countResult);
            page2.Initialize(countResult);
            page3.Initialize(countResult);
            collectedDanmaku.text = $"抓取弹幕 ： {(float)countResult.collectedDanmakuCount/10000:0.00}万";
            sampleDanmaku.text = $"样本弹幕 ： {(float)countResult.SampleDanmakuCount / 10000:0.00}万";
            StartCoroutine(Play());
        }

        public override void Initialize(CountResult countResult)
        {
            Initialize(new CountResultSave(countResult));
        }

        IEnumerator Play()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    break;
                yield return 1;
            }
            yield return page1.Play();
            ChangePage(page1.gameObject, page2.gameObject);
            yield return new WaitForSeconds(page2Staytime);
            ChangePage(page2.gameObject, page3.gameObject);
            yield return new WaitForSeconds(page3Staytime);
        }
    }
}