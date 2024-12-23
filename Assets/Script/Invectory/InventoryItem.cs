using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData ItemData;

    public int originHeight;
    public int originWidth;
    public int HEIGHT
    {
        get 
        { 
            if(!rotated)
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
    public int onGridPosX;
    public int onGridPosY;

    private bool rotated = false;

    internal void ChangeRotateState()
    {
        rotated = !rotated;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0,0,rotated ? 90 : 0);

    }

    internal void Set(ItemData itemData)
    {
        this.ItemData = itemData;

        var icon = Resources.Load<Sprite>(itemData.itemIcon);
        GetComponent<Image>().sprite = icon;

        Vector2 size = new Vector2();
        size.x = WIDTH * InvectoryGrid.tileWidth;
        size.y = HEIGHT * InvectoryGrid.tileHeight;

        originWidth = WIDTH;
        originHeight = HEIGHT;

        GetComponent<RectTransform>().sizeDelta = size;
    }


}
