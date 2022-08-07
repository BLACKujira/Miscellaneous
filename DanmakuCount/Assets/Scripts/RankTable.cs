using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitrusDammakuCount
{
    [CreateAssetMenu(menuName = "Citrus/RankTable")]
    public class RankTable : ScriptableObject
    {
        public RankTableItem[] rankTableItems;

        public RankTableItem this[int index]=>rankTableItems[index];

        [System.Serializable]
        public class RankTableItem
        {
            public string key;
            public string value;
            public Sprite preview;

            public RankTableItem(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}