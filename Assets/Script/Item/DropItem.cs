using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    InventoryItem inventoryItem;
    InventoryController inventoryController;
    [SerializeField]
    private Object itemPrefab;
    [SerializeField]
    private ItemData itemData;

    private bool isInit=false;
    private void Start()
    {
        if(!isInit)
           Init(itemData);
    }
    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            inventoryController.InsertItem(inventoryItem);
            Destroy(this.gameObject);
        }

    }
    public void Init(ItemData data)
    {
        isInit = true;
        inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        inventoryItem.transform.SetParent(this.transform);
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        SetItemData(data);
        inventoryItem.Set(inventoryItem.ItemData);
  
    }
    public void SetItemData(ItemData data)
    {
        inventoryItem.ItemData = data;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = data.mesh;
        
    }


}
