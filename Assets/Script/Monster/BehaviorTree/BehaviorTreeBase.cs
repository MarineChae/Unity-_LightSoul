using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public enum WaitContext
{
    Patrol,   
    AfterAttack 
}
public class BehaviorTreeBase : MonoBehaviour
{
    //트리의 루트 노드는 항상 브런치노드에서 파생 되어야함
    protected BranchNode rootNode;
    protected float cooldown;
    protected Animator animator;
    protected Monster monster;
    protected NavMeshAgent agent;
    protected MonsterRangeChecker rangeChecker;
    protected Vector3 destination;
    protected Vector3 lastSeenPosition;
    private float waitTime = 0;
    private bool isRun = true;
    private void Start()
    {
        rangeChecker = monster.monsterRangeChecker;
        agent = monster.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(isRun)
        {
            cooldown += Time.deltaTime;
        }
    }
    public Monster Monster { get => monster; set => monster = value; }

    public void RunTree()
    {
        if (isRun && rangeChecker != null)
            rootNode.Tick();
    }

    public void ChangeTreeState()
    {
        rootNode.currentChild = 0;
        isRun = !isRun;
    }
    public bool GetRunState()
    {
        return isRun;
    }

    virtual public ReturnCode Wait(float waitingTime, WaitContext context)
    {
        if (context == WaitContext.Patrol && rangeChecker.Target != null)
        {
            waitTime = 0;
            return ReturnCode.FAILURE;
        }

        if (waitTime >= waitingTime)
        {
            waitTime = 0;
            return ReturnCode.SUCCESS;
        }
        else
        {
            waitTime += Time.deltaTime;
            return ReturnCode.RUNNING;
        }
    }
    protected ReturnCode CoolDown(ATTACK_TYPE skillType)
    {
        var skillData = monster.MonsterSkillDatas[skillType];
        if (skillData.remainCoolDown >= skillData.coolDown)
        {
            skillData.remainCoolDown = 0;
            return ReturnCode.SUCCESS;
        }
        else
        {
            return ReturnCode.FAILURE;
        }
    }
    protected ReturnCode InRange(float range)
    {
        if (rangeChecker == null || rangeChecker.Target == null) return
                ReturnCode.FAILURE;

        float dist = Vector3.Magnitude(monster.transform.position - rangeChecker.Target.transform.position);
        if (dist <= range)
        {

            return ReturnCode.SUCCESS;
        }

        return ReturnCode.FAILURE;
    }

    protected ReturnCode AttackPlayer(ATTACK_TYPE type)
    {
        monster.Attack(type);
        return ReturnCode.SUCCESS;
    }

    protected ReturnCode ChasePlayer()
    {
   
        if (rangeChecker.Target != null && monster.HP > 0)
        {
            agent.SetDestination(rangeChecker.Target.position);
            return ReturnCode.SUCCESS;
        }
        return ReturnCode.FAILURE;
    }

    protected ReturnCode SetPatrolPosition()
    {
        monster.RandomPoint(out destination);
        agent.SetDestination(destination);
        return ReturnCode.SUCCESS;
    }
    protected ReturnCode Patrol()
    {
        float dist = Vector3.SqrMagnitude(destination - monster.transform.position);
        if (dist < 1 || rangeChecker.Target != null)
        {
            return ReturnCode.SUCCESS;
        }
        else
        {

            return ReturnCode.RUNNING;
        }
    }


}
