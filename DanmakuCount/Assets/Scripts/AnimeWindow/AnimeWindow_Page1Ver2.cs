using System.Collections;
using UnityEngine;
using CitrusDammakuCount.AnimeWindow.Page1;

namespace CitrusDammakuCount.AnimeWindow
{
    public class AnimeWindow_Page1Ver2 : AnimeWindow_Page1
    {
        public int quickShowItemNum = 4;
        public float quickShowItemDelay = .2f;

        protected override IEnumerator ShowItem()
        {
            for (int i = 0; i < quickShowItemNum-1; i++)
            {
                universalGenerator.Items[i].GetComponent<Page1_Item>().TurnOn();
                yield return new WaitForSeconds(quickShowItemDelay);
            }
            for (int i = quickShowItemNum-1; i < universalGenerator.Items.Count; i++)
            {
                universalGenerator.Items[i].GetComponent<Page1_Item>().TurnOn();
                yield return new WaitForSeconds(showItemDelay);
            }
        }
    }
}