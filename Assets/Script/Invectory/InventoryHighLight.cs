using UnityEngine;

public class InventoryHighLight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void HighLight(bool isHighLight)
    {
        highlighter.gameObject.SetActive(isHighLight);
        highlighter.GetComponent<RectTransform>().SetAsFirstSibling();

    }
    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * InvectoryGrid.tileWidth;
        size.y = targetItem.HEIGHT * InvectoryGrid.tileHeight;
        highlighter.sizeDelta = size;

    }

    public void SetPosition(InvectoryGrid targetGrid, InventoryItem targetItem)
    { 
        Vector2 pos = targetGrid.ComputePositionGrid(targetItem, targetItem.onGridPosX, targetItem.onGridPosY);
        highlighter.localPosition = pos;
    }

    public void SetParent(InvectoryGrid targetGrid)
    {
        if (targetGrid == null) return;

        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(InvectoryGrid targetGrid, InventoryItem targetItem,int posX, int posY)
    {
        Vector2 pos = targetGrid.ComputePositionGrid(targetItem, posX, posY);

        highlighter.localPosition = pos;
    }

}
