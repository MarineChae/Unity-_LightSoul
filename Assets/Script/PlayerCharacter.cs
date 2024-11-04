using System.Collections;
using System.Collections.Generic;
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
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }


    void Start()
    {
        itemDatas = new ItemData[(int)ITEMTYPE.END];
        equipItems = new EquipItem[(int)ITEMTYPE.END];
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }
    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        Move();
        if(itemDatas[(int)ITEMTYPE.WEAPON] == null)
        {
            animator.SetBool("EquipWeapon", false);
        }
        else
        {
            animator.SetBool("EquipWeapon", true);
        }
    }

    private void Move()
    {
        moveVector = navMeshAgent.velocity;

        if (moveVector != Vector3.zero)
        {
            animator.SetBool("Walk", true);

        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    public void LateUpdateWork() { }

    public void EquipItem(ItemData itemData)
    {
        itemDatas[(int)itemData.SlotType] = itemData;
        if(itemData.SlotType == ITEMTYPE.WEAPON )
        {
            EquipWeapon(itemData, weaponSocket.transform);
        }
        else if(itemData.SlotType == ITEMTYPE.WEAPON2)
        {
            EquipWeapon(itemData ,weapon2Socket.transform);
        }
    }

    private void EquipWeapon(ItemData itemData,Transform socketTransform)
    {
        equipItems[(int)itemData.SlotType] = Instantiate(equipItem);
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
            var des = equipItems[(int)itemData.SlotType];
            equipItems[(int)itemData.SlotType] = null;
            Destroy(des.gameObject);

        }
    }


}
