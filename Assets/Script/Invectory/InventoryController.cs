using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

enum ITEMTYPE
{
    NONE = 0,
    WEAPON,
    WEAPON2,
    BODY,
    HEAD,
    GLOVES,
    BOOTS,
    END,
}


public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InvectoryGrid originSelectedItemGrid;
    public InvectoryGrid OriginSelectedItemGrid
    {   
        get => originSelectedItemGrid; 
        set => originSelectedItemGrid = value; 
    }

    private InvectoryGrid selectedItemGrid;
    public InvectoryGrid SelectedItemGrid 
    {
        get => selectedItemGrid; 
        set
        {
            selectedItemGrid = value;
            inventoryHighLight.SetParent(value);
        }
    }

    private EquipmentSlot selctedEquipmentSlot;
    public EquipmentSlot SelctedEquipmentSlot
    { 
        get => selctedEquipmentSlot;
        set
        {
            selctedEquipmentSlot = value;
            //inventoryHighLight.SetParent(value);
        }
    }


    private bool isDropItem = false;
    public bool IsDropItem { get => isDropItem; set => isDropItem = value; }


    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField]
    DropItem DropItem;
    [SerializeField] 
    List<ItemData> items;
    [SerializeField]
    GameObject itemPrefab;
    [SerializeField] 
    Transform canvasTrasform;
    [SerializeField]
    Canvas inventoryUI;
    bool inventoryState = true;
    InventoryHighLight inventoryHighLight;
    InventoryItem itemToHighLight;
    Vector2Int oldPosition;



    private void Awake()
    {
        inventoryHighLight = GetComponent<InventoryHighLight>();
        inventoryHighLight.SetParent(selectedItemGrid);
        SelectedItemGrid = originSelectedItemGrid;
        StartCoroutine(InitInventory());
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryState = !inventoryState;
            inventoryUI.gameObject.SetActive(inventoryState);
        }
        if (!inventoryState) return;
        
        DragItemIcon();
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (selectedItemGrid == null)
        {
            inventoryHighLight.HighLight(false);
            return;
        }

        HandleHighLight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftButtonPress();
        }

    }


    private void RotateItem()
    {
        if (selectedItem == null) return;

        selectedItem.ChangeRotateState();

    }

    private void InsertRandomItem()
    {
        if (selectedItemGrid == null) return;

        CreateRandomItem();
        InventoryItem item = selectedItem;
        selectedItem = null;
        if (!InsertItem(item))
            Destroy(item.gameObject);

    }

    public bool InsertItem(InventoryItem item)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSapceForItem(item);

        if (posOnGrid == null)return false;

        selectedItemGrid.PlaceItem(item, posOnGrid.Value.x,posOnGrid.Value.y);
      
        return true;
    }

    private void HandleHighLight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid) return;

        oldPosition = positionOnGrid;

        if ( selectedItem == null)
        {
            itemToHighLight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if(itemToHighLight!=null)
            {
                inventoryHighLight.HighLight(true);
                inventoryHighLight.SetSize(itemToHighLight);
                inventoryHighLight.SetPosition(selectedItemGrid, itemToHighLight);
            }
            else
            {
                inventoryHighLight.HighLight(false);
            }

        }
        else
        {
            inventoryHighLight.HighLight(selectedItemGrid.BoundaryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.WIDTH,
                selectedItem.HEIGHT)
                );
            inventoryHighLight.SetSize(selectedItem);
            inventoryHighLight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x,positionOnGrid.y);
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem item = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = item;
        rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTrasform);
        rectTransform.SetAsLastSibling();
        int itemId = UnityEngine.Random.Range(0, items.Count);
        item.Set(items[itemId]);
    }

    private void LeftButtonPress()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();

        if (selectedItem == null)
        {
            ItemPickup(positionOnGrid);
            if (selectedItem == null) return;
            selectedItem.GetComponent<RectTransform>().SetParent(canvasTrasform);
            selectedItem.GetComponent<RectTransform>().SetAsLastSibling();
        }
        else
        {
            PlaceItem(positionOnGrid);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;
        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * InvectoryGrid.tileWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * InvectoryGrid.tileHeight / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    private void PlaceItem(Vector2Int positionOnGrid)
    {
        if(IsDropItem)
        {
            var Item = Instantiate(DropItem);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Item.Init(selectedItem.ItemData);
                Item.transform.position = hit.point;

                Debug.Log("DROP");
            }

            Destroy(selectedItem.gameObject);
            selectedItem = null;
        }
        else
        {
            if (SelctedEquipmentSlot != null)
            {
                bool chk = SelctedEquipmentSlot.EquipItem(selectedItem, ref overlapItem);
                if (chk)
                {
                    selectedItem = null;
                    if (overlapItem != null)
                    {
                        selectedItem = overlapItem;
                        overlapItem = null;
                        rectTransform = selectedItem.GetComponent<RectTransform>();
                        rectTransform.SetAsLastSibling();
                    }
                }
            }
            else if (selectedItemGrid != null)
            {
                bool chk = selectedItemGrid.PlaceItemWithCheck(selectedItem, positionOnGrid.x, positionOnGrid.y, ref overlapItem);
                if (chk)
                {
                    selectedItem = null;
                    if (overlapItem != null)
                    {
                        selectedItem = overlapItem;
                        overlapItem = null;
                        rectTransform = selectedItem.GetComponent<RectTransform>();
                        rectTransform.SetAsLastSibling();
                    }
                }
            }

        }

    }

    private void ItemPickup(Vector2Int positionOnGrid)
    {
        if (SelctedEquipmentSlot != null)
        {
            selectedItem = SelctedEquipmentSlot.PickUpItem();
        }
        else if (selectedItemGrid != null)
        {
            selectedItem = selectedItemGrid.PickUpItem(positionOnGrid.x, positionOnGrid.y);
        }
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
            
        }
    }

    private void DragItemIcon()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
    IEnumerator InitInventory()
    {
        yield return new WaitForFixedUpdate();

        inventoryState = false;
        inventoryUI.gameObject.SetActive(false);
    }
}
