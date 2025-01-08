using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �κ��丮�� grid�� Ȯ���ϱ����� �޼���
/// </summary>
[RequireComponent(typeof(InvectoryGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{

    private InventoryController inventoryController;
    private InvectoryGrid       inventoryGrid;
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        inventoryGrid = GetComponent<InvectoryGrid>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.IsDropItem = false;
        inventoryController.SelectedItemGrid = inventoryGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.IsDropItem = true;
        inventoryController.SelectedItemGrid = inventoryController.OriginSelectedItemGrid;
    }

}
