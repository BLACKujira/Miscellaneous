using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace CitrusDammakuCount.TotalCounter
{
    public class TotalCounter : MonoBehaviour
    {
        public TextAsset[] countResults;
        public int maxNumber = 20;
        public string savePath;


#if UNITY_EDITOR

        private void Awake()
        {
            CountResultSave[] countResults = new CountResultSave[this.countResults.Length];
            for (int i = 0; i < countResults.Length; i++)
                countResults[i] = JsonUtility.FromJson<CountResultSave>(this.countResults[i].text);

            TotalCount_Anime_OrangePercent(countResults);
            TotalCount_Anime_GreenPercent(countResults);
            TotalCount_Chapter_OrangePercent(countResults);
            TotalCount_Chapter_GreenPercent(countResults);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void TotalCount_Anime_OrangePercent(CountResultSave[] countResults)
        {
            List<CountResultSave> countResultList = new List<CountResultSave>(countResults);
            countResultList.Sort((x, y) => -((float)x.OrangeDanmakuCount / x.SampleDanmakuCount).CompareTo((float)y.OrangeDanmakuCount / y.SampleDanmakuCount));
            RankTable rankTable = ScriptableObject.CreateInstance<RankTable>();
            rankTable.rankTableItems = (from CountResultSave cr in countResultList
                                        select new RankTable.RankTableItem(cr.animeName, $"橙色弹幕占比 {100f * cr.OrangeDanmakuCount / cr.SampleDanmakuCount:0.000}%")).ToArray();
            AssetDatabase.CreateAsset(rankTable, Path.Combine(savePath, "Anime_OrangePercent.asset"));
        }

        void TotalCount_Anime_GreenPercent(CountResultSave[] countResults)
        {
            List<CountResultSave> countResultList = new List<CountResultSave>(countResults);
            countResultList.Sort((x, y) => -((float)x.GreenDanmakuCount / x.SampleDanmakuCount).CompareTo((float)y.GreenDanmakuCount/ y.SampleDanmakuCount));
            RankTable rankTable = ScriptableObject.CreateInstance<RankTable>();
            rankTable.rankTableItems = (from CountResultSave cr in countResultList
                                        select new RankTable.RankTableItem(cr.animeName, $"绿色弹幕占比 {100f * cr.GreenDanmakuCount / cr.SampleDanmakuCount:0.000}%")).ToArray();
            AssetDatabase.CreateAsset(rankTable, Path.Combine(savePath, "Anime_GreenPercent.asset"));
        }

        void TotalCount_Chapter_OrangePercent(CountResultSave[] countResults)
        {
            List<KeyValuePair<string,float>> chapterCountResults = new List<KeyValuePair<string, float>>();
            foreach (var countResult in countResults)
            {
                for (int i = 0; i < countResult.chapterCountResults.Length; i++)
                {
                    ChapterCountResultSave chapterCountResult = countResult.chapterCountResults[i];
                    chapterCountResults.Add(new KeyValuePair<string, float>($"{countResult.animeName} 第{i + 1}话",
                        (float)chapterCountResult.OrangeDanmakuCount / chapterCountResult.sampleDanmakuCount));
                }
            }
            chapterCountResults.Sort((x, y) => -x.Value.CompareTo(y.Value));
            RankTable rankTable = ScriptableObject.CreateInstance<RankTable>();
            List<RankTable.RankTableItem> rankTableItems = new List<RankTable.RankTableItem>();
            for (int i = 0; i < maxNumber&&i<chapterCountResults.Count; i++)
            {
                rankTableItems.Add(new RankTable.RankTableItem(chapterCountResults[i].Key, $"橙色弹幕占比 {100f * chapterCountResults[i].Value:0.000}%"));
            }
            rankTable.rankTableItems = rankTableItems.ToArray();
            AssetDatabase.CreateAsset(rankTable, Path.Combine(savePath, "Chapter_OrangePercent.asset"));
        }

        void TotalCount_Chapter_GreenPercent(CountResultSave[] countResults)
        {
            List<KeyValuePair<string, float>> chapterCountResults = new List<KeyValuePair<string, float>>();
            foreach (var countResult in countResults)
            {
                for (int i = 0; i < countResult.chapterCountResults.Length; i++)
                {
                    ChapterCountResultSave chapterCountResult = countResult.chapterCountResults[i];
                    chapterCountResults.Add(new KeyValuePair<string, float>($"{countResult.animeName} 第{i + 1}话",
                        (float)chapterCountResult.GreenDanmakuCount / chapterCountResult.sampleDanmakuCount));
                }
            }
            chapterCountResults.Sort((x, y) => -x.Value.CompareTo(y.Value));
            RankTable rankTable = ScriptableObject.CreateInstance<RankTable>();
            List<RankTable.RankTableItem> rankTableItems = new List<RankTable.RankTableItem>();
            for (int i = 0; i < maxNumber && i < chapterCountResults.Count; i++)
            {
                rankTableItems.Add(new RankTable.RankTableItem(chapterCountResults[i].Key, $"绿色弹幕占比 {100f * chapterCountResults[i].Value:0.000}%"));
            }
            rankTable.rankTableItems = rankTableItems.ToArray();
            AssetDatabase.CreateAsset(rankTable, Path.Combine(savePath, "Chapter_GreenPercent.asset"));
        }
#endif
    }
}
