using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : SingleTon<DialogueManager>
{
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public NPC npc;
    private bool isActive = false;
    private int currentIndex = 0;
    public void Start()
    {
        dialogueCanvas.SetActive(isActive);
    }
    public void Interact(GameObject sceletObject)
    {
        npc = sceletObject.GetComponent<NPC>();
        dialogueCanvas.SetActive(GetMessage());

    }

    private bool GetMessage()
    {

        if(npc.HasQuest)
        {
            DataManager.Instance.dicQuestDatas[npc.QuestList[npc.QuestIndex]].isAccept = true;
            return FindMessage(npc.QuestList[npc.QuestIndex]);

        }
        else
        {
            return FindMessage(npc.DialogueBase);
        }

   
    }

    private bool FindMessage(int id)
    {
        if (DataManager.Instance.dicDialogueDatas[id].dialogueList.Length <= currentIndex)
        {
            currentIndex = 0;
            return false;
        }
        dialogueText.text = DataManager.Instance.dicDialogueDatas[id].dialogueList[currentIndex++];

        return true;
    }
}
