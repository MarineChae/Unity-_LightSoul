using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] Transform camTarget;
    [SerializeField] Vector3 offSet;

  
    void Update()
    {
        transform.position = camTarget.position + offSet;
    }
}
