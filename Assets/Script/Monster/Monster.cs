using Newtonsoft.Json;
using System;
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

public class Monster : Entity, IUpdatable
{

    [SerializeField]
    private float patrolRange;
    [SerializeField]
    private Material dissolveMaterial;
    [SerializeField]
    private SerializedDictianary<ATTACK_TYPE, int> skillDic;
    [SerializeField]
    private AudioClip attackSound;
    [SerializeField]
    private AudioClip[] skillSound;
    [SerializeField]
    private bool isBoss;
    [SerializeField]
    private MonsterData monsterData;
    private MonsterRangeChecker monsterRangeChecker;
    private Dictionary<ATTACK_TYPE, MonsterSkillData> monsterSkillDatas = new Dictionary<ATTACK_TYPE, MonsterSkillData>();
    private MonsterAttack monsterAttack;
    private BehaviorTreeBase behaviorTreeBase;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private NavMeshPath path;
    private MonsterHpUI monsterHpUI;
    private Collider monsterCollider;
    private Vector3 lockOnPosition;
    private Vector3 lastPosition;
    private bool isAttack;
    private bool isStunned;
    private bool canRotate = true;
    private bool isDead = false;
    private ATTACK_TYPE currentAttackType;
    private float checkTimer;
    private float stuckTimer;
    private readonly float checkInterval = 2.0f;
    private readonly float stuckThreshold = 0.5f;
    private readonly float maxStuckTime = 5f; 


    /////////////////////////////// Life Cycle ///////////////////////////////////

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
        InitBehaviorTree();
        MonsterRangeChecker = GetComponentInChildren<MonsterRangeChecker>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = MonsterData.moveSpeed;
        animator = GetComponent<Animator>();
        monsterAttack = GetComponentInChildren<MonsterAttack>();
        monsterHpUI = GetComponentInChildren<MonsterHpUI>();
        monsterCollider = GetComponent<Collider>();

    }


    public void FixedUpdateWork()
    {

    }
    public void UpdateWork()
    {

        if (behaviorTreeBase.GetRunState())
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= checkInterval)
            {
                checkTimer = 0f;
                CheckIfStuck();
            }

            behaviorTreeBase.RunTree();
            if (HP <= 0)
            {
                MonsterDie();
            }
            else
            {
                foreach (var skill in monsterSkillDatas)
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
            if (MonsterRangeChecker.Target != null)
            {
                RotateToTarget(MonsterRangeChecker.Target.transform, false);
            }
            navMeshAgent.velocity = navMeshAgent.desiredVelocity;
            lockOnPosition = monsterCollider.bounds.center;
        }
    }

    public void LateUpdateWork()
    {

    }

    /////////////////////////////// Override Method///////////////////////////////////
    public override void TakeDamage(float damage)
    {
        if (damage > MaxHP * 0.2f)
            Animator.SetTrigger("HardHit");
        else
            Animator.SetTrigger("Hit");

        //피격사운드
        SoundManager.Instance.PlaySFXSound("Sound/BowWater1");

        HP -= damage;
    }

    public override void UseStamina(float stamina)
    {
        throw new NotImplementedException();
    }
    /////////////////////////////// Private Method///////////////////////////////////
    private void InitMonsterData()
    {
        var jsonData = DataManager.Instance.dicMonsterDatas[MonsterData.id];
        MonsterData = jsonData;
        HP = MonsterData.hp;
        for (int i = 0; i < skillDic.keys.Count; i++)
        {
            var original = DataManager.Instance.dicMonsterSkillDatas[skillDic.values[i]];
            string json = JsonConvert.SerializeObject(original);

            MonsterSkillData copy = JsonConvert.DeserializeObject<MonsterSkillData>(json);
            MonsterSkillDatas.Add(skillDic.keys[i], copy);
        }

    }

    //behaviorTree 초기화
    private void InitBehaviorTree()
    {
        var obj = new GameObject("BehaviorTree");
        obj.transform.position = this.transform.position;
        obj.transform.parent = this.transform;
        DataManager.Instance.dicBehaviorFuncs[MonsterData.behaviorTreeName](obj);
        behaviorTreeBase = obj.GetComponent<BehaviorTreeBase>();
        behaviorTreeBase.Monster = this;
    }
    //몬스터가 좁은곳을 향해 갈때 정체되는 현상을 없애기 위해
    private void CheckIfStuck()
    {
        float distanceMoved = Vector3.Distance(navMeshAgent.transform.position, lastPosition);
        // 에이전트가 거의 움직이지 않았다면, 정체로 간주
        if (distanceMoved < stuckThreshold)
        {
            stuckTimer += checkInterval;
            if (stuckTimer >= maxStuckTime)
            {
                RecalculatePath();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f; // 움직임이 감지되면 타이머 초기화
        }

        lastPosition = navMeshAgent.transform.position; // 현재 위치 저장
    }

    private void RecalculatePath()
    {
        RandomPoint(out Vector3 target);
        navMeshAgent.SetDestination(target);
    }

    private void MonsterDie()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        //navMeshAgent.enabled = false;
        behaviorTreeBase.ChangeTreeState();
        navMeshAgent.ResetPath();
        Animator.SetTrigger("Die");
        StartCoroutine("Die");
        EventManager.Instance.TriggerAction("KILL", MonsterData.name);
        isDead = true;
        if (isBoss)
            StartCoroutine("Ending");
        monsterHpUI.gameObject.SetActive(false);
    }

    /////////////////////////////// Public Method///////////////////////////////////
    public bool RandomPoint(out Vector3 result)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * patrolRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, patrolRange, NavMesh.AllAreas))
            {
                //Sampling한 위치의 navmesh가 끊겼는지 갈 수 없는 곳인지 판단하기 위해 사용
                //체크하지 않으면 네비메쉬가 끊겨있는 곳으로 계속해서 이동하려함 
                if (NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, path))
                {
                    if (path.status == NavMeshPathStatus.PathComplete)
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
        if (ATTACK_TYPE.Melee == currentAttackType)
            monsterAttack.AllowAttack(MonsterData.meleeDamage);
        else
            monsterAttack.AllowSkillAttack(monsterAttack.transform.position,
                                           MonsterRangeChecker.Target.transform.position,
                                           MonsterSkillDatas[currentAttackType].skillDamage,
                                           MonsterSkillDatas[currentAttackType].skillType);


    }
    public void RotateToTarget(Transform target, bool immediately)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        // 부드럽게 회전
        if (immediately)
            transform.rotation = targetRotation;
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
    }

    /////////////////////////////// Animator Event /////////////////////////////////
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
        navMeshAgent.speed = MonsterData.moveSpeed;
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
    public void PlayAttackSound()
    {
        SoundManager.Instance.PlaySFXSound(attackSound);
    }
    public void PlaySkillSound(int num)
    {
        SoundManager.Instance.PlaySFXSound(skillSound[num]);
    }

    /////////////////////////////// Coroutine //////////////////////////
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1.0f);
        var mat = GetComponentInChildren<SkinnedMeshRenderer>().material = dissolveMaterial;
        float time = -1.0f;
        while (true)
        {
            time += Time.deltaTime;
            mat.SetFloat("_time", time);
            yield return new WaitForSeconds(Time.deltaTime);
            if (time >= 0.4f) break; ;
        }
        yield return new WaitForSeconds(6.0f);
        Destroy(gameObject);
        yield return null;
    }
    IEnumerator Ending()
    {
        yield return new WaitForSeconds(2f);
        SoundManager.Instance.PlaySFXSound("Sound/Jingle_Win_00");
        yield return new WaitForSeconds(5.0f);
        LoadingSceneContoller.LoadScene("StartScene");
        yield break;
    }

    /////////////////////////////// Property /////////////////////////////////
    public float PatrolRange { get => patrolRange; set => patrolRange = value; }
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public Animator Animator { get => animator; }
    public bool IsStunned { get => isStunned; set => isStunned = value; }
    public bool CanRotate { get => canRotate; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public Vector3 LockOnPosition { get => lockOnPosition; set => lockOnPosition = value; }
    public Dictionary<ATTACK_TYPE, MonsterSkillData> MonsterSkillDatas { get => monsterSkillDatas; }
    public override float MaxHP => MonsterData.hp;
    public override float MaxStamina => throw new System.NotImplementedException();
    public override float StaminaRecovery => throw new System.NotImplementedException();
    public MonsterData MonsterData { get => monsterData; set => monsterData = value; }
    public MonsterRangeChecker MonsterRangeChecker { get => monsterRangeChecker; set => monsterRangeChecker = value; }
}
