using UnityEngine;

public class InventoryQuickSelect : MonoBehaviour
{
    public InventorySlot[] slots; // slots[0] = Slot1, slots[1] = Slot2
    private int selectedIndex = -1; // -1 = 無選擇
    public Transform player; // 拖入玩家 Transform
    public GameObject itemPrefab; // 丟出的物品 prefab（需含 ItemScript）

    void Update()
    {
        // 選中 slot（數字鍵）
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0); // 對應 slots[0] = Slot1
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1); // 對應 slots[1] = Slot2

        // 按 Y 丟出選中物品（不取消選中）
        if (Input.GetKeyDown(KeyCode.Y) && selectedIndex >= 0 && slots[selectedIndex].GetItem() != null)
        {
            DropSelectedItem();
        }

        // 其他操作鍵 → 取消選擇
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

    // 計算掉落位置 → 玩家腳下，並嘗試貼地
    Vector3 dropPos = player.position;
    if (Physics.Raycast(player.position, Vector3.down, out RaycastHit hitInfo, 2f))
    {
        dropPos = hitInfo.point + new Vector3(0, 0.05f, 0); // 稍微浮起
    }

    // 生成掉落物品
    GameObject go = Instantiate(itemPrefab, dropPos, Quaternion.identity);

    // 設置 item 資訊
    var itemScript = go.GetComponent<ItemScript>();
    if (itemScript != null)
        itemScript.Initialize(item);

    // 加入彈跳效果
    Rigidbody rb = go.GetComponent<Rigidbody>();
    if (rb != null)
    {
        // 加一點隨機力讓它彈一下，看起來像被丟出去
        Vector3 randomForce = new Vector3(Random.Range(-0.2f, 0.2f), 1f, Random.Range(-0.2f, 0.2f));
        rb.AddForce(randomForce * 2f, ForceMode.Impulse);
    }

    // 從背包移除
    Inventory inventory = FindObjectOfType<Inventory>();
    inventory.RemoveItem(item);

    // 如果該物品已經丟完，自動取消選中
    if (slots[selectedIndex].GetItem() == null)
    {
        Deselect();
    }
}

}
