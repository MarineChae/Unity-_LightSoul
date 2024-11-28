using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class WolfBehavior : BehaviorTreeBase
{

    private NavMeshAgent agent;
    private MonsterRangeChecker rangeChecker;
    private Vector3 destination;
    private float waitTime = 0;
    private void Start() 
    {
        Debug.Log("Wolf");
        rootNode = new SelectNode();

        SequenceNode chaseSequence = new SequenceNode();
        rootNode.childList.Add(chaseSequence);
        TaskNode chase = new TaskNode(ChasePlayer);
        chaseSequence.childList.Add(chase);

        SequenceNode patrolSequence = new SequenceNode();
        rootNode.childList.Add(patrolSequence);
        TaskNode findPatrolPos = new TaskNode(SetPatrolPosition);
        patrolSequence.childList.Add(findPatrolPos);
        TaskNode patrol = new TaskNode(Patrol);
        patrolSequence.childList.Add(patrol);
        TaskNode wait = new TaskNode(Wait);
        patrolSequence.childList.Add(wait);


        agent = monster.GetComponent<NavMeshAgent>();
        rangeChecker = monster.monsterRangeChecker;
    }
    ReturnCode ChasePlayer()
    {
        if (rangeChecker.Target != null && monster.Hp > 0)
        {
            agent.SetDestination(rangeChecker.Target.position);
            return ReturnCode.SUCCESS;
        }
        return ReturnCode.FAILURE;
    }
    ReturnCode SetPatrolPosition()
    {
        monster.RandomPoint(out destination);
        agent.SetDestination(destination);
        return ReturnCode.SUCCESS;
    }
    ReturnCode Patrol()
    {
        float dist = Vector3.Distance(destination, monster.transform.position);
        if (dist < 1 || rangeChecker.Target != null)
        {
            return ReturnCode.SUCCESS;
        }
        else
        {

            return ReturnCode.RUNNING;
        }
    }
     override public ReturnCode Wait() 
    {
        waitTime += Time.deltaTime;
        if (waitTime >= 5)
        {
            Debug.Log("wait");
            waitTime = 0;
            return ReturnCode.SUCCESS;
        }
        else if (rangeChecker.Target != null)
        {
            return ReturnCode.FAILURE;
        }
        else
        {
            return ReturnCode.RUNNING;
        }
    }
}
