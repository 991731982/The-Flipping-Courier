using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<Item> inventoryList;

    // delegate event for inventory updates
    public delegate void InventoryDelegate();
    public static event InventoryDelegate OnInventoryChange;

    void Start()
    {
        inventoryList = new List<Item>();
    }

    public List<Item> GetInventoryList()
    {
        return inventoryList;
    }

    public void AddItem(Item item)
    {
        if (inventoryList.Count == 0)
        {
            inventoryList.Add(item);
        }
        else
        {
            bool inList = false;

            foreach (Item i in inventoryList)
            {
                if (item.itemName == i.itemName)
                {
                    i.stackSize++;
                    inList = true;
                    break;
                }
            }

            if (!inList)
            {
                inventoryList.Add(item);
            }
        }

        OnInventoryChange?.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ItemScript otherItemScript = other.GetComponent<ItemScript>();
            Item otherItem = otherItemScript.GetItem();

            AddItem(otherItem);
            other.gameObject.SetActive(false);
        }
    }
}
