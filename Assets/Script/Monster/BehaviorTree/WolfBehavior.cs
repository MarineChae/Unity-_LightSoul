using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class WolfBehavior : BehaviorTreeBase
{


    private Animator animator;
    private void Awake() 
    {
        Debug.Log("Wolf");
        rootNode = new SelectNode();

        
        SequenceNode attackSequence = new SequenceNode();
        rootNode.childList.Add(attackSequence);
        DecoratorNode inRange = new DecoratorNode(InRange);
        attackSequence.childList.Add(inRange);
        TaskNode attack = new TaskNode(AttackPlayer);
        attackSequence.childList.Add(attack);
        TaskNode attackWait = new TaskNode(() => Wait(1.0f,WaitContext.AfterAttack));
        attackSequence.childList.Add(attackWait);

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
        TaskNode patrolWait = new TaskNode(() => Wait(5.0f, WaitContext.Patrol));
        patrolSequence.childList.Add(patrolWait);



    }


    private ReturnCode InRange()
    {
        if (rangeChecker.Target == null) return 
                ReturnCode.FAILURE;

        float dist = Vector3.SqrMagnitude(monster.transform.position - rangeChecker.Target.transform.position);
        if (dist <= 5.0f)
        {
            agent.ResetPath();
            Debug.Log("Attack");
            monster.Attack();
            return ReturnCode.SUCCESS;
        }
        return ReturnCode.FAILURE;
    }

    private ReturnCode AttackPlayer()
    {
        if(monster.IsAttack)
        {
            return ReturnCode.RUNNING;
        }
        else
        {
            return ReturnCode.SUCCESS;
        }


    }

    ReturnCode ChasePlayer()
    {
        if (rangeChecker.Target != null && monster.Hp > 0)
        {
            lastSeenPosition = rangeChecker.Target.position;
            agent.SetDestination(rangeChecker.Target.position);
            return ReturnCode.RUNNING;
        }
        float dist  = Vector3.SqrMagnitude(lastSeenPosition-transform.position);
        if (dist>=1.0f)
        {
            agent.SetDestination(lastSeenPosition);
            return ReturnCode.RUNNING;
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
