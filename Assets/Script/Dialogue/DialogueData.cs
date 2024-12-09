using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
