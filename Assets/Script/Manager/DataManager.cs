using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : SingleTon<DataManager>
{

    public Dictionary<int, ItemData> dicItemDatas = new Dictionary<int, ItemData>();
    public Dictionary<int, MonsterData> dicMonsterDatas = new Dictionary<int, MonsterData>();
    public Dictionary<int, MonsterSkillData> dicMonsterSkillDatas = new Dictionary<int, MonsterSkillData>();
    public Dictionary<int, DialogueData> dicDialogueDatas = new Dictionary<int, DialogueData>();
    public Dictionary<int, QuestData> dicQuestDatas = new Dictionary<int, QuestData>();
    public Dictionary<string, Action<GameObject>> dicBehaviorFuncs = new Dictionary<string, Action<GameObject>>();

    void Awake()
    {
        LoadItemData();
        LoadMonsterData();
        LoadDialogueData();
        LoadQuestData();
    }
    private void LoadBehaviorTree(string treeName)
    {
        Type componentType = Type.GetType(treeName);
        if (componentType != null)
        {
            if(!dicBehaviorFuncs.ContainsKey(treeName))
                dicBehaviorFuncs.Add(treeName, obj => obj.AddComponent(componentType));
        }
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
        LoadMonsterSkillData();

        var loadedJson = Resources.Load<TextAsset>("JsonDatas/MonsterData").text;
        var data = JsonUtility.FromJson<MonsterDataArray>(loadedJson);
        foreach (var monsterData in data.MonsterDatas)
        {
            dicMonsterDatas.Add(monsterData.id, monsterData);
            LoadBehaviorTree(monsterData.behaviorTreeName);
        }
    }

    private void LoadMonsterSkillData()
    {
        var loadedJson = Resources.Load<TextAsset>("JsonDatas/MonsterSkillData").text;
        var data = JsonUtility.FromJson<MonsterSkillDataArray>(loadedJson);
        foreach (var monsterSkillData in data.MonsterSkillDatas)
        {
            dicMonsterSkillDatas.Add(monsterSkillData.id, monsterSkillData);
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
 

}
