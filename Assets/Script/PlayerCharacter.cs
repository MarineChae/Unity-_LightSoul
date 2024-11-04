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
        itemDatas[(int)itemData.ItemType] = itemData;
        if(itemData.ItemType == ITEMTYPE.WEAPON)
        {
            equipItems[(int)itemData.ItemType] = Instantiate(equipItem);
            equipItems[(int)itemData.ItemType].Init(itemData);
            equipItems[(int)itemData.ItemType].transform.SetParent(weaponSocket.transform);
            equipItems[(int)itemData.ItemType].transform.localPosition = Vector3.zero;
            equipItems[(int)itemData.ItemType].transform.localRotation = Quaternion.identity;
        }

    }
    public void UnEquipItem(ItemData itemData)
    {
        itemDatas[(int)itemData.ItemType] = null;
        if (itemData.ItemType == ITEMTYPE.WEAPON)
        {
            var des = equipItems[(int)itemData.ItemType];
            equipItems[(int)itemData.ItemType] = null;
            Destroy(des.gameObject);

        }
    }


}
