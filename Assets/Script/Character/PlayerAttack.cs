using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour , IUpdatable
{
    private Animator animator;
    private int acttackCount = Animator.StringToHash("AttackCount");
    private NavMeshAgent navMeshAgent;
    private Weapon weapon;
    private bool isGuard;
    private TrailRenderer trailRenderer;
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }
    public void UpdateWork()
    {
        if (Input.GetMouseButtonDown(1) && weapon != null && !isGuard)
        {
            trailRenderer.enabled = true;
            animator.SetTrigger("Attack");
            AttackCount = 0;
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isGuard = !isGuard;
            animator.SetBool("Guard", isGuard);

        }
    }

    public void FixedUpdateWork()
    {

    }

    public void LateUpdateWork()
    {

    }
    public void ColliderEnable()
    {
        Debug.Log("colliderEnable");
        Weapon.capsuleCollider.enabled = true;
    }
    public void ColliderDisable()
    {
        Debug.Log("ColliderDisable");
        Weapon.capsuleCollider.enabled = false;
    }

    public int AttackCount
    {
        get => animator.GetInteger(acttackCount);
        set => animator.SetInteger(acttackCount, value);
    }
    public Weapon Weapon
    {
        get => weapon;
        set 
            {   
                weapon = value; 
                trailRenderer = Weapon.GetComponentInChildren<TrailRenderer>();
                trailRenderer.enabled = false;
            } 
    }

    public void AttackEnd()
    {
        trailRenderer.enabled = false;
        navMeshAgent.destination = transform.position;
        navMeshAgent.isStopped = false;
        navMeshAgent.velocity = Vector3.zero;
    }
   
}
