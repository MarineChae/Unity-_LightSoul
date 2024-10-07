using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowCamera : MonoBehaviour, IUpdatable
{

    [SerializeField] Transform camTarget;
    [SerializeField] Vector3 offSet;
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }

    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        transform.position = camTarget.position + offSet;

    }
    public void LateUpdateWork() { }

}
