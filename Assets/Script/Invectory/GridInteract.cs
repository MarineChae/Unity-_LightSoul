using System.Collections;
using System.Collections.Generic;
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
        inventoryController.SelectedItemGrid = inventoryGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;
    }


}
