using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour , IUpdatable
{
    private Animator animator;
    private Weapon weapon;
    private Weapon shield;
    private TrailRenderer trailRenderer;
    private Move move;
    private Monster targetMonster;
    private PlayerCharacter character;
    private InputAction.CallbackContext inputContition;
    private bool isGuard;
    private int acttackCount = Animator.StringToHash("AttackCount");
    private bool guardTrigger;
    private float rightButtunHoldTime = 0.0f;
    private float rightButtunholdThreshold = 0.1f;

    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        move = GetComponent<Move>();
        character = GetComponent<PlayerCharacter>();
    }
    public void UpdateWork()
    {
        if (character.IsDead)
            return;

        Attack();
        GuardAndParring();
        if (targetMonster != null && targetMonster.HP <= 0)
            targetMonster = null;
    }
    public void FixedUpdateWork()
    {

    }

    public void LateUpdateWork()
    {

    }

    /////////////////////////////// Private Method ///////////////////////////////////
    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && weapon != null && !IsGuard && !Cursor.visible)
        {

            move.StopMovement();
            trailRenderer.enabled = true;
            if (targetMonster != null && targetMonster.IsStunned)
            {
                character.RotateToTarget(targetMonster.transform, true);
                animator.SetTrigger("StrongAttack");
            }
            else
            {
                animator.SetTrigger("Attack");
            }

            AttackCount = 0;
        }
    }

    //마우스 입력이 가드인지 패링인지 확인
    private void GuardAndParring()
    {
        if (inputContition.performed)
        {
            rightButtunHoldTime += Time.deltaTime;
            Debug.Log(rightButtunHoldTime);
        }
        if (guardTrigger)
        {
            //마우스 입력이 단발성인 경우 패링모션
            if (!IsGuard)
                animator.SetTrigger("Parring");
            else
                ChangeGuardState();
            guardTrigger = false;
            rightButtunHoldTime = 0.0f;
        }
        //홀드상태면 가드
        if (!IsGuard && rightButtunholdThreshold <= rightButtunHoldTime)
        {
            ChangeGuardState();
        }
    }

    private void ChangeGuardState()
    {
        IsGuard = !IsGuard;
        animator.SetBool("Guard", IsGuard);
    }

    /////////////////////////////// Animator Event /////////////////////////////////
    public void ColliderEnable()
    {
        if (weapon != null)
        {
            SoundManager.Instance.PlaySFXSound("Sound/SWORD_09");
            Weapon.CapsuleCollider.enabled = true;
        }
           
    }
    public void ColliderDisable()
    {
        if(weapon != null)
         Weapon.CapsuleCollider.enabled = false;
    }
    public void ShieldColliderEnable()
    {
        if (Shield != null)
            Shield.CapsuleCollider.enabled = true;
    }
    public void ShieldColliderDisable()
    {
        if(Shield!=null)
             Shield.CapsuleCollider.enabled = false;
    }
    public void AttackEnd()
    {
        move.AllowMovement();
        if (trailRenderer != null)
            trailRenderer.enabled = false;
    }

    /////////////////////////////// Input System Event //////////////////////////
    public void OnGuard(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            inputContition = value;
        }

        if (value.canceled)
        {
            inputContition = value;
            guardTrigger = true;
        }

    }

    /////////////////////////////// Property /////////////////////////////////
    public int AttackCount
    {
        get => animator.GetInteger(acttackCount);
        set => animator.SetInteger(acttackCount, value);
    }
    public Weapon Weapon
    {
        get => weapon;
        set 
            {   
                weapon = value; 
                trailRenderer = Weapon.GetComponentInChildren<TrailRenderer>();
                trailRenderer.enabled = false;
            } 
    }

    public Weapon Shield{ get => shield;  set => shield = value;  }
    public Monster TargetMonster { get => targetMonster; set => targetMonster = value; }
    public bool IsGuard { get => isGuard; set => isGuard = value; }

}
