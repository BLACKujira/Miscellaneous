using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CitrusDammakuCount.UI
{
    public class LogWindow : MonoBehaviour
    {
        public Text _message;
        public RectTransform textTransform;
        public RectTransform scrollTransform;
        public float edgeSize = 25;
        
        public string message
        {
            get => _message.text;
            set
            {
                _message.text = value;
            }
        }

        private void Update()
        {
            textTransform.sizeDelta = new Vector2(textTransform.sizeDelta.x, _message.preferredHeight);
            scrollTransform.sizeDelta = new Vector2(scrollTransform.sizeDelta.x, textTransform.sizeDelta.y + edgeSize);
        }
    }
}