using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{

    public int width = 1;
    public int height = 1;
    public Sprite itemIcon;
    public Mesh mesh;
    [SerializeField]
    private ITEMTYPE itemType;
    internal ITEMTYPE ItemType { get => itemType; set => itemType = value; }

    private ITEMTYPE slotType;
    internal ITEMTYPE SlotType { get => slotType; set => slotType = value; }

}
