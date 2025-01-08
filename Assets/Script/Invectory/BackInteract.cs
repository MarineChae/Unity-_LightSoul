using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 인벤토리와 상호작용을 위한 스크립트
/// </summary>
public class BackInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private InventoryController inventoryController;
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.IsDropItem = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.IsDropItem = true;
    }


}
