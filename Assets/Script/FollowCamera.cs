
using System;
using UnityEngine;


public class FollowCamera : MonoBehaviour, IUpdatable
{

    [SerializeField]  private Transform camPos;
    [SerializeField]  private Vector3 offSet;
    [SerializeField]  private float cameraSensitive;
    [SerializeField]  private Transform camTarget;
    private Vector2 camLook;
    private bool isUIActive;
    private float ditanceToTarget = 3.0f;
    private PlayerCharacter player;
    private LockOn lockOn;
    private int ignoreLayer;
    public Transform CamPos { get => camPos; set => camPos = value; }
    public Vector3 OffSet { get => offSet; }
    public Vector2 CamLook { get => camLook; set => camLook = value; }
    public bool IsUIActive { get => isUIActive; set => isUIActive = value; }

    private void Awake()
    {
        player = FindObjectOfType<PlayerCharacter>();
        lockOn = player.GetComponentInChildren<LockOn>();
        ignoreLayer = LayerMask.NameToLayer("Player");
    }
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, true, false);
    }

    public void FixedUpdateWork() 
    {
        camPos.position = Vector3.Slerp(camPos.position, camTarget.position + OffSet, 5.0f * Time.fixedDeltaTime);
    }
    public void UpdateWork()
    {

        if (!isUIActive)
        {
            CheckObstruct();

            if (player.IsLockOn)
                TargetLook(lockOn.Target.transform);
            else
                CameraLook();
        }


    }

    private void TargetLook(Transform target)
    {
        Vector3 direction = target.position - camPos.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        camPos.rotation = Quaternion.Slerp(camPos.rotation, targetRotation, cameraSensitive * Time.deltaTime);
    }

    private void CheckObstruct()
    {
        Ray ray = new Ray(camPos.position, transform.position - camPos.position);
        Debug.DrawRay(camPos.position, transform.position- camPos.position, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, ditanceToTarget,ignoreLayer))
        {
            transform.position = hit.point;
        }
    }

    public void CameraLook()
    {
        Vector2 mouseDelta = new Vector2 (camLook.x, camLook.y)*Time.deltaTime * cameraSensitive;
        Vector3 cameraAngle = camPos.rotation.eulerAngles;
        float x = cameraAngle.x - mouseDelta.y;
        if(x <180.0f)
        {
            x = Mathf.Clamp(x , -1f, 45f);
        }
        else
        {
            x = Mathf.Clamp(x, 320f, 361f);
        }
        camPos.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);

    }

    public void LateUpdateWork() { }

}
//y -
//x - 
//z +