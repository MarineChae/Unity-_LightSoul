
using UnityEngine;


public class FollowCamera : MonoBehaviour, IUpdatable
{

    [SerializeField]  Transform camTarget;
    [SerializeField]  Vector3 offSet;
    [SerializeField]  private float cameraSensitive;

    private Vector2 camLook;
    private bool isUIActive;
    public Transform CamTarget { get => camTarget; set => camTarget = value; }
    public Vector3 OffSet { get => offSet; }
    public Vector2 CamLook { get => camLook; set => camLook = value; }
    public bool IsUIActive { get => isUIActive; set => isUIActive = value; }
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

    public void FixedUpdateWork() 
    {
  
    }
    public void UpdateWork()
    {
        if (!isUIActive)
            CameraLook();

    }

    public void CameraLook()
    {
        Vector2 mouseDelta = new Vector2 (camLook.x, camLook.y)*Time.deltaTime * cameraSensitive;
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