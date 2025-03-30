using System.Collections.Generic;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{
    public InventorySlot[] slots;
    private Inventory inventoryScript;

    void Start()
    {
        inventoryScript = FindObjectOfType<Inventory>();
        Inventory.OnInventoryChange += UpdateBar;
    }

    void OnDestroy()
    {
        Inventory.OnInventoryChange -= UpdateBar;
    }

    void UpdateBar()
    {
        // 清空所有 slot
        foreach (var slot in slots)
{
    slot.ClearSlot(); // 先清空所有 slot
}


        // 填充當前物品
        List<Item> inventoryList = inventoryScript.GetInventoryList();

        for (int i = 0; i < inventoryList.Count && i < slots.Length; i++)
        {
            slots[i].SetSlot(inventoryList[i]);
        }
    }
}
