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
    [SerializeField]
    private Material dissolveMaterial;
    [SerializeField]
    private bool isDummy = false;
    private MonsterAttack monsterAttack;
    private BehaviorTreeBase behaviorTreeBase;
    private int hp;
    private float walkSpeed;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isAttack;
    private ATTACK_TYPE currentAttackType;
    private bool isParried;
    private bool isStunned;
    private bool canRotate = true;

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
        InitBehaviorTree();

    }

    //behaviorTree 초기화
    private void InitBehaviorTree()
    {
        var obj = new GameObject("BehaviorTree");
        obj.transform.position = this.transform.position;
        obj.transform.parent = this.transform;
        DataManager.Instance.dicBehaviorFuncs[monsterData.behaviorTreeName](obj);
        behaviorTreeBase = obj.GetComponent<BehaviorTreeBase>();
        behaviorTreeBase.Monster = this;
    }

    public void FixedUpdateWork()
    {

    }
    public void UpdateWork()
    {

        if(behaviorTreeBase.GetRunState())
        {
            behaviorTreeBase.RunTree();
            if (Hp <= 0)
            {
                GetComponent<CapsuleCollider>().enabled = false;
                navMeshAgent.enabled = false;
                behaviorTreeBase.ChangeTreeState();
                Animator.SetBool("Die", true);
                StartCoroutine("Die");
                EventManager.Instance.TriggerAction("KILL", monsterData.name);
            }
            else
            {
                if (navMeshAgent.velocity != Vector3.zero)
                {
                    Animator.SetBool("Walk", true);
                }
                else
                {
                    Animator.SetBool("Walk", false);
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
        if (IsAttack)
            return;
        if(ATTACK_TYPE.Melee == type)
             Animator.SetTrigger("Attack");
        else if(ATTACK_TYPE.Skill1 == type)
            Animator.SetTrigger("Skill1");
        currentAttackType = type;
        canRotate = false;
    }
    public void AttackStart()
    {
        if(ATTACK_TYPE.Melee == currentAttackType)
            monsterAttack.AllowAttack(monsterData.meleeDamage);
        else if (ATTACK_TYPE.Skill1 == currentAttackType)
            monsterAttack.AllowSkillAttack(monsterAttack.transform.position,monsterRangeChecker.Target.transform.position);
        IsAttack = true;
        navMeshAgent.speed = 0;
    }
    public void AttackEnd()
    {
        monsterAttack.StopAttack();
        IsAttack = false;
        navMeshAgent.speed = monsterData.moveSpeed;
    }
    public void StartStun()
    {
        IsStunned = true;
    }
    public void EndStun()
    {
        IsStunned = false;
    }
    public void AllowRotate()
    {
        canRotate = true;
    }
    internal void RotateToTarget(Transform target, bool immediately)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        // 부드럽게 회전
        if (immediately)
            transform.rotation = targetRotation;
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1.0f);
        var mat = GetComponentInChildren<SkinnedMeshRenderer>().material = dissolveMaterial;
        float time = -1.0f;
        while(true)
        {
            time += Time.deltaTime;
            mat.SetFloat("_time", time);
            yield return new WaitForSeconds(Time.deltaTime);
            if (time >= 0.4f) break; ;
        }
        Destroy(gameObject);
        yield return null;
    }

    public int Hp { get => hp; set => hp = value; }
    public float PatrolRange { get => patrolRange; set => patrolRange = value; }
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public bool IsParried { get => isParried; set => isParried = value; }
    public Animator Animator { get => animator;}
    public bool IsStunned { get => isStunned; set => isStunned = value; }
    public bool CanRotate { get => canRotate;}
}
