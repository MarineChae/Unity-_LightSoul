using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �κ��丮�� ��ȣ�ۿ��� ���� ��ũ��Ʈ
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
