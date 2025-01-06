using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour , IUpdatable
{
    private Animator animator;
    private int acttackCount = Animator.StringToHash("AttackCount");
    private Weapon weapon;
    private Weapon shield;
    private bool isGuard;
    private TrailRenderer trailRenderer;
    private Move move;
    private bool guardTrigger;
    private float rightButtunHoldTime = 0.0f;
    private float rightButtunholdThreshold = 0.1f;
    InputAction.CallbackContext testvalue;
    private Monster targetMonster;
    private PlayerCharacter character;
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
        if (targetMonster != null && targetMonster.Hp <= 0)
            targetMonster = null;


    }

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

    private void GuardAndParring()
    {
        if (testvalue.performed)
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

    public void FixedUpdateWork()
    {

    }

    public void LateUpdateWork()
    {

    }
    public void ColliderEnable()
    {
        if (weapon != null)
        {
            SoundManager.Instance.PlaySFXSound("Sound/SWORD_09");
            Weapon.capsuleCollider.enabled = true;
        }
           
    }
    public void ColliderDisable()
    {
        if(weapon != null)
         Weapon.capsuleCollider.enabled = false;
    }
    public void ShieldColliderEnable()
    {
        if (Shield != null)
            Shield.capsuleCollider.enabled = true;
    }
    public void ShieldColliderDisable()
    {
        if(Shield!=null)
             Shield.capsuleCollider.enabled = false;
    }
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

    public Weapon Shield
    {  
       get => shield; 
       set => shield = value; 
    }
    public Monster TargetMonster { get => targetMonster; set => targetMonster = value; }
    public bool IsGuard { get => isGuard; set => isGuard = value; }

    public void AttackEnd()
    {
        move.AllowMovement();
        if(trailRenderer != null)
        trailRenderer.enabled = false;
    }
    public void OnGuard(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            testvalue = value;
        }

        if (value.canceled)
        {
            testvalue = value;
            guardTrigger = true;
        }
           
    }
    
}
