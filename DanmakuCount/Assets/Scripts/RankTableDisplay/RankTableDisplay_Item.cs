using CitrusDammakuCount.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CitrusDammakuCount.RankTableDisplay
{
    public class RankTableDisplay_Item : MonoBehaviour
    {
        public Text text_Rank;
        public Text text_Key;
        public Text text_Value;
        public RectTransform rectTransform_Preview;
        public Image image_Preview;
        public GraphicsAlphaController graphicsAlphaController;
        [Header("Settings")]
        public float maxAngle = 5;
        public float fadeTime = 1;

        private void Awake()
        {
            graphicsAlphaController.Alpha = 0;
        }

        public void Initialize(int rank, RankTable.RankTableItem rankTableItem)
        {
            text_Rank.text = rank.ToString();
            text_Key.text = rankTableItem.key;
            text_Value.text = rankTableItem.value;
            image_Preview.sprite = rankTableItem.preview;
            rectTransform_Preview.rotation = Quaternion.Euler(0, 0, Random.Range(-maxAngle, maxAngle));
        }

        public void TurnOn()
        {
            DOTween.To(() => graphicsAlphaController.Alpha, v => graphicsAlphaController.Alpha = v, 1, fadeTime);
        }
        public void TurnOff()
        {
            DOTween.To(() => graphicsAlphaController.Alpha, v => graphicsAlphaController.Alpha = v, 0, fadeTime);
        }
    }
}