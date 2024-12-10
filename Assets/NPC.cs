using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private bool hasQuest;
    [SerializeField]
    private int npcID;
    [SerializeField]
    private int[] questList;
    [SerializeField]
    private int dialogueBase;
    private int questIndex = 0;

    public int DialogueBase { get => dialogueBase; set => dialogueBase = value; }

    public int[] QuestList { get => questList; set => questList = value; }
    public int QuestIndex { get => questIndex; set => questIndex = value; }
    public bool HasQuest { get => hasQuest; set => hasQuest = value; }

    private void Update()
    {
        if(questList.Length>=0 && QuestIndex<questList.Length)
        {
            if (DataManager.Instance.dicQuestDatas[questList[QuestIndex]].isCleared)
            {
                QuestIndex++;
            }
        }
    }

}
