using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public enum WaitContext
{
    Patrol,   
    AfterAttack 
}
public class BehaviorTreeBase : MonoBehaviour
{
    //트리의 루트 노드는 항상 브런치노드에서 파생 되어야함
    protected BranchNode rootNode;
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
    public Monster Monster { get => monster; set => monster = value; }

    public void RunTree()
    {
        if (isRun)
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
        waitTime += Time.deltaTime;

        if (context == WaitContext.Patrol && rangeChecker.Target != null)
        {
            return ReturnCode.FAILURE;
        }

        if (waitTime >= waitingTime)
        {
            waitTime = 0;
            return ReturnCode.SUCCESS;
        }
        else
        {
            return ReturnCode.RUNNING;
        }
    }

}
