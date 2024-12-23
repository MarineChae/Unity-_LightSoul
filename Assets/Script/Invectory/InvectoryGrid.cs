using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;




public class InvectoryGrid : MonoBehaviour
{
    public const float tileWidth = 25.6f;
    public const float tileHeight = 25.6f;
    public const float ratio = 10;
    private RectTransform rectTransform;
    private Vector2 positionOnTheGrid = new Vector2(0, 0);
    private Vector2Int tileGridPosition = new Vector2Int();
    private InventoryItem[,] inventoryItemsSlot;
    [SerializeField] private int gridSizeWidth;
    [SerializeField] private int gridSizeHeight;
    [SerializeField] private ITEMTYPE itemSlotType;
    internal ITEMTYPE ItemSlotType { get => itemSlotType; set => itemSlotType = value; }

    private void Start()
    {
         rectTransform = GetComponent<RectTransform>();
         Init(gridSizeWidth, gridSizeHeight );

    }
    private void Init(int width, int height)
    {
        inventoryItemsSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2 (width * tileWidth * ratio, height * tileHeight * ratio);
        rectTransform.sizeDelta = size;

    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {    // 현재 화면 해상도 기준으로 보정된 마우스 위치를 계산
        Vector2 adjustedMousePosition = AdjustForResolution(mousePosition);
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileHeight);

        return tileGridPosition;
    }
    private Vector2 AdjustForResolution(Vector2 mousePosition)
    {
        // CanvasScaler가 있다면 UI 스케일링 계산
        CanvasScaler canvasScaler = rectTransform.GetComponentInParent<CanvasScaler>();
        if (canvasScaler != null)
        {
            float referenceWidth = canvasScaler.referenceResolution.x;
            float referenceHeight = canvasScaler.referenceResolution.y;
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // 스케일 팩터 계산
            float scaleFactorX = screenWidth / referenceWidth;
            float scaleFactorY = screenHeight / referenceHeight;

            // 해상도 보정된 마우스 위치
            return new Vector2(mousePosition.x / scaleFactorX, mousePosition.y / scaleFactorY);
        }

        // CanvasScaler가 없는 경우, 기본적으로 화면 해상도만 고려
        return mousePosition;
    }
    public bool PlaceItemWithCheck(InventoryItem item , int posX, int posY, ref InventoryItem overlapItem)
    {

        if (!BoundaryCheck(posX, posY, item.WIDTH, item.HEIGHT))
            return false;

        if (!OverlapCheck(posX, posY, item.WIDTH, item.HEIGHT, ref overlapItem))
        {
            overlapItem = null;
            return false;

        }

        if (overlapItem != null)
        {
            CleanGridRef(overlapItem);
        }

       
        PlaceItem(item, posX, posY);
        return true;
    }

    public void PlaceItem(InventoryItem item, int posX, int posY)
    {
        if (item.ItemData.itemType == ITEMTYPE.POTION && itemSlotType == ITEMTYPE.NONE)
        {            /////ui에 포션 갯수정보 추가
            EventManager.Instance.PotionTriggerAction("GET");
        }
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < item.WIDTH; ++x)
        {
            for (int y = 0; y < item.HEIGHT; ++y)
            {
                inventoryItemsSlot[posX + x, posY + y] = item;
            }
        }

        item.onGridPosX = posX;
        item.onGridPosY = posY;

        Vector2 position = ComputePositionGrid(item, posX, posY);

        rectTransform.localPosition = position;
    }
    public void EquipItem(InventoryItem item)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        inventoryItemsSlot[0,0] = item;
        Vector2 position = ComputePositionGrid(item, 0, 0);

        item.WIDTH = 1; item.HEIGHT = 1;

        rectTransform.localPosition = new Vector3(0,0,0);
    }
    public Vector2 ComputePositionGrid(InventoryItem item, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = (posX * tileWidth + tileWidth / 2 * item.WIDTH) * ratio;
        position.y = -(posY * tileHeight + tileHeight / 2 * item.HEIGHT) * ratio;
        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for(int x = 0; x < width;++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (inventoryItemsSlot[posX + x, posY + y] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemsSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if(overlapItem != inventoryItemsSlot[posX + x, posY + y])
                            return false;
                    }
             
                }

            }
        }

        return true;
    }
    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (inventoryItemsSlot[posX + x, posY + y] != null)
                {
                    return false;
                }

            }
        }

        return true;
    }
    internal InventoryItem PickUpItem(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridSizeWidth || y >=gridSizeHeight ) return null;
        InventoryItem ret = inventoryItemsSlot[x, y];

        if (ret == null) return null;

        CleanGridRef(ret);

        return ret;
    }

    private void CleanGridRef(InventoryItem item)
    {
        for (int ix = 0; ix < item.WIDTH; ++ix)
        {
            for (int iy = 0; iy < item.HEIGHT; ++iy)
            {
                inventoryItemsSlot[item.onGridPosX + ix, item.onGridPosY + iy] = null;
            }
        }
    }

    bool PositionChk(int x, int y)
    {
        if(x < 0 || y < 0) return false;
        if(x>=gridSizeWidth || y>=gridSizeHeight) return false;
        return true;
    }

    public bool BoundaryCheck(int x, int y,int width,int height)
    {
        if(PositionChk(x,y)==false) return false;

        x += width-1;
        y += height-1;

        if(PositionChk(x,y) == false) return false;

        return true;
    }

    internal InventoryItem GetItem(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridSizeWidth || y >= gridSizeHeight) return null;

        return inventoryItemsSlot[x,y];
    }

    public Vector2Int? FindSapceForItem(InventoryItem item)
    {
        int height = gridSizeHeight - item.HEIGHT + 1;
        int width = gridSizeWidth - item.WIDTH + 1;
        for (int y = 0; y < height; y++) 
        {
            for (int x = 0; x < width; x++)
            {
                if(CheckAvailableSpace(x,y,item.WIDTH,item.HEIGHT))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }

    public void InsertItem(int num)
    {
        var itemPrefab = Resources.Load("Item");
        InventoryItem item = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        item.Set(DataManager.Instance.dicItemDatas[num]);

        Vector2Int? posOnGrid = FindSapceForItem(item);

        if (posOnGrid == null) return ;

        PlaceItem(item, posOnGrid.Value.x, posOnGrid.Value.y);

    }


    public InventoryItem UsePotion()
    {
        InventoryItem ret = null;
        for (int x = 0 ; x < gridSizeWidth; x++)
        {
            for(int y = 0; y < gridSizeHeight; y++)
            {
                if (inventoryItemsSlot[x, y] != null &&  inventoryItemsSlot[x, y].ItemData.itemType == ITEMTYPE.POTION)
                {
                    ret = inventoryItemsSlot[x, y];
                    break;
                }
            }
        }

        if (ret == null) return null;

        CleanGridRef(ret);

        return ret;
    }

}
