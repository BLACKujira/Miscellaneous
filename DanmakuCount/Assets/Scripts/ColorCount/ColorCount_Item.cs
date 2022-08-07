using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CitrusDammakuCount.ColorCount
{
    public class ColorCount_Item : MonoBehaviour
    {
        public List<Graphic> setColorGraphics = new List<Graphic>();
        public Text colorHex;
        public Text colorCount;

        public void Initialize(Color32 color,int count)
        {
            foreach (var graphic in setColorGraphics)
            {
                graphic.color = color;
            }
            colorHex.text = $"#{color.r:x2}{color.g:x2}{color.b:x2}";
            colorCount.text = count.ToString();
        }
    }
}
