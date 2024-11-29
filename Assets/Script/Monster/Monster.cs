using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public enum ATTACK_TYPE
{
    Melee,
    Skill1,
}

public class Monster : MonoBehaviour , IUpdatable
{

    public MonsterData monsterData;
    public MonsterRangeChecker monsterRangeChecker;
    [SerializeField]
    private float patrolRange;
    private MonsterAttack monsterAttack;
    private BehaviorTreeBase behaviorTreeBase;
    private int hp;
    private float walkSpeed;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isAttack;
    private ATTACK_TYPE currentAttackType;
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
        monsterAttack = GetComponentInChildren<MonsterAttack>();

    }

    private void InitMonsterData()
    {
        var jsonData = DataManager.Instance.dicMonsterDatas[monsterData.id];
        monsterData = jsonData;
        Hp = monsterData.hp;
        walkSpeed = monsterData.moveSpeed;

        var obj = new GameObject("BehaviorTree");
        obj.transform.position = this.transform.position;
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
        if(behaviorTreeBase.GetRunState())
        {
            behaviorTreeBase.RunTree();
            if (Hp <= 0)
            {
                navMeshAgent.enabled = false;
                behaviorTreeBase.ChangeTreeState();
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
            navMeshAgent.velocity = navMeshAgent.desiredVelocity;
        }
         
    }
    public void LateUpdateWork()
    {
        
    }

    public bool RandomPoint(out Vector3 result)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, patrolRange, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = transform.position;
        return true;
    }

    public void Attack(ATTACK_TYPE type)
    {
        if(ATTACK_TYPE.Melee == type)
             animator.SetTrigger("Attack");
        else if(ATTACK_TYPE.Skill1 == type)
            animator.SetTrigger("Skill1");
        currentAttackType = type;
        IsAttack = true;
    }
    public void AttackStart()
    {
        if(ATTACK_TYPE.Melee == currentAttackType)
            monsterAttack.AllowAttack(monsterData.meleeDamage);
        else if (ATTACK_TYPE.Skill1 == currentAttackType)
            monsterAttack.AllowSkillAttack(monsterAttack.transform.position,monsterRangeChecker.Target.transform.position);
    }
    public void AttackEnd()
    {
        monsterAttack.StopAttack();
        IsAttack = false;
    }
    public int Hp { get => hp; set => hp = value; }
    public float PatrolRange { get => patrolRange; set => patrolRange = value; }
    public bool IsAttack { get => isAttack; set => isAttack = value; }
}
