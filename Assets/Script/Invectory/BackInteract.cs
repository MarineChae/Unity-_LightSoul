using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
