using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : SingleTon<EventManager>
{

    public event Action<string, string> onActionTriggerd;
    public event Action<string> onPotionTriggerd;

    public void TriggerAction(string actiontype,string targetName)
    {
        onActionTriggerd?.Invoke(actiontype, targetName);
    }
    public void PotionTriggerAction(string actiontype)
    {
        onPotionTriggerd?.Invoke(actiontype);
    }
    private void OnDestroy()
    {
        Debug.Log("t4etnsekl;tjnseklth");
    }
}
