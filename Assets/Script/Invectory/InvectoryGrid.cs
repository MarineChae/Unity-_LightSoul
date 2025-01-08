using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;





public class InvectoryGrid : MonoBehaviour
{
    [SerializeField] private int gridSizeWidth;
    [SerializeField] private int gridSizeHeight;
    [SerializeField] private ITEMTYPE itemSlotType;
    [SerializeField] public Canvas rootCanvas;

    public const float tileWidth = 25.6f;
    public const float tileHeight = 25.6f;
    public const float ratio = 10;
    private float canvasScale = 0.0f;
    private RectTransform rectTransform;
    private InventoryItem[,] inventoryItemsSlot;
    private Vector2 positionOnTheGrid = new Vector2(0, 0);
    private Vector2Int tileGridPosition = new Vector2Int();

    /////////////////////////////// Life Cycle ///////////////////////////////////
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init(gridSizeWidth, gridSizeHeight);
    }

    private void Init(int width, int height)
    {
        rectTransform = GetComponent<RectTransform>();
        inventoryItemsSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2 (width * tileWidth * ratio, height * tileHeight * ratio);
        rectTransform.sizeDelta = size;
        canvasScale = rootCanvas.scaleFactor;
    }


    /////////////////////////////// Private Method ///////////////////////////////////

    //�������� �׸��� ���� �ִ��� Ȯ���ϱ����� �޼���
    private bool PositionChk(int x, int y)
    {
        if (x < 0 || y < 0) return false;
        if (x >= gridSizeWidth || y >= gridSizeHeight) return false;
        return true;
    }


    /////////////////////////////// public Method ///////////////////////////////////

    //�׸����� ��ġ�� ã������ �޼���
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {  
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / (tileWidth * rootCanvas.scaleFactor));
        tileGridPosition.y = (int)(positionOnTheGrid.y / (tileHeight * rootCanvas.scaleFactor));

        return tileGridPosition;
    }

    //�������� �ش��ϴ� ��ġ�� �ֱ����� Ȯ�� �� �ִ��Լ�
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

       
        PlaceItem(item, posX, posY,true);
        return true;
    }

    public void PlaceItem(InventoryItem item, int posX, int posY,bool playSound)
    {
        if(playSound)
            SoundManager.Instance.PlaySFXSound("Sound/Inventory_Open_00");
        if (item.ItemData.itemType == ITEMTYPE.POTION && itemSlotType == ITEMTYPE.NONE)
        {            /////ui�� ���� �������� �߰�
            EventManager.Instance.PotionTriggerAction("GET");
        }
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < item.WIDTH; ++x)
        {
            for (int y = 0; y < item.HEIGHT; ++y)
            {
                InventoryItemsSlot[posX + x, posY + y] = item;
            }
        }

        item.OnGridPosX = posX;
        item.OnGridPosY = posY;

        Vector2 position = ComputePositionGrid(item, posX, posY);

        rectTransform.localPosition = position;
    }

    //�׸��� ��ġ�� ����ϱ� ���� �޼���
    public Vector2 ComputePositionGrid(InventoryItem item, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = (posX * tileWidth + tileWidth / 2 * item.WIDTH) * ratio;
        position.y = -(posY * tileHeight + tileHeight / 2 * item.HEIGHT) * ratio;
        return position;
    }

    //�ش��ϴ� ��ġ�� �������� �ִ��� Ȯ���ϴ� �޼���
    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for(int x = 0; x < width;++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (InventoryItemsSlot[posX + x, posY + y] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = InventoryItemsSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if(overlapItem != InventoryItemsSlot[posX + x, posY + y])
                            return false;
                    }
             
                }

            }
        }

        return true;
    }
    //�ش��ϴ� ��ġ�� ����ִ��� Ȯ���ϴ� �޼���
    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (InventoryItemsSlot[posX + x, posY + y] != null)
                {
                    return false;
                }

            }
        }

        return true;
    }
    //�κ��丮���� �������� �Ⱦ��ϴ� �޼���
    internal InventoryItem PickUpItem(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridSizeWidth || y >=gridSizeHeight ) return null;
        InventoryItem ret = InventoryItemsSlot[x, y];

        if (ret == null) return null;

        CleanGridRef(ret);

        return ret;
    }
    //�Ⱦ� �� �� �������� �׸��忡�� ����ֱ� ���� �޼���
    private void CleanGridRef(InventoryItem item)
    {
        for (int ix = 0; ix < item.WIDTH; ++ix)
        {
            for (int iy = 0; iy < item.HEIGHT; ++iy)
            {
                InventoryItemsSlot[item.OnGridPosX + ix, item.OnGridPosY + iy] = null;
            }
        }
    }

    //�������� �׸��� ���� �ִ��� Ȯ���ϱ� ���� �޼���
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

        return InventoryItemsSlot[x,y];
    }

    //�׸��忡 �������� ������ ������ ã�� �޼���
    //���۽� ���ڿ� �������� �ְų� ������������ ���� �� ���
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
    
    //�������� �׸��忡 �ֱ����� ����ϴ� �޼���
    public void InsertItem(int num)
    {
        var itemPrefab = Resources.Load("Item");
        InventoryItem item = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        item.Set(DataManager.Instance.dicItemDatas[num], canvasScale);

        Vector2Int? posOnGrid = FindSapceForItem(item);

        if (posOnGrid == null) return ;

        PlaceItem(item, posOnGrid.Value.x, posOnGrid.Value.y,false);

    }
    //���� ��� �� �κ��丮���� �����ϱ� ���� �޼���
    public InventoryItem UsePotion()
    {
        InventoryItem ret = null;
        for (int x = 0 ; x < gridSizeWidth; x++)
        {
            for(int y = 0; y < gridSizeHeight; y++)
            {
                if (InventoryItemsSlot[x, y] != null &&  InventoryItemsSlot[x, y].ItemData.itemType == ITEMTYPE.POTION)
                {
                    ret = InventoryItemsSlot[x, y];
                    break;
                }
            }
        }

        if (ret == null) return null;

        CleanGridRef(ret);

        return ret;
    }

    /////////////////////////////// Property ///////////////////////////////////
    internal ITEMTYPE ItemSlotType { get => itemSlotType; set => itemSlotType = value; }
    public InventoryItem[,] InventoryItemsSlot { get => inventoryItemsSlot; }
}
