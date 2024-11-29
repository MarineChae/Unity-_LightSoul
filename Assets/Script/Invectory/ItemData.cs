using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
[Serializable]
public class ItemDataArray
{ 
    public ItemData[] ItemDatas;
}
