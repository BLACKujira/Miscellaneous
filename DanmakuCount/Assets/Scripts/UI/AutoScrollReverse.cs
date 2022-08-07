using System;
using System.Collections;
using UnityEngine;

namespace CitrusDammakuCount.UI
{
    public class AutoScrollReverse : MonoBehaviour
    {
        public RectTransform contentRectTransform;
        [Header("Settings")]
        public float stayTimeBottom;
        public float scrollSpeed;
        public float stayTimeTop;
        public float scrollViewHeight;

        public IEnumerator IPlay(Action onComplete = null)
        {
            float maxYPos = Mathf.Max(0, contentRectTransform.sizeDelta.y - scrollViewHeight);
            contentRectTransform.anchoredPosition = new Vector2
                (contentRectTransform.anchoredPosition.x, maxYPos);
            yield return new WaitForSeconds(stayTimeBottom);
            float stayOnTopTime = 0;
            while (stayOnTopTime < stayTimeTop)
            {
                Vector2 anchoredPosition = contentRectTransform.anchoredPosition;
                anchoredPosition.y -= scrollSpeed * Time.deltaTime;
                if (anchoredPosition.y <= 0)
                {
                    anchoredPosition.y = 0;
                    stayOnTopTime += Time.deltaTime;
                }
                contentRectTransform.anchoredPosition = anchoredPosition;
                yield return 1;
            }

            if (onComplete != null)
                onComplete();
        }
    }
}