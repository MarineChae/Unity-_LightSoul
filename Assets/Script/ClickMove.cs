using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class ClickMove : MonoBehaviour ,IUpdatable
{

    private NavMeshAgent navMeshAgent;

    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                navMeshAgent.SetDestination(hit.point);
                GameObject prefab = Resources.Load("Prefeb/WayPoint") as GameObject;
                GameObject bullet = Instantiate(prefab);
                bullet.name = "bullet";
                bullet.transform.position = hit.point;


            }

        }

    }
    public void LateUpdateWork() { }

}
