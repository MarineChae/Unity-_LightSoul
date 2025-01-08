using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �κ��丮�� ���� ������ ��ũ��Ʈ
/// </summary>
public class InventoryItem : MonoBehaviour
{
    private ItemData itemData;
    private int onGridPosX;
    private int onGridPosY;
    private bool rotated = false;

    /////////////////////////////// Public Method ///////////////////////////////////
    
    //������ ȸ���� ���� �޼���
    public void ChangeRotateState()
    {
        rotated = !rotated;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0,0,rotated ? 90 : 0);
    }
    //������ ���� �ʱ�ȭ
    public void Set(ItemData itemData,float canvasScaleFacter)
    {
        this.ItemData = itemData;

        var icon = Resources.Load<Sprite>(itemData.itemIcon);
        GetComponent<Image>().sprite = icon;
        SetSize(canvasScaleFacter);


    }
    public void SetSize(float canvasScaleFacter)
    {
        Vector2 size = new Vector2();
        size.x = WIDTH * InvectoryGrid.tileWidth * canvasScaleFacter;
        size.y = HEIGHT * InvectoryGrid.tileHeight * canvasScaleFacter;
        var rect = GetComponent<RectTransform>();
        rect.sizeDelta = size;
    }

    /////////////////////////////// Property /////////////////////////////////
    public int HEIGHT
    {
        get
        {
            if (!rotated)
            {
                return ItemData.height;
            }
            return ItemData.width;
        }
        set
        {
            if (!rotated)
            {
                ItemData.height = value;
            }
            ItemData.width = value;
        }
    }
    public int WIDTH
    {
        get
        {
            if (!rotated)
            {
                return ItemData.width;
            }
            return ItemData.height;
        }
        set
        {
            if (!rotated)
            {
                ItemData.width = value;
            }
            ItemData.height = value;
        }
    }

    public int OnGridPosX { get => onGridPosX; set => onGridPosX = value; }
    public int OnGridPosY { get => onGridPosY; set => onGridPosY = value; }
    public ItemData ItemData { get => itemData; set => itemData = value; }
}

