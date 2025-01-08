using System;
using UnityEngine;

/// <summary>
/// 게임내에서 발생하는 이벤트를 관리하기위한 매니저
/// </summary>
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

}
