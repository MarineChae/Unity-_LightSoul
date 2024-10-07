using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ClickMove : MonoBehaviour
{

    
    NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        navMeshAgent.velocity = navMeshAgent.desiredVelocity;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray  = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if(Physics.Raycast(ray,out RaycastHit hit))
            {
                navMeshAgent.SetDestination(hit.point);
              
            }

        }

    }
}
