using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class WolfBehavior : BehaviorTreeBase
{

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

        DecoratorNode inRange = new DecoratorNode(() => InRange(2.5f));
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


   


}
