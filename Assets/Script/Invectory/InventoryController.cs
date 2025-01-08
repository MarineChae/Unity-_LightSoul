
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ITEMTYPE
{
    NONE = 0,
    WEAPON,
    WEAPON2,
    BODY,
    HEAD,
    GLOVES,
    BOOTS,
    POTION,
    CHEST,
    END,
}


public class InventoryController : MonoBehaviour , IUpdatable
{
    [SerializeField]
    private InvectoryGrid originSelectedItemGrid;
    [SerializeField]
    private DropItem DropItem;
    [SerializeField]
    private List<ItemData> items;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private Transform canvasTrasform;
    [SerializeField]
    private Canvas inventoryUI;
    private InvectoryGrid selectedItemGrid;
    private EquipmentSlot selctedEquipmentSlot;
    private InventoryItem selectedItem;
    private InventoryItem overlapItem;
    private RectTransform rectTransform;
    private InventoryHighLight inventoryHighLight;
    private InventoryItem itemToHighLight;
    private FollowCamera followCamera;
    private Vector2Int oldPosition;
    private bool isDropItem = false;
    public bool inventoryState = true;

    /////////////////////////////// Life Cycle ///////////////////////////////////

    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }


    private void Awake()
    {
        inventoryHighLight = GetComponent<InventoryHighLight>();
        inventoryHighLight.SetParent(selectedItemGrid);
        SelectedItemGrid = originSelectedItemGrid;
        StartCoroutine(InitInventory());
        followCamera = GetComponent<FollowCamera>();
    }
    public void FixedUpdateWork() { }
    public void LateUpdateWork() { }

    public void UpdateWork()
    {


        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeInventoryState(!inventoryState);
           
        }

        if (!inventoryState) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeInventoryState(false);
        }
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
    /////////////////////////////// Private Method///////////////////////////////////

    //�������� �׸��� ������ ȸ���ϱ����� �޼���
    private void RotateItem()
    {
        if (selectedItem == null) return;

        selectedItem.ChangeRotateState();

    }
    //�κ��丮�� ��ȣ�ۿ��� ���� �޼���
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

    //�׸����� ��ġ�� ���ϱ� ���� �޼���
    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;
        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * InvectoryGrid.tileWidth * selectedItemGrid.rootCanvas.scaleFactor / 2;
            position.y += (selectedItem.HEIGHT - 1) * InvectoryGrid.tileHeight * selectedItemGrid.rootCanvas.scaleFactor / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    //�������� �׸��忡 ��ġ��Ű�� �޼���
    private void PlaceItem(Vector2Int positionOnGrid)
    {
        if (IsDropItem)
        {
            /// <summary>
            /// Deprecated
            /// Change to ThridPersonView from TopView
            /// </summary>
            //var Item = Instantiate(DropItem);
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit))
            //{
            //    Item.Init(selectedItem.ItemData);
            //    Item.transform.position = hit.point;

            //}
            //Destroy(selectedItem.gameObject);
            //selectedItem = null;
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

    //�׸��忡�� �������� �Ⱦ��ϱ� ���� �޼���
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
            if (selectedItem.ItemData.itemType == ITEMTYPE.POTION && selectedItemGrid.ItemSlotType == ITEMTYPE.NONE)
            {
                EventManager.Instance.PotionTriggerAction("DROP");
                /////ui�� ���� �������� �߰�
            }
            rectTransform = selectedItem.GetComponent<RectTransform>();

        }
    }
    //�Ⱦ��� �������� �������� ���콺�� ���� �̵��ϵ��� �ϴ� �޼���
    private void DragItemIcon()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
    /////////////////////////////// Public Method///////////////////////////////////

    //�κ��丮 ���� ���¸� �����ϴ� �޼���
    public void ChangeInventoryState(bool isOpen)
    {
        Cursor.visible = isOpen;
        if(isOpen)
        {
            UIManager.Instance.AddCanvas(inventoryUI);
            Cursor.lockState = CursorLockMode.None;
        }     
        else
            Cursor.lockState = CursorLockMode.Locked;
        inventoryState = isOpen;
        followCamera.IsUIActive = isOpen;
        inventoryUI.gameObject.SetActive(isOpen);
        SoundManager.Instance.PlaySFXSound("Sound/Inventory_Open_01");
    }

    /// <summary>
    /// Deprecated
    /// Change to ThridPersonView from TopView
    /// </summary>
    public bool InsertItem(InventoryItem item)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSapceForItem(item);

        if (posOnGrid == null)return false;

        selectedItemGrid.PlaceItem(item, posOnGrid.Value.x,posOnGrid.Value.y, true);
      
        return true;
    }
    //�׸��忡 �������� ���� ��ġ�� ���̶���Ʈ ǥ�ø� ���� �޼���
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
    public void UsePotion()
    {
        var potion = originSelectedItemGrid.UsePotion();
        Destroy(potion.gameObject);
    }

    /////////////////////////////// Coroutine //////////////////////////
    IEnumerator InitInventory()
    {
        yield return new WaitForFixedUpdate();

        inventoryState = false;
        inventoryUI.gameObject.SetActive(false);

        yield break;
    }

    /////////////////////////////// Property /////////////////////////////////
    public InvectoryGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighLight.SetParent(value);
        }
    }
    public InvectoryGrid OriginSelectedItemGrid
    {
        get => originSelectedItemGrid;
        set => originSelectedItemGrid = value;
    }
    public EquipmentSlot SelctedEquipmentSlot
    {
        get => selctedEquipmentSlot;
        set
        {
            selctedEquipmentSlot = value;
        }
    }
    public bool IsDropItem { get => isDropItem; set => isDropItem = value; }
}
