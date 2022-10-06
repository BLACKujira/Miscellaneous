using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CitrusDammakuCount.UI;

namespace CitrusDammakuCount.ColorCount
{
    public class ColorCount : DisplayBehaviour
    {
        public UniversalGeneratorV2 universalGenerator_Other;
        public UniversalGeneratorV2 universalGenerator_Orange;
        public UniversalGeneratorV2 universalGenerator_Green;
        [Header("Prefab")]
        public ColorCount_Item itemPrefab;

        public override void Initialize(CountResult countResult)
        {
            HashSet<Danmaku> orangeDanmakus = new HashSet<Danmaku>();
            HashSet<Danmaku> greenDanmakus = new HashSet<Danmaku>();
            HashSet<Danmaku> otherDanmakus = new HashSet<Danmaku>();

            foreach (var chapterCountResult in countResult.chapterCountResults)
            {
                orangeDanmakus.UnionWith(chapterCountResult.OrangeDanmakus);
                greenDanmakus.UnionWith(chapterCountResult.GreenDanmakus);
                otherDanmakus.UnionWith(chapterCountResult.sampleDanmakus);
            }

            otherDanmakus.ExceptWith(orangeDanmakus);
            otherDanmakus.ExceptWith(greenDanmakus);

            Dictionary<Color32, int> otherCount = new Dictionary<Color32, int>();
            Dictionary<Color32, int> orangeCount = new Dictionary<Color32, int>();
            Dictionary<Color32, int> greenCount = new Dictionary<Color32, int>();

            foreach (var danmaku in otherDanmakus)
            {
                otherCount[danmaku.Color] =
                    otherCount.ContainsKey(danmaku.Color) ?
                    otherCount[danmaku.Color] + 1 :
                    1;
            }
            foreach (var danmaku in orangeDanmakus)
            {
                orangeCount[danmaku.Color] =
                    orangeCount.ContainsKey(danmaku.Color) ?
                    orangeCount[danmaku.Color] + 1 :
                    1;
            }
            foreach (var danmaku in greenDanmakus)
            {
                greenCount[danmaku.Color] =
                    greenCount.ContainsKey(danmaku.Color) ?
                    greenCount[danmaku.Color] + 1 :
                    1;
            }

            foreach (var keyValuePair in otherCount)
            {
                AddItem(universalGenerator_Other,keyValuePair);
            }
            foreach (var keyValuePair in orangeCount)
            {
                AddItem(universalGenerator_Orange,keyValuePair);
            }
            foreach (var keyValuePair in greenCount)
            {
                AddItem(universalGenerator_Green,keyValuePair);
            }
        }

        private void AddItem(UniversalGeneratorV2 universalGenerator,KeyValuePair<Color32, int> keyValuePair)
        {
            universalGenerator.AddItem(itemPrefab.gameObject, (gobj) =>
            {
                ColorCount_Item colorCount_Item = gobj.GetComponent<ColorCount_Item>();
                colorCount_Item.Initialize(keyValuePair.Key, keyValuePair.Value);
            });
        }
    }
}
