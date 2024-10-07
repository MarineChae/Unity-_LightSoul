using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knight : MonoBehaviour ,IUpdatable
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 moveVector;

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
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }
    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
       moveVector = navMeshAgent.velocity;
       
        if(moveVector != Vector3.zero)
        {
            animator.SetBool("Walk", true);

        }
        else
        {
            animator.SetBool("Walk", false);
        }

    }
    public void LateUpdateWork() { }

}
