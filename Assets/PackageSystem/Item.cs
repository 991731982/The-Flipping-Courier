using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PackageType
{
    Main,
    Side,
    Usable
}

public class Item
{
    public string itemName;
    public Sprite itemIcon;
    public int stackSize = 1;
    public PackageType packageType;

     public float weight = 1f;

    public bool IsSame(Item other)
    {
        return itemName == other.itemName;
    }
}
