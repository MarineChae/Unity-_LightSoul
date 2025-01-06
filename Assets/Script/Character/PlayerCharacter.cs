using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI.Table;


public class PlayerCharacter : Entity, IUpdatable
{
    private Animator animator;
    [HideInInspector]
    public ItemData[] itemDatas;
    [SerializeField]
    private Transform weaponSocket;
    [SerializeField]
    private Transform weapon2Socket;
    [SerializeField]
    private EquipItem equipItem;
    [SerializeField]
    private float baseHp = 100;
    private CapsuleCollider capsuleCollider;
    private EquipItem[] equipItems;
    public Weapon[] equipWeapon;
    private PlayerAttack playerAttack;
    private int layerMask;
    private bool isHit = false;
    private bool isRoll = false;
    private bool isDrink = false;
    private Coroutine runCoroutine;
    private Move playerMove;
    private bool isDead;
    private LockOn lockOn;
    private bool isLockOn=false;
    
    public override float MaxHP => baseHp;

    public override float MaxStamina => 100;

    public override float StaminaRecovery => 1.5f;


    private float staminerConsumption = 30;
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, true, false);
    }


    void Awake()
    {
        layerMask = 1 << LayerMask.NameToLayer("Interactive");//
        InitStatus();
        itemDatas = new ItemData[(int)ITEMTYPE.END];
        equipItems = new EquipItem[(int)ITEMTYPE.END];
        equipWeapon = new Weapon[2];
        animator = GetComponentInChildren<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerMove = GetComponent<Move>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        lockOn = GetComponentInChildren<LockOn>();
    }
    public void FixedUpdateWork()
    {
        
    }
    public void UpdateWork()
    {
        if (IsDead) return;
        if (itemDatas[(int)ITEMTYPE.WEAPON] == null)
        {
            animator.SetBool("EquipWeapon", false);
        }
        else
        {
            animator.SetBool("EquipWeapon", true);
        }
        Roll();
        if(HP <=0)
        {
            StartCoroutine("Die");
        }
        if(IsLockOn)
        {
            if(lockOn.Target.IsDead)
            {
                animator.SetBool("LockOn", false);
                IsLockOn = false;
                lockOn.RemoveTarget(lockOn.Target.transform);
                lockOn.Target = null;
            }
            else
            {
                var dir = lockOn.Target.transform.position - transform.position;
                dir.y = 0;
                RotateToTarget(lockOn.Target.transform, false);
            }
        }
    }

    private void PlayerInteraction()
    {
        if (Physics.Raycast(transform.position + transform.up, transform.forward, out RaycastHit hit, 1.5f, layerMask))
        {
            if(hit.transform.CompareTag("NPC"))
            {
                bool isMove = DialogueManager.Instance.Interact(hit.collider.gameObject);
                playerMove.CanMove = !isMove;
                RotateToTarget(hit.transform,false);
            }
            if(hit.transform.CompareTag("Chest"))
            {
                var chset = hit.transform.GetComponent<Chest>();
                chset.OpenChest();
            }
        }
    }

    public void RotateToTarget(Transform target,bool immediately)
    {
        if (!IsRoll)
        {
            if(immediately)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                directionToTarget.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = targetRotation;
            }
            else
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                directionToTarget.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,10.0f * Time.deltaTime);
            }

        }

    }
    public void ForceRotatePlayer(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }
    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Stamina >= staminerConsumption && !IsRoll &&!isDrink && !isHit && playerMove.IsMove)
        {
            animator.SetTrigger("Roll");
            IsRoll = true;
            UseStamina(staminerConsumption);
        }
    }

    void RollEnd()
    {
        IsRoll=false;
    }


    public void LateUpdateWork() { }

    public void EquipItem(ItemData itemData)
    {
        itemDatas[(int)itemData.slotType] = itemData;
        if(itemData.slotType == ITEMTYPE.WEAPON )
        {
            EquipWeapon(itemData, weaponSocket.transform,true);
            equipWeapon[0] = equipItems[(int)ITEMTYPE.WEAPON].weaponData;
            equipWeapon[0].tag = "Weapon";
            PlayerAttack.Weapon = equipWeapon[0];
        }
        else if(itemData.slotType == ITEMTYPE.WEAPON2)
        {
            EquipWeapon(itemData ,weapon2Socket.transform,false);
            equipWeapon[1] = equipItems[(int)ITEMTYPE.WEAPON2].weaponData;
            equipWeapon[1].tag = "Shield";
            PlayerAttack.Shield = equipWeapon[1];
        }
    }

    private void EquipWeapon(ItemData itemData,Transform socketTransform,bool isWeapon)
    {
        equipItems[(int)itemData.slotType] = Instantiate(equipItem);
        equipItems[(int)itemData.slotType].IsWeapon = isWeapon;
        equipItems[(int)itemData.slotType].Init(itemData);
        equipItems[(int)itemData.slotType].transform.SetParent(socketTransform);
        equipItems[(int)itemData.slotType].transform.localPosition = Vector3.zero;
        equipItems[(int)itemData.slotType].transform.localRotation = Quaternion.identity;
        SoundManager.Instance.PlaySFXSound("Sound/SWORD_05");
    }

    public void UnEquipItem(ItemData itemData)
    {
        itemDatas[(int)itemData.slotType] = null;
        if (itemData.itemType == ITEMTYPE.WEAPON || itemData.itemType == ITEMTYPE.WEAPON2)
        {
            equipWeapon[(int)itemData.slotType - 1] = null;
            var des = equipItems[(int)itemData.slotType];
            equipItems[(int)itemData.slotType] = null;
            Destroy(des.gameObject);

        }
    }

    public override void TakeDamage(float damage)
    {
        playerAttack.ShieldColliderDisable();
        animator.ResetTrigger("Hit");
        animator.SetTrigger("Hit");
        if (playerAttack.IsGuard)
            HP -= damage * 0.2f;
        else
            HP -= damage;
        IsHit = true;
    }
    public void EndHit()
    {
        IsHit = false;
        bool isRun = animator.GetBool("Run");
        if (isRun)
            playerMove.MoveSpeed = 5.0f;
        else 
            playerMove.MoveSpeed = 2.0f;
    }
    public override void UseStamina(float stamina)
    {
        Stamina -= stamina;

    }
    public void OnPotion(InputAction.CallbackContext value)
    {
        if (value.started && !isDrink)
        {
            animator.SetLayerWeight(1, 1);
            StartCoroutine(DrinkPotion());
            EventManager.Instance.PotionTriggerAction("USE");
        }
    }
    public void OnRun(InputAction.CallbackContext value)
    {
        if (IsLockOn)
        {
            if (runCoroutine != null)
            {
                playerMove.MoveSpeed = 2.0f;
                animator.SetBool("Run", false);
                StopCoroutine(runCoroutine);
                runCoroutine = null;
                StartCoroutine("Recovery");
            }
            return;
        } 
        if (value.started)
        {
            playerMove.MoveSpeed = 5.0f;
            animator.SetBool("Run",true);
            runCoroutine = StartCoroutine("Run");
            StopCoroutine("Recovery");
        }
        if(value.canceled)
        {
            playerMove.MoveSpeed = 2.0f;
            animator.SetBool("Run", false);
            StopCoroutine(runCoroutine);
            runCoroutine = null;
            StartCoroutine("Recovery");
        }
    }
    public void OnInteraction(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            PlayerInteraction();
        }
    }
    public void OnLockOn(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (lockOn.Target == null)
                return;

            if(!IsLockOn)
            {
                animator.SetBool("LockOn", true);
                IsLockOn=true;
            }
            else
            {
                animator.SetBool("LockOn", false);
                IsLockOn = false;
            }
        }
    }
    IEnumerator Run()
    {
        while (true)
        {
            UseStamina(0.5f);
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator DrinkPotion()
    {
        float time =1.0f;
        isDrink = true;
        animator.SetBool("Drink", isDrink);
        while (true)
        {
           if(animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
            {
                if(time>=0.0f)
                {
                    time -= Time.deltaTime;
                }
                else
                {
                    isDrink=false;
                    animator.SetBool("Drink", isDrink);
                    yield break;
                }
                animator.SetLayerWeight(1, time);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public void DrinkSound()
    {
        SoundManager.Instance.PlaySFXSound("Sound/Drink");
    }
    public void PlayFootStepSound()
    {
        int value = UnityEngine.Random.Range(0, 8);
        string sound = "Sound/Footstep_Dirt_0";
        sound += value;
        SoundManager.Instance.PlaySFXSound(sound);
    }
    public void PlayRollSound()
    {
        SoundManager.Instance.PlaySFXSound("Sound/Inventory_Open_01");
    }
    IEnumerator Die()
    {
        IsDead = true;
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1.0f);
        SoundManager.Instance.PlaySFXSound("Sound/Jingle_Lose_00");
        yield return new WaitForSeconds(5.0f);
        LoadingSceneContoller.LoadScene("StartScene");
        yield break ;
    }


    public bool IsRoll { get => isRoll; set => isRoll = value; }
    public PlayerAttack PlayerAttack { get => playerAttack; }
    public bool IsHit { get => isHit; set => isHit = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool IsLockOn { get => isLockOn; set => isLockOn = value; }
}
