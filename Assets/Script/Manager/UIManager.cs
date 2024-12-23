using UnityEngine;

public class UIManager : SingleTon<UIManager>
{
    [SerializeField]
    InventoryController inventoryController;
    [SerializeField]
    PotionSlot potionSlot;

    public InventoryController InventoryController { get => inventoryController; }
    public PotionSlot PotionSlot { get => potionSlot;}
}
