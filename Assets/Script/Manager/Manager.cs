using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    static Manager instance;
    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        GameObject update = new GameObject("UpdateManager");
        update.transform.SetParent(transform);
        update.AddComponent<UpdateManager>();

        GameObject data = new GameObject("DataManager");
        data.transform.SetParent(transform);
        data.AddComponent<DataManager>();

        GameObject dialogue = new GameObject("DialogueManager");
        dialogue.transform.SetParent(transform);
        dialogue.AddComponent<DialogueManager>();

        GameObject eventm = new GameObject("EventManager");
        eventm.transform.SetParent(transform);
        eventm.AddComponent<EventManager>();

        GameObject quest = new GameObject("QuestManager");
        quest.transform.SetParent(transform);
        quest.AddComponent<QuestManager>();

        GameObject UI = new GameObject("UIManager");
        UI.transform.SetParent(transform);
        UI.AddComponent<UIManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }







}
