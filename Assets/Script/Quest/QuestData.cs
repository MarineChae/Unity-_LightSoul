using System;



[Serializable]
public class QuestData 
{
    public int questIndex;
    public string questName;
    public string questType;
    public string questTarget;
    public int nextQuestIndex;
    public int currentCount;
    public int count;
    public bool isAccept;
    public bool isCleared;
}


[Serializable]
public class QuestDataArray
{
    public QuestData[] QuestDatas;
}
