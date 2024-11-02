using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    List<Mesh> meshList;


    InventoryItem inventoryItem;
    InventoryController inventoryController;
    [SerializeField]
    private Object itemPrefab;
    [SerializeField]
    private ItemData itemData;
    private void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        var mesh = meshList[Random.Range(0, meshList.Count)];
        meshFilter.mesh = mesh;

        inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        inventoryItem.Set(itemData);
    }
    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            inventoryController.InsertItem(inventoryItem);
            Debug.Log("hit");
            Destroy(this.gameObject);
        }

    }

}
