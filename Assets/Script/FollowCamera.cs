using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowCamera : MonoBehaviour, IUpdatable
{

    [SerializeField] Transform camTarget;
    [SerializeField] Vector3 offSet;
    private float scroollWheel;

    private void Start()
    {
        
    }
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
        float wheelValue = Input.GetAxis("Mouse ScrollWheel");
        if (0 < wheelValue)
        {
            var dir = camTarget.position - transform.position;
            var dist = Vector3.Distance(camTarget.position, transform.position);
            if(dist > 5)
            {
                offSet.x += dir.normalized.x;
                offSet.y += dir.normalized.y;
                offSet.z += dir.normalized.z;
            }

        }
        else if (0 > wheelValue)
        {
            var dir = camTarget.position - transform.position;
            var dist = Vector3.Distance(camTarget.position, transform.position);
            if(dist<10)
            {
                offSet.x -= dir.normalized.x;
                offSet.y -= dir.normalized.y;
                offSet.z -= dir.normalized.z;
            }

        }
        transform.position = camTarget.position + offSet;

    }
    public void LateUpdateWork() { }

}
//y -
//x - 
//z +