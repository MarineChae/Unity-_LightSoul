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

    }
    public void SetItemData(ItemData data)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = data.mesh;
    }

}
