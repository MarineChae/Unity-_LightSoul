using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 아이템 장착을 위한 스크립트
/// </summary>
[RequireComponent(typeof(EquipmentSlot))]
public class EquipmentInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private InventoryController inventoryController;
    private EquipmentSlot equipmentInventory;
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        equipmentInventory = GetComponent<EquipmentSlot>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.IsDropItem = false;
        inventoryController.SelctedEquipmentSlot = equipmentInventory;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.IsDropItem = true;
        inventoryController.SelctedEquipmentSlot = null;
    }



}
