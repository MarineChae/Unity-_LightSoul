using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantBehavior : BehaviorTreeBase
{

    private void Awake()
    {
        Debug.Log("mutant");
        rootNode = new SelectNode();

        //DecoratorNode skillCoolDown = new DecoratorNode(() => CoolDown(ATTACK_TYPE.Skill1));
        //rootNode.childList.Add(skillCoolDown);
        //DecoratorNode inSkillRange = new DecoratorNode(() => InRange(4.5f, ATTACK_TYPE.Skill1));
        //skillCoolDown.child = inSkillRange;
        //SequenceNode SkillSequence = new SequenceNode();
        //inSkillRange.child = SkillSequence;
        //TaskNode skill = new TaskNode(() => AttackPlayer());
        //SkillSequence.childList.Add(skill);

        //DecoratorNode inRange = new DecoratorNode(() => InRange(3.5f, ATTACK_TYPE.Melee));
        //rootNode.childList.Add(inRange);
        //SequenceNode attackSequence = new SequenceNode();
        //inRange.child = attackSequence;
        //TaskNode attack = new TaskNode(() => AttackPlayer());
        //attackSequence.childList.Add(attack);
        //TaskNode attackWait = new TaskNode(() => Wait(2.0f, WaitContext.AfterAttack));
        //attackSequence.childList.Add(attackWait);

        //SequenceNode chaseSequence = new SequenceNode();
        //rootNode.childList.Add(chaseSequence);
        //TaskNode chase = new TaskNode(ChasePlayer);
        //chaseSequence.childList.Add(chase);

        //SequenceNode patrolSequence = new SequenceNode();
        //rootNode.childList.Add(patrolSequence);
        //TaskNode findPatrolPos = new TaskNode(SetPatrolPosition);
        //patrolSequence.childList.Add(findPatrolPos);
        //TaskNode patrol = new TaskNode(Patrol);
        //patrolSequence.childList.Add(patrol);
        //TaskNode patrolWait = new TaskNode(() => Wait(5.0f, WaitContext.Patrol));
        //patrolSequence.childList.Add(patrolWait);


    }

}
