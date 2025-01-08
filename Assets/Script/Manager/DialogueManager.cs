using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : SingleTon<DialogueManager>
{
    private GameObject dialogueCanvas;
    private TextMeshProUGUI dialogueText;
    private NPC npc;
    private int currentIndex = 0;

    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dialogueCanvas = GameObject.Find("Dialogue");
        if (dialogueCanvas != null)
        {
            dialogueText = dialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
            dialogueCanvas.SetActive(false);
        }
    }

    /////////////////////////////// private Method///////////////////////////////////
    
    //dataManager���� �޼����� ������������ �޼��� 
    //����Ʈ�� �ִ� npc�� ���� npc�� �����ϱ� ����
    private bool GetMessage()
    {
        if (npc.HasQuest)
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

    /////////////////////////////// public Method///////////////////////////////////
    
    //�÷��̾�� ��ȣ�ۿ��ϴ� npc�� dialouge�� ������������ �޼���
    public bool Interact(GameObject sceletObject)
    {
        npc = sceletObject.GetComponent<NPC>();
        bool ret = GetMessage();
        dialogueCanvas.SetActive(ret);
        return ret;
    }

}
