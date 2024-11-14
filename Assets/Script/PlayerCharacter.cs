using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacter : MonoBehaviour, IUpdatable
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 moveVector;
    [HideInInspector]
    public ItemData[] itemDatas;
    [SerializeField]
    private Transform weaponSocket;
    [SerializeField]
    private Transform weapon2Socket;
    [SerializeField]
    private EquipItem equipItem;
    private EquipItem[] equipItems;
    private Weapon[] equipWeapon;
    private bool isRoll=false;
    private bool isAttack = false;
    private float attackDelay = 0.0f;
    private bool canAttack = true;
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
        itemDatas = new ItemData[(int)ITEMTYPE.END];
        equipItems = new EquipItem[(int)ITEMTYPE.END];
        equipWeapon = new Weapon[2];
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }
    public void FixedUpdateWork() 
    {
        Move();
    }
    public void UpdateWork()
    {

        if (moveVector != Vector3.zero)
        {
            animator.SetBool("Walk", true);

        }
        else
        {
            animator.SetBool("Walk", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Roll");
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            isRoll = true;
        }
        if (itemDatas[(int)ITEMTYPE.WEAPON] == null)
        {
            animator.SetBool("EquipWeapon", false);
        }
        else
        {
            animator.SetBool("EquipWeapon", true);
        }

        Attack();

    }

    private void Attack()
    {
        if (equipWeapon[0] == null) return;
        attackDelay += Time.deltaTime;
        canAttack = equipWeapon[0].attackRate < attackDelay;
        if (Input.GetKeyDown(KeyCode.A) && canAttack)
        {
            equipWeapon[0].Attack();
            animator.SetTrigger("Attack");
            attackDelay = 0.0f;
            isAttack = true;
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }
    }
    public void AttackEnd()
    {
        navMeshAgent.destination = transform.position;
        isAttack = false;
        navMeshAgent.isStopped = false;
        navMeshAgent.velocity = Vector3.zero;
    }
    private void Move()
    {
        moveVector = navMeshAgent.velocity;

        if (navMeshAgent.remainingDistance >= 0.5f && !isRoll && !isAttack && navMeshAgent.desiredVelocity != Vector3.zero)
        {
            Vector3 direction = navMeshAgent.desiredVelocity;
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * 10.0f);


        }
    }

    void RollEnd()
    {
        isRoll=false;
        navMeshAgent.destination = transform.position;
        navMeshAgent.isStopped = false;
        navMeshAgent.velocity = Vector3.zero;
    }


    public void LateUpdateWork() { }

    public void EquipItem(ItemData itemData)
    {
        itemDatas[(int)itemData.SlotType] = itemData;
        if(itemData.SlotType == ITEMTYPE.WEAPON )
        {
            EquipWeapon(itemData, weaponSocket.transform);
            equipWeapon[0] = equipItems[(int)ITEMTYPE.WEAPON].weaponData;
        }
        else if(itemData.SlotType == ITEMTYPE.WEAPON2)
        {
            EquipWeapon(itemData ,weapon2Socket.transform);
            equipWeapon[1] = equipItems[(int)ITEMTYPE.WEAPON2].weaponData;
        }
    }

    private void EquipWeapon(ItemData itemData,Transform socketTransform)
    {
        equipItems[(int)itemData.SlotType] = Instantiate(equipItem);
        equipItems[(int)itemData.SlotType].IsWeapon = true;
        equipItems[(int)itemData.SlotType].Init(itemData);
        equipItems[(int)itemData.SlotType].transform.SetParent(socketTransform);
        equipItems[(int)itemData.SlotType].transform.localPosition = Vector3.zero;
        equipItems[(int)itemData.SlotType].transform.localRotation = Quaternion.identity;

    }

    public void UnEquipItem(ItemData itemData)
    {
        itemDatas[(int)itemData.SlotType] = null;
        if (itemData.ItemType == ITEMTYPE.WEAPON || itemData.ItemType == ITEMTYPE.WEAPON2)
        {
            equipWeapon[(int)itemData.SlotType - 1] = null;
            var des = equipItems[(int)itemData.SlotType];
            equipItems[(int)itemData.SlotType] = null;
            Destroy(des.gameObject);

        }
    }


}
