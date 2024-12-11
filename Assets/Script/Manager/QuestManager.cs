using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SingleTon<QuestManager>
{

    private void OnEnable()
    {
        EventManager.Instance.onActionTriggerd += OnActionTriggered;
    }
    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.onActionTriggerd -= OnActionTriggered;
        }
    }
    public void OnActionTriggered(string actionType, string targetName)
    {
        //Kill event�� �߻��� ���s
        if (actionType == "KILL")
        {
            OnMonsterKilled(targetName);
        }
        else if(actionType =="EQUIP")
        {
            OnEquipItem(targetName);
        }
        else if (actionType =="USE")
        {
            OnUseItem(targetName);
        }
    }

    public void OnUseItem(string targetName)
    {
        foreach (var quest in DataManager.Instance.dicQuestDatas)
        {
            if (quest.Value.questType == "USE" && quest.Value.questTarget == targetName && quest.Value.isAccept)
            {
                quest.Value.isCleared = true;
                Debug.Log("USE");

            }
        }
    }

    private void OnMonsterKilled(string targetName)
    {
        foreach (var quest in DataManager.Instance.dicQuestDatas)
        {
            if (quest.Value.questType == "KILL" && quest.Value.questTarget == targetName && quest.Value.isAccept)
            {
                quest.Value.currentCount++;
                if (quest.Value.count >= quest.Value.currentCount)
                {
                    quest.Value.isCleared = true;
                    Debug.Log("KILL");
                }

            }
        }
    }
    private void OnEquipItem(string targetName)
    {
        foreach (var quest in DataManager.Instance.dicQuestDatas)
        {
            if(quest.Value.questType == "EQUIP" && quest.Value.questTarget == targetName && quest.Value.isAccept)
            {
                quest.Value.isCleared = true;
                Debug.Log("EQUIP");
            }
        }
    }
}



//npc�� ����Ʈ ��ȣ����Ʈ�� ������ �ְ�
//npc�� �پ��� ����Ʈ�� ���� �� �ְԸ������ҵ�?
//����Ʈ�� Ŭ���� ������ ������ �־���� �Լ������ͷ� ������ָ� ����������?

//�ƴϸ� ���� ����Ʈ�� ��ũ��Ʈ�� �������ϴµ� ���� �ָ�
//Ŭ�����ϸ� ����Ǵ� ����Ʈ�� ���� �� �����Ƿ� �̰͵� üũ�� �־����
//