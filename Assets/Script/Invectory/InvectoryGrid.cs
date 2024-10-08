using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvectoryGrid : MonoBehaviour
{
    private const float tileWidth = 51.2f;
    private const float tileHeight = 51.2f;
    private const float ratio = 5;
    private RectTransform rectTransform;
    private Vector2 positionOnTheGrid = new Vector2(0, 0);
    private Vector2Int tileGridPosition = new Vector2Int();
    private InventoryItem[,] inventoryItemsSlot;
    [SerializeField] private int gridSizeWidth;
    [SerializeField] private int gridSizeHeight;
    [SerializeField] GameObject inventoryItemPrefab;

    private void Start()
    {
         rectTransform = GetComponent<RectTransform>();
         Init(gridSizeWidth, gridSizeHeight );

        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 5, 5);
    }
    private void Init(int width, int height)
    {
        inventoryItemsSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2 (width * tileWidth * ratio, height * tileHeight * ratio);
        rectTransform.sizeDelta = size;

    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileHeight);

        return tileGridPosition;
    }

    public void PlaceItem(InventoryItem item , int posX, int posY)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        inventoryItemsSlot[posX, posY] = item;

        Vector2 position = new Vector2 ();
        position.x = (posX * tileWidth + tileWidth / 2) * ratio;
        position.y = -(posY * tileHeight + tileHeight / 2) * ratio;

        rectTransform.localPosition = position ;
    }
}
