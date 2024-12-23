using UnityEngine;
using UnityEngine.EventSystems;

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
        Debug.Log(inventoryGrid.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.IsDropItem = true;
        inventoryController.SelectedItemGrid = inventoryController.OriginSelectedItemGrid;
    }

}
