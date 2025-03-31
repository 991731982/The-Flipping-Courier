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
    // mainpackage 只能放 slot0，如果已經有就不加
    if (item.packageType == PackageType.Main)
    {
        if (inventoryList.Count > 0)
        {
            foreach (Item i in inventoryList)
            {
                if (i.packageType == PackageType.Main)
                {
                    Debug.Log("主線包裹已存在，不可再放！");
                    return;
                }
            }
        }

        inventoryList.Insert(0, item); // 插入 slot0 位置
        OnInventoryChange?.Invoke();
        return;
    }

    // 其他類型照正常流程（side / usable）
    foreach (Item i in inventoryList)
    {
        if (i.IsSame(item))
        {
            i.stackSize += item.stackSize;
            OnInventoryChange?.Invoke();
            return;
        }
    }

    inventoryList.Add(item);
    OnInventoryChange?.Invoke();
}


    // void OnTriggerEnter(Collider other)
// {
//     if (other.CompareTag("Item"))
//     {
//         ItemScript itemScript = other.GetComponent<ItemScript>();
//         Item newItem = itemScript.GetItem();

//         AddItem(newItem);
//         other.gameObject.SetActive(false);
//     }
// }
    public void RemoveItem(Item item)
{
    for (int i = 0; i < inventoryList.Count; i++)
    {
        if (inventoryList[i].IsSame(item))
        {
            inventoryList[i].stackSize--;
            if (inventoryList[i].stackSize <= 0)
                inventoryList.RemoveAt(i);

            OnInventoryChange?.Invoke();
            return;
        }
    }
}


}
