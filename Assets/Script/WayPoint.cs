using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class WayPoint : MonoBehaviour, IUpdatable , IPoolingable
{

    private float time = 0;
    private Vector3 originScale;
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, false, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, false, true, false);
    }


    public PoolingType GetPoolingType() { return PoolingType.WAYPOINT; }

    public void Activate(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void DeActivate() 
    {
        gameObject.SetActive(false);
        transform.localScale = originScale;
        time = 0;
    }

    private void Start()
    {
        originScale = transform.localScale;
    }

    public void UpdateWork()
    {
        
    }

    public void FixedUpdateWork()
    {
        time += Time.deltaTime;
        float scale = Mathf.Lerp(1, 0, time);
        transform.localScale *= scale;
        if (transform.localScale == Vector3.zero)
        {
            DeActivate();
        }
    }

    public void LateUpdateWork() { }
}
