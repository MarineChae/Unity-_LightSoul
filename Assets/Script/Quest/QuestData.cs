using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LookDev;

[Serializable]
public class QuestData 
{

    public string questName;
    public int[] npcID;

}


[Serializable]
public class QuestDataArray
{
    public QuestDataArray[] QuestDatas;
}
