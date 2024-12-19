using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehavior : BehaviorTreeBase
{

    private void Awake()
    {
        Debug.Log("Wolf");
        rootNode = new SelectNode();



        DecoratorNode skillCoolDown = new DecoratorNode(() => CoolDown(ATTACK_TYPE.Skill1));
        rootNode.childList.Add(skillCoolDown);
        DecoratorNode inSkillRange = new DecoratorNode(() => InRange(2.5f, ATTACK_TYPE.Skill1));
        skillCoolDown.child = inSkillRange;
        SequenceNode SkillSequence = new SequenceNode();
        inSkillRange.child = SkillSequence;
        TaskNode skill = new TaskNode(() => AttackPlayer());
        SkillSequence.childList.Add(skill);

        DecoratorNode skill2CoolDown = new DecoratorNode(() => CoolDown(ATTACK_TYPE.Skill2));
        rootNode.childList.Add(skill2CoolDown);
        DecoratorNode inSkill2Range = new DecoratorNode(() => InRange(8.0f, ATTACK_TYPE.Skill2));
        skill2CoolDown.child = inSkill2Range;
        SequenceNode Skill2Sequence = new SequenceNode();
        inSkill2Range.child = Skill2Sequence;
        TaskNode skill2 = new TaskNode(() => AttackPlayer());
        Skill2Sequence.childList.Add(skill2);


        DecoratorNode inRange = new DecoratorNode(() => InRange(2.0f, ATTACK_TYPE.Melee));
        rootNode.childList.Add(inRange);
        SequenceNode attackSequence = new SequenceNode();
        inRange.child = attackSequence;
        TaskNode attack = new TaskNode(() => AttackPlayer());
        attackSequence.childList.Add(attack);
        TaskNode attackWait = new TaskNode(() => Wait(2.0f, WaitContext.AfterAttack));
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
