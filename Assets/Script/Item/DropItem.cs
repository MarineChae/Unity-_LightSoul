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
            if(inventoryController.InsertItem(inventoryItem))
            {
                Destroy(this.gameObject);
            }

        }

    }
    public void Init(ItemData data)
    {
        if (data.id == 0) return;
        isInit = true;
        data = DataManager.Instance.dicItemDatas[data.id];
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

        var newMesh = Resources.Load<Mesh>(data.mesh);
        meshFilter.mesh = newMesh;
        
    }


}
