using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitrusDammakuCount;
using System.Linq;

public class Temp : MonoBehaviour
{
    [SerializeField] List<TextAsset> countResults = new List<TextAsset>();

    private void Awake()
    {
        //Percent();
        Average();
    }

    private void Average()
    {
        Debug.Log((from TextAsset ta in countResults
                   select JsonUtility.FromJson<CountResultSave>(ta.text).OrangeDanmakuPercent).Average()*100);
    }

    private void Percent()
    {
        List<CountResultSave> countResultSaves = new List<CountResultSave>();
        foreach (var textAsset in countResults)
        {
            CountResultSave item = JsonUtility.FromJson<CountResultSave>(textAsset.text);
            item.animeName = textAsset.name;
            countResultSaves.Add(item);
        }
        countResultSaves.Sort((x, y) => x.OrangeDanmakuPercent.CompareTo(y.OrangeDanmakuPercent));
        foreach (var countResultSave in countResultSaves)
        {
            Debug.Log($"{countResultSave.animeName} : {countResultSave.OrangeDanmakuPercent * 100}");
        }
    }

}
