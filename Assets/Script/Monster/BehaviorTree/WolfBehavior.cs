using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class WolfBehavior : BehaviorTreeBase
{

    private NavMeshAgent agent;
    private MonsterRangeChecker rangeChecker;
    float time = 0;
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
        TaskNode patrol = new TaskNode(test2);
        patrolSequence.childList.Add(patrol);



      
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
    ReturnCode test2()
    {
        Debug.Log("NotFound");
        return ReturnCode.SUCCESS;
    }
}
