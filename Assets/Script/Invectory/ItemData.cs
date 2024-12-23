using System;

[Serializable]
public class ItemData 
{
    public int id = 0;
    public string name = "";
    public int width = 1;
    public int height = 1;
    public string itemIcon;
    public string mesh;
    public ITEMTYPE itemType;
    public ITEMTYPE slotType;
    public string hitEffect;
    public int damage;

}
[Serializable]
public class ItemDataArray
{ 
    public ItemData[] ItemDatas;
}
