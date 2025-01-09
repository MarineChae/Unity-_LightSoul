using System;

[Serializable]
public class DialogueData
{
    public int dialogueID;
    public string[] dialogueList;
}

[Serializable]
public class DialogueDataArray
{
    public DialogueData[] DialogueDatas;
}
