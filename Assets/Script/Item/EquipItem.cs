using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipItem : MonoBehaviour
{
    [SerializeField]
    private Object itemPrefab;
    private readonly ItemData itemData;
    [SerializeField] 
    private GameObject itemPrefabGameObject;
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
            var trail = Instantiate(itemPrefabGameObject);
            trail.transform.parent = transform;

        }
    }
    public void SetItemData(ItemData data)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        var newMesh = Resources.Load<Mesh>(data.mesh);
        meshFilter.mesh = newMesh;
       
    }

}
