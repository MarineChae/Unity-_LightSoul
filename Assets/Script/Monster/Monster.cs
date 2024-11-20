using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Monster : MonoBehaviour , IUpdatable
{

    public MonsterData monsterData;
    public MonsterRangeChecker monsterRangeChecker;
    private int hp;

    [SerializeField]
    private float walkSpeed;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, true, false);
    }



    void Start()
    {
        monsterRangeChecker = GetComponentInChildren<MonsterRangeChecker>();    
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        var meshFilter = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();
        meshFilter.sharedMesh = monsterData.mesh;
        Hp = monsterData.hp;

    }

    public void FixedUpdateWork()
    {
        if (monsterRangeChecker.Target != null && Hp > 0)
            navMeshAgent.SetDestination(monsterRangeChecker.Target.position);
    }
    public void UpdateWork()
    {
        if (Hp <= 0)
        {
            animator.SetBool("Die", true);
        }
        else
        {
            if (navMeshAgent.velocity != Vector3.zero)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
            }
        }
    }
    public void LateUpdateWork()
    {
        
    }



    public int Hp { get => hp; set => hp = value; }

}
