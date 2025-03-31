using System.Collections.Generic;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{
    public InventorySlot[] slots; // slot[0] = 主線包裹, slot[1] 和 slot[2] = 快捷欄
    private Inventory inventoryScript;
    private List<Item> inventoryList;

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
        inventoryList = inventoryScript.GetInventoryList();

        // 先清空所有 slot
        foreach (InventorySlot slot in slots)
        {
            slot.ClearSlot();
        }

        // 主線包裹進 slots[0]
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].packageType == PackageType.Main)
            {
                slots[0].SetSlot(inventoryList[i]);
                break;
            }
        }

        // 其他物品塞入 slots[1]、slots[2]
        int slotIndex = 1;
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].packageType != PackageType.Main)
            {
                if (slotIndex < slots.Length)
                {
                    slots[slotIndex].SetSlot(inventoryList[i]);
                    slotIndex++;
                }
            }
        }

        // 多餘 slot 已在最前面清空，這裡可以略過
    }
}
