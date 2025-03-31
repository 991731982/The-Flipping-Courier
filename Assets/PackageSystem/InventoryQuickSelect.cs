using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemPrefabPair
{
    public string itemName;
    public GameObject prefab;
}

public class InventoryQuickSelect : MonoBehaviour
{
    public InventorySlot[] slots; // slots[0] = Slot1, slots[1] = Slot2
    private int selectedIndex = -1;
    public Transform player;

    [Header("Item Prefabs")]
    public ItemPrefabPair[] itemPrefabs; // 在 Inspector 中設定
    private Dictionary<string, GameObject> itemPrefabMap = new();

    void Start()
    {
        // 初始化 item prefab 對應表
        foreach (var pair in itemPrefabs)
        {
            if (!itemPrefabMap.ContainsKey(pair.itemName))
            {
                itemPrefabMap.Add(pair.itemName, pair.prefab);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);

        if (Input.GetKeyDown(KeyCode.Y) && selectedIndex >= 0 && slots[selectedIndex].GetItem() != null)
        {
            DropSelectedItem();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            Deselect();
        }
    }

    void SelectSlot(int index)
    {
        if (selectedIndex != -1)
            slots[selectedIndex].SetSelected(false);

        selectedIndex = index;
        slots[selectedIndex].SetSelected(true);
    }

    void Deselect()
    {
        if (selectedIndex != -1)
        {
            slots[selectedIndex].SetSelected(false);
            selectedIndex = -1;
        }
    }

    void DropSelectedItem()
    {
        Item item = slots[selectedIndex].GetItem();
        if (item == null) return;

        // 取得對應的 prefab
        if (!itemPrefabMap.TryGetValue(item.itemName, out GameObject prefabToDrop))
        {
            Debug.LogWarning("找不到對應的 prefab：" + item.itemName);
            return;
        }

        // 掉落位置
        Vector3 dropPos = player.position;
        if (Physics.Raycast(player.position, Vector3.down, out RaycastHit hitInfo, 2f))
        {
            dropPos = hitInfo.point + new Vector3(0, 0.05f, 0);
        }

        // 生成物品
        GameObject go = Instantiate(prefabToDrop, dropPos, Quaternion.identity);

        // 設置資料
        var itemScript = go.GetComponent<ItemScript>();
        if (itemScript != null)
            itemScript.Initialize(item);

        // 從背包移除
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.RemoveItem(item);

        if (slots[selectedIndex].GetItem() == null)
        {
            Deselect();
        }
    }
}
