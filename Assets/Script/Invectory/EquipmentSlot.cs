using UnityEngine;


public class EquipmentSlot : MonoBehaviour
{
    [SerializeField]
    private float tileWidth = 51.2f;
    [SerializeField]
    private float tileHeight = 51.2f;
    [SerializeField]
    private ITEMTYPE slotType;
    private RectTransform rectTransform;
    private InventoryItem equippedItem;
    private PlayerCharacter character;

    /////////////////////////////// Life Cycle ///////////////////////////////////
    
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        character = FindAnyObjectByType<PlayerCharacter>();
    }

    /////////////////////////////// Private Method///////////////////////////////////
    
    //슬롯에 아이템이 장착되어있는지 확인 후 아이템을 장착하는 메서드
    private InventoryItem CheckAndEquip(InventoryItem item, InventoryItem overlapItem)
    {
        OverlapCheck(ref overlapItem);

        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        equippedItem = item;
        Vector2 pos = new Vector2(tileWidth / 4, -tileHeight / 4);
        rectTransform.anchoredPosition = Vector2.zero;
        item.ItemData.slotType = slotType;
        character.EquipItem(item.ItemData);
        return overlapItem;
    }

    /////////////////////////////// Public Method///////////////////////////////////
    
    //아이템을 장착하기 위해 외부에서 사용할 수 있도록 만든 메서드
    public bool EquipItem(InventoryItem item, ref InventoryItem overlapItem)
    {
        if (slotType != item.ItemData.itemType) return false;
        if ((item.ItemData.itemType == ITEMTYPE.WEAPON || item.ItemData.itemType == ITEMTYPE.WEAPON2))
        {
            overlapItem = CheckAndEquip(item, overlapItem);
            return true;
        }
        CheckAndEquip(item, overlapItem);

        return true;
    }

    //아이템 장착 해제를 위한 메서드
    public InventoryItem PickUpItem()
    {
        InventoryItem ret = equippedItem;
        if (ret == null)
            return null;
        character.UnEquipItem(ret.ItemData);
        equippedItem = null;
        return ret;
    }
    //이미 장착한 아이템이 있는지 확인하는 메서드
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
