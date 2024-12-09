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
        if (DataManager.Instance.dicDialogueDatas[npc.DialogueBase].dialogueList.Length <= currentIndex)
        {
            currentIndex = 0;
            return false;
        }
        dialogueText.text = DataManager.Instance.dicDialogueDatas[npc.DialogueBase].dialogueList[currentIndex++];

        return true;
    }

}
