using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipItem : MonoBehaviour
{
    [SerializeField]
    private Object itemPrefab;
    private ItemData itemData;

    private bool isInit = false;
    private bool isWeapon = false;
    public bool IsWeapon
    {
        get { return isWeapon; }
        set { isWeapon = value; }
    }
    public Weapon weaponData;

    private void Start()
    {
        if (!isInit)
            Init(itemData);

           
    }

    public void Init(ItemData data)
    {
        if (data == null) return;
        isInit = true;
        SetItemData(data);
        if (IsWeapon)
        {
            gameObject.AddComponent<CapsuleCollider>().isTrigger = true;
            weaponData = gameObject.AddComponent<Weapon>();
        }
    }
    public void SetItemData(ItemData data)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = data.mesh;
    }

}
