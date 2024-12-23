using UnityEngine;

public class SkeletonBehavior : BehaviorTreeBase
{

    private void Awake()
    {
        Debug.Log("Wolf");
        rootNode = new SelectNode();



        DecoratorNode inSkillRange = new DecoratorNode(() => InRange(4.0f));
        rootNode.childList.Add(inSkillRange);
        DecoratorNode skillCoolDown = new DecoratorNode(() => CoolDown(ATTACK_TYPE.Skill1));
        inSkillRange.child = skillCoolDown;
        SequenceNode SkillSequence = new SequenceNode();
        skillCoolDown.child = SkillSequence;
        TaskNode skill = new TaskNode(() => AttackPlayer(ATTACK_TYPE.Skill1));
        SkillSequence.childList.Add(skill);

        DecoratorNode inSkill2Range = new DecoratorNode(() => InRange(8.0f));
        rootNode.childList.Add(inSkill2Range);
        DecoratorNode skill2CoolDown = new DecoratorNode(() => CoolDown(ATTACK_TYPE.Skill2));
        inSkill2Range.child = skill2CoolDown;
        SequenceNode Skill2Sequence = new SequenceNode();
        skill2CoolDown.child = Skill2Sequence;
        TaskNode skill2 = new TaskNode(() => AttackPlayer(ATTACK_TYPE.Skill2));
        Skill2Sequence.childList.Add(skill2);

        DecoratorNode inRange = new DecoratorNode(() => InRange(monster.monsterData.attackRange));
        rootNode.childList.Add(inRange);
        SequenceNode attackSequence = new SequenceNode();
        inRange.child = attackSequence;
        TaskNode attack = new TaskNode(() => AttackPlayer(ATTACK_TYPE.Melee));
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
