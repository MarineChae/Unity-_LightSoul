using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData ItemData;

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

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = WIDTH * InvectoryGrid.tileWidth;
        size.y = HEIGHT * InvectoryGrid.tileHeight;

        GetComponent<RectTransform>().sizeDelta = size;
    }


}
