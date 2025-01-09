using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnUI : MonoBehaviour ,IUpdatable
{
    private void Start()
    {
        gameObject.SetActive(false);
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
        transform.LookAt(Camera.main.transform.position);
    }
    public void LateUpdateWork() { }

}
