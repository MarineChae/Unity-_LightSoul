using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class EquipmentSlot : MonoBehaviour
{
    private RectTransform rectTransform;
    private InventoryItem equippedItem;
    private PlayerCharacter character;
    [SerializeField]
    private float tileWidth = 51.2f;
    [SerializeField]
    private float tileHeight = 51.2f;
    [SerializeField]
    private ITEMTYPE slotType;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        character = FindAnyObjectByType<PlayerCharacter>();
    }

    public bool EquipItem(InventoryItem item, ref InventoryItem overlapItem)
    {
        if ((item.ItemData.ItemType == ITEMTYPE.WEAPON || item.ItemData.ItemType == ITEMTYPE.WEAPON2))
        {
            overlapItem = CheckAndEquip(item, overlapItem);
            return true;
        }
        if (slotType != item.ItemData.ItemType) return false;
        CheckAndEquip(item, overlapItem);

        return true;
    }

    private InventoryItem CheckAndEquip(InventoryItem item, InventoryItem overlapItem)
    {
        OverlapCheck(ref overlapItem);

        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        equippedItem = item;
        Vector2 pos = new Vector2(tileWidth / 2, -tileHeight / 2);
        rectTransform.localPosition = pos;
        item.ItemData.SlotType = slotType;
        character.EquipItem(item.ItemData);
        return overlapItem;
    }

    public InventoryItem PickUpItem()
    {
        InventoryItem ret = equippedItem;
        character.UnEquipItem(ret.ItemData);
        equippedItem = null;
        return ret;
    }
    public bool OverlapCheck(ref InventoryItem overlapItem)
    {
        if (equippedItem != null)
        {
            character.UnEquipItem(equippedItem.ItemData);
            overlapItem = equippedItem;
        
        }
        return true;
    }



}
