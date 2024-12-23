using UnityEngine;

public class ResetHit : StateMachineBehaviour
{
    [SerializeField] string triggerName;
    [SerializeField] string attackTriggerName;
    [SerializeField] string parringTriggerName;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(triggerName);
        animator.ResetTrigger(attackTriggerName);
        animator.ResetTrigger(parringTriggerName);
    }


}
