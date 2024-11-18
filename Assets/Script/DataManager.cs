using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : SingleTon<DataManager>, IUpdatable
{

    public Dictionary<int, ItemData> dicItemDatas = new Dictionary<int, ItemData>();

    


    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, true, false);
    }
    void Start()
    {

        var loadedJson = Resources.Load<TextAsset>("ItemData").text;
        var data = JsonUtility.FromJson<ItemDataArray>(loadedJson);

        foreach (var itemData in data.ItemDatas)
        {
            dicItemDatas.Add(itemData.id, itemData);
        }


    }

    public void UpdateWork() { }

    public void FixedUpdateWork() { }

    public void LateUpdateWork() { }


 

}
