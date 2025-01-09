
using System;
using UnityEngine;


public class FollowCamera : MonoBehaviour, IUpdatable
{

    [SerializeField]  private Transform camPos;
    [SerializeField]  private Transform camTarget;
    [SerializeField] private Vector3 offSet;
    [SerializeField] private float cameraSensitive;
    private PlayerCharacter player;
    private LockOn lockOn;
    private Vector2 camLook;
    private bool isUIActive;
    private readonly float ditanceToTarget = 3.0f;
    private int ignoreLayer;
    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, true, false);
    }
    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, true, false);
    }
    private void Awake()
    {
        player = FindObjectOfType<PlayerCharacter>();
        lockOn = player.GetComponentInChildren<LockOn>();
        ignoreLayer = LayerMask.NameToLayer("Player");
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
                TargetLook(lockOn.Target.LockOnPosition);
            else
                CameraLook();
        }
    }
    public void LateUpdateWork() { }
    /////////////////////////////// Private Method///////////////////////////////////

    //Lockon 되었을 때 타겟을 향해 카메라를 고정하는 메서드
    private void TargetLook(Vector3 target)
    {
        Vector3 direction = target - camPos.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        camPos.rotation = Quaternion.Slerp(camPos.rotation, targetRotation, cameraSensitive * Time.deltaTime);
    }

    //장애물이 카메라와 플레이어 사이에 있는경우 카메라를 장애물 앞으로 이동하는 메서드
    private void CheckObstruct()
    {
        Ray ray = new Ray(camPos.position, transform.position - camPos.position);
        Debug.DrawRay(camPos.position, transform.position- camPos.position, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, ditanceToTarget,ignoreLayer))
        {
            transform.position = hit.point;
        }
    }

    //마우스로 카메라 조작을 위한 메서드
    private void CameraLook()
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

    /////////////////////////////// Property /////////////////////////////////
    public Transform CamPos { get => camPos; set => camPos = value; }
    public Vector3 OffSet { get => offSet; }
    public Vector2 CamLook { get => camLook; set => camLook = value; }
    public bool IsUIActive { get => isUIActive; set => isUIActive = value; }

}
