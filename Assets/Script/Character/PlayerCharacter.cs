using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

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
    private EquipItem[] equipItems;
    public Weapon[] equipWeapon;
    private PlayerAttack playerAttack;

    private bool isRoll = false;

    [SerializeField]
    private float baseHp = 100;
    public override float MaxHP => baseHp;

    public override float MaxStamina => 100;

    public override float StaminaRecovery => 5;


    private float staminerConsumption = 30;
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, true, false);
    }


    void Start()
    {
        InitStatus();
        itemDatas = new ItemData[(int)ITEMTYPE.END];
        equipItems = new EquipItem[(int)ITEMTYPE.END];
        equipWeapon = new Weapon[2];
        animator = GetComponentInChildren<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        
    }
    public void FixedUpdateWork()
    {
        
    }
    public void UpdateWork()
    {
        if (itemDatas[(int)ITEMTYPE.WEAPON] == null)
        {
            animator.SetBool("EquipWeapon", false);
        }
        else
        {
            animator.SetBool("EquipWeapon", true);
        }
        Roll();
        if (Input.GetKeyDown(KeyCode.F2))
        {
            HP = MaxHP;
        }
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Stamina >= staminerConsumption && !isRoll)
        {
            animator.SetTrigger("Roll");
            isRoll = true;
            UseStamina(staminerConsumption);
        }
    }

    void RollEnd()
    {
        isRoll=false;
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
            playerAttack.Weapon = equipWeapon[0];
        }
        else if(itemData.slotType == ITEMTYPE.WEAPON2)
        {
            EquipWeapon(itemData ,weapon2Socket.transform,false);
            equipWeapon[1] = equipItems[(int)ITEMTYPE.WEAPON2].weaponData;
            equipWeapon[1].tag = "Shield";
            playerAttack.Shield = equipWeapon[1];
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
        HP -= damage;
        Debug.Log(damage);
    }

    public override void UseStamina(float stamina)
    {
        Stamina -= stamina;
        Debug.Log(stamina);
    }
}
