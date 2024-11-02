using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    private InvectoryGrid selectedItemGrid;
    public InvectoryGrid SelectedItemGrid {
        get => selectedItemGrid; 
        set
        {
            selectedItemGrid = value;
            inventoryHighLight.SetParent(value);
        }
    }

    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTrasform;

    InventoryHighLight inventoryHighLight;
    InventoryItem itemToHighLight;
    Vector2Int oldPosition;

    private void Awake()
    {
        inventoryHighLight = GetComponent<InventoryHighLight>();
    }

    private void Update()
    {
        DragItemIcon();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(selectedItem == null)
                CreateRandomItem();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            InsertRandomItem();
        }
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
        InsertItem(item);
    }

    private void InsertItem(InventoryItem item)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSapceForItem(item);

        if (posOnGrid == null) return;

        selectedItemGrid.PlaceItem(item, posOnGrid.Value.x,posOnGrid.Value.y);
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
        bool chk = selectedItemGrid.PlaceItemWithCheck(selectedItem, positionOnGrid.x, positionOnGrid.y ,ref overlapItem);
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

    private void ItemPickup(Vector2Int positionOnGrid)
    {
        selectedItem = selectedItemGrid.PickUpItem(positionOnGrid.x, positionOnGrid.y);
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
}
