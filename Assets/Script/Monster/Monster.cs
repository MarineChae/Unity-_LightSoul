using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ATTACK_TYPE
{
    Melee,
    Skill1,
    Skill2,
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
    [SerializeField]
    private SerializedDictianary<ATTACK_TYPE,int> skillDic;

    private Dictionary<ATTACK_TYPE,MonsterSkillData> monsterSkillDatas = new Dictionary<ATTACK_TYPE,MonsterSkillData>();
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
    private NavMeshPath path;
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, true, false);
    }

    private void Awake()
    {
        path = new NavMeshPath();
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

        for(int i = 0; i < skillDic.keys.Count; i++) 
        {
            var original = DataManager.Instance.dicMonsterSkillDatas[skillDic.values[i]];
            string json = JsonConvert.SerializeObject(original);

            MonsterSkillData copy = JsonConvert.DeserializeObject<MonsterSkillData>(json);
            MonsterSkillDatas.Add(skillDic.keys[i], copy);
        }

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
                //navMeshAgent.enabled = false;
                behaviorTreeBase.ChangeTreeState();
                navMeshAgent.ResetPath();
                Animator.SetTrigger("Die");
                StartCoroutine("Die");
                EventManager.Instance.TriggerAction("KILL", monsterData.name);
            }
            else
            {
                foreach(var skill in monsterSkillDatas)
                {
                    skill.Value.remainCoolDown += Time.deltaTime;
                }

                if (navMeshAgent.velocity != Vector3.zero)
                {
                    Animator.SetBool("Walk", true);
                }
                else
                {
                    Animator.SetBool("Walk", false);
                }
            }
            if (monsterRangeChecker.Target != null)
            {
                RotateToTarget(monsterRangeChecker.Target.transform,false);
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
                //Sampling한 위치의 navmesh가 끊겼는지 갈 수 없는 곳인지 판단하기 위해 사용
                //체크하지 않으면 네비메쉬가 끊겨있는 곳으로 계속해서 이동하려함 
                if(NavMesh.CalculatePath(transform.position, hit.position,NavMesh.AllAreas, path))
                {
                    if(path.status == NavMeshPathStatus.PathComplete)
                    {
                        result = hit.position;
                        return true;
                    }

                }
            }
        }
        
        result = transform.position;
        return true;
    }

    public void Attack(ATTACK_TYPE type)
    {

        if (IsAttack)
            return;
        Animator.SetTrigger(type.ToString());
        currentAttackType = type;
        canRotate = false;
        IsAttack = true;
        navMeshAgent.speed = 0;
        navMeshAgent.ResetPath();
    }
    public void AttackStart()
    {
        if(ATTACK_TYPE.Melee == currentAttackType)
            monsterAttack.AllowAttack(monsterData.meleeDamage);
        else
            monsterAttack.AllowSkillAttack(monsterAttack.transform.position,
                                           monsterRangeChecker.Target.transform.position,
                                           monsterData.skillDamage,
                                           MonsterSkillDatas[currentAttackType].skillType);


    }
    public void ResetHit()
    {
        animator.ResetTrigger("Hit");
    }
    public void AttackEnd()
    {
        IsAttack = false;

    }
    public void MoveStop()
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.speed = 0;
        }
    }
    public void MoveStart()
    {
        navMeshAgent.speed = monsterData.moveSpeed;
    }
    public void StartStun()
    {
        IsStunned = true;
    }
    public void EndStun()
    {
        IsStunned = false;
        MoveStart();
    }
    public void AllowRotate()
    {
        canRotate = true;
    }
    public void StopAttack()
    {
        monsterAttack.StopAttack();
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
    public Dictionary<ATTACK_TYPE, MonsterSkillData> MonsterSkillDatas { get => monsterSkillDatas; }
}
