using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class DataManager : SingleTon<DataManager>, IUpdatable
{

    public Dictionary<int, ItemData> dicItemDatas = new Dictionary<int, ItemData>();
    public Dictionary<int, MonsterData> dicMonsterDatas = new Dictionary<int, MonsterData>();
    public Dictionary<int, DialogueData> dicDialogueDatas = new Dictionary<int, DialogueData>();
    public Dictionary<int, QuestData> dicQuestDatas = new Dictionary<int, QuestData>();
    public Dictionary<string, Action<GameObject>> dicBehaviorFuncs = new Dictionary<string, Action<GameObject>>();


    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, true, false);
    }
    void Awake()
    {
        LoadItemData();
        LoadMonsterData();
        LoadDialogueData();
        LoadQuestData();
        //몬스터 추가시 트리를 넣어주어야함
        dicBehaviorFuncs.Add("WolfBehavior", obj => obj.AddComponent<WolfBehavior>());
        dicBehaviorFuncs.Add("MutantBehavior", obj => obj.AddComponent<MutantBehavior>());
        dicBehaviorFuncs.Add("TraningBotBehavior", obj => obj.AddComponent<TraningBotBehavior>());
    }
    private void LoadQuestData()
    {
        var loadedJson = Resources.Load<TextAsset>("JsonDatas/QuestData").text;
        var data = JsonUtility.FromJson<QuestDataArray>(loadedJson);
        foreach (var questData in data.QuestDatas)
        {
            dicQuestDatas.Add(questData.questIndex, questData);
        }
    }

    private void LoadDialogueData()
    {
        var loadedJson = Resources.Load<TextAsset>("JsonDatas/DialogueData").text;
        var data = JsonUtility.FromJson<DialogueDataArray>(loadedJson);
        foreach (var dialogueData in data.DialogueDatas)
        {
            dicDialogueDatas.Add(dialogueData.dialogueID, dialogueData);
        }
    }

    private void LoadMonsterData()
    {
        var loadedJson = Resources.Load<TextAsset>("JsonDatas/MonsterData").text;
        var data = JsonUtility.FromJson<MonsterDataArray>(loadedJson);
        foreach (var monsterData in data.MonsterDatas)
        {
            dicMonsterDatas.Add(monsterData.id, monsterData);
        }
    }

    private void LoadItemData()
    {
        var loadedJson = Resources.Load<TextAsset>("JsonDatas/ItemData").text;
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
