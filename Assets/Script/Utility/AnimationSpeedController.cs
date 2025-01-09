
using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    private Animator animator;
    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void Awake()
    {
         TryGetComponent<Animator>(out animator);
    }
    /////////////////////////////// Animator Event /////////////////////////////////
    public void AttackReady(float speed)
    {
        animator.SetFloat("AttackSpeed",speed);
    }
    public void AttackBegin()
    {
        animator.SetFloat("AttackSpeed", 1.0f);
    }


}
