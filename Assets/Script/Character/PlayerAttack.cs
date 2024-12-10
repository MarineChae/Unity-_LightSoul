using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
    }
    public void UpdateWork()
    {
        if (Input.GetMouseButtonDown(0) && weapon != null && !isGuard && !Cursor.visible)
        {
            move.StopMovement();
            trailRenderer.enabled = true;
            if(targetMonster != null)
            {
                move.RotateToTarget(targetMonster.transform.position);
            }
            if (targetMonster != null &&  targetMonster.IsStunned)
            {
                animator.SetTrigger("StrongAttack");
            }
            else
            {
                animator.SetTrigger("Attack");
            }
            AttackCount = 0;
        }
        GuardAndParring();
        if (targetMonster != null && targetMonster.Hp <= 0)
            targetMonster = null;


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
            if (!isGuard)
                animator.SetTrigger("Parring");
            else
                ChangeGuardState();
            guardTrigger = false;
            rightButtunHoldTime = 0.0f;
        }
        //홀드상태면 가드
        if (!isGuard && rightButtunholdThreshold <= rightButtunHoldTime)
        {
            ChangeGuardState();
        }
    }

    private void ChangeGuardState()
    {
        isGuard = !isGuard;
        animator.SetBool("Guard", isGuard);
    }

    public void FixedUpdateWork()
    {

    }

    public void LateUpdateWork()
    {

    }
    public void ColliderEnable()
    {
        Debug.Log("colliderEnable");
        Weapon.capsuleCollider.enabled = true;
    }
    public void ColliderDisable()
    {
        Debug.Log("ColliderDisable");
        Weapon.capsuleCollider.enabled = false;
    }
    public void ShieldColliderEnable()
    {
        Debug.Log("colliderEnable");
        Shield.capsuleCollider.enabled = true;
    }
    public void ShieldColliderDisable()
    {
        Debug.Log("ColliderDisable");
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

    public void AttackEnd()
    {
        move.AllowMovement();
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
