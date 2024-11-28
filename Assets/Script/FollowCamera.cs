using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class FollowCamera : MonoBehaviour, IUpdatable
{

    [SerializeField]  Transform camTarget;
    [SerializeField]  Vector3 offSet;
    [SerializeField]  private float rayLength;

    public Transform CamTarget { get => camTarget; set => camTarget = value; }
    public Vector3 OffSet { get => offSet; }

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
      

    }

    public void CameraLook(Vector2 inputValue)
    {
        Vector2 mouseDelta = new Vector2 (inputValue.x, inputValue.y);
        Vector3 cameraAngle = CamTarget.rotation.eulerAngles;
        
        float x = cameraAngle.x - mouseDelta.y;
        if(x <180.0f)
        {
            x = Mathf.Clamp(x , -1f, 45f);
        }
        else
        {
            x = Mathf.Clamp(x, 320f, 361f);
        }
        CamTarget.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);

    }

    public void LateUpdateWork() { }

}
//y -
//x - 
//z +