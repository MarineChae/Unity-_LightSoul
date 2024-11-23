using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Monster : MonoBehaviour , IUpdatable
{

    public MonsterData monsterData;
    public MonsterRangeChecker monsterRangeChecker;

    private BehaviorTreeBase behaviorTreeBase;
    private int hp;
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
        InitMonsterData();
        monsterRangeChecker = GetComponentInChildren<MonsterRangeChecker>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        animator = GetComponent<Animator>();

        

    }

    private void InitMonsterData()
    {
        var jsonData = DataManager.Instance.dicMonsterDatas[monsterData.id];
        monsterData = jsonData;
        Hp = monsterData.hp;
        walkSpeed = monsterData.moveSpeed;

        var obj = new GameObject("BehaviorTree");
        obj.transform.parent = this.transform;
        DataManager.Instance.dicBehaviorFuncs[monsterData.behaviorTreeName](obj);
        behaviorTreeBase = obj.GetComponent<BehaviorTreeBase>();
        behaviorTreeBase.Monster = this;

    }

    public void FixedUpdateWork()
    {
        //if (monsterRangeChecker.Target != null && Hp > 0)
        //    navMeshAgent.SetDestination(monsterRangeChecker.Target.position);
    }
    public void UpdateWork()
    {
        behaviorTreeBase.RunTree();
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
