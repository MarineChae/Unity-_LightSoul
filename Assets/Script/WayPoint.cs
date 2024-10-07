using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class WayPoint : MonoBehaviour, IUpdatable
{

    private float time = 0;
    private Vector3 currentScale;
    private float scalingSize = 10;
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, false, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, false, true, false);
    }

    private void Start()
    {
        currentScale = transform.localScale;
    }
    public void UpdateWork()
    {
        
    }

    public void FixedUpdateWork()
    {
        time += Time.deltaTime;
        float scale = Mathf.Lerp(1, 0, time);
        transform.localScale *= scale;

    }

    public void LateUpdateWork() { }
}
