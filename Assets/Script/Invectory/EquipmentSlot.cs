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
        if (slotType != item.ItemData.ItemType) return false;
        OverlapCheck(ref overlapItem);

        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        equippedItem = item;
        Vector2 pos = new Vector2(tileWidth/2, -tileHeight/2);
        rectTransform.localPosition = pos;

        character.EquipItem(item.ItemData);


        return true;
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
