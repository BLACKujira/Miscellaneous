using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitrusDammakuCount.UI
{
    public class MoveAndFade : MonoBehaviour
    {
        public RectTransform rectTransform;
        public GraphicsAlphaController graphicsAlphaController;
        [Header("Settings")]
        public Vector2 offset;
        public float time;

        Vector2 originPos;
        private void Awake()
        {
            originPos = rectTransform.anchoredPosition;
            graphicsAlphaController.Alpha = 0;
            rectTransform.anchoredPosition = rectTransform.anchoredPosition + offset;
        }

        public void TurnOn()
        {
            DOTween.To(() => graphicsAlphaController.Alpha, v => graphicsAlphaController.Alpha = v, 1, time);
            rectTransform.DOAnchorPos(originPos, time);
        }
    }
}