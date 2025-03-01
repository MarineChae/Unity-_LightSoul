using System;
using UnityEngine;

/// <summary>
/// 업데이트 최적화를 위한 매니저 클래스
/// </summary>
public class UpdateManager : MonoBehaviour
{
    private static Action updateWorks;
    private static Action fixedUpdateWorks;
    private static Action lateUpdateWorks;

    private void Update()
    {
        updateWorks?.Invoke();
    }
    private void FixedUpdate()
    {
        fixedUpdateWorks?.Invoke();
    }
    private void LateUpdate()
    {
        lateUpdateWorks?.Invoke();
    }

    public static void OnSubscribe(IUpdatable updatable , bool update, bool fixedUpdate , bool lateUpdate)
    {
        if (update)
        {
            updateWorks += updatable.UpdateWork;
        }
        if(fixedUpdate)
        {
            fixedUpdateWorks += updatable.FixedUpdateWork;
        }
        if(lateUpdate)
        {
            lateUpdateWorks += updatable.LateUpdateWork;
        }
    }

    public static void UnSubscribe(IUpdatable updatable, bool update, bool fixedUpdate, bool lateUpdate)
    {
        if (update)
        {
            updateWorks -= updatable.UpdateWork;
        }
        if (fixedUpdate)
        {
            fixedUpdateWorks -= updatable.FixedUpdateWork;
        }
        if (lateUpdate)
        {
            lateUpdateWorks -= updatable.LateUpdateWork;
        }
    }


}
