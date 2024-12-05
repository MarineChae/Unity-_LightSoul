using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class WolfBehavior : BehaviorTreeBase
{

    private float cooldown;
    private Animator animator;
    private void Awake() 
    {
        Debug.Log("Wolf");
        rootNode = new SelectNode();


        DecoratorNode inSkillRange = new DecoratorNode(() => InRange(10.0f));
        rootNode.childList.Add(inSkillRange);
        DecoratorNode skillCoolDown = new DecoratorNode(() => CoolDown(10.0f));
        inSkillRange.child = skillCoolDown;
        SequenceNode SkillSequence = new SequenceNode();
        skillCoolDown.child = SkillSequence;
        TaskNode skill = new TaskNode(() => AttackPlayer(ATTACK_TYPE.Skill1));
        SkillSequence.childList.Add(skill);

        DecoratorNode inRange = new DecoratorNode(() => InRange(3.0f));
        rootNode.childList.Add(inRange);
        SequenceNode attackSequence = new SequenceNode();
        inRange.child = attackSequence;
        TaskNode attack = new TaskNode(() => AttackPlayer(ATTACK_TYPE.Melee));
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

    private void Update()
    {
        cooldown += Time.deltaTime;
    }
    private ReturnCode CoolDown(float time)
    {
        if(cooldown >= time)
        {
            cooldown = 0;
            return ReturnCode.SUCCESS;
        }
        else
        {
            return ReturnCode.FAILURE;
        }
    }
    private ReturnCode InRange(float range )
    {
        if (rangeChecker.Target == null) return 
                ReturnCode.FAILURE;

        float dist = Vector3.Magnitude(monster.transform.position - rangeChecker.Target.transform.position);
        if (dist <= range)
        {
            agent.ResetPath();

            return ReturnCode.SUCCESS;
        }
        monster.IsAttack = false;
        return ReturnCode.FAILURE;
    }

    private ReturnCode AttackPlayer(ATTACK_TYPE type)
    {
        monster.Attack(type);
        if (monster.IsAttack)
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
            return ReturnCode.SUCCESS;
        }
        float dist  = Vector3.SqrMagnitude(lastSeenPosition-transform.position);
        if (dist>=5.0f)
        {
            agent.SetDestination(lastSeenPosition);
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
