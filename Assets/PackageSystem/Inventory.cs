using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<Item> inventoryList = new List<Item>();

    public delegate void InventoryDelegate();
    public static event InventoryDelegate OnInventoryChange;

    public List<Item> GetInventoryList() => inventoryList;

   public void AddItem(Item item)
{
    foreach (Item i in inventoryList)
    {
        if (i.IsSame(item)) // 判斷是否為相同物品（可擴充）
        {
            i.stackSize += item.stackSize;
            OnInventoryChange?.Invoke();
            return;
        }
    }

    // 否則是新物品，加入 list
    inventoryList.Add(item);
    OnInventoryChange?.Invoke();
}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ItemScript itemScript = other.GetComponent<ItemScript>();
            Item newItem = itemScript.GetItem();

            AddItem(newItem);
            other.gameObject.SetActive(false);
        }
    }
}
