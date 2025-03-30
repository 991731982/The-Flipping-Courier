using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string itemName;
    public Sprite itemIcon;
    public int stackSize = 1;

    public bool IsSame(Item other)
    {
        return itemName == other.itemName;
        // 如果未來你有 itemID、品質、強化屬性等等，可以在這裡擴充比較條件
    }
}
