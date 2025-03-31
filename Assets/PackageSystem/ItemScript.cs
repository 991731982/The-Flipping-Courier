using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public int defaultStack = 1;

    public PackageType packageType; // ✅ 新增這個

    private Item item;

    public float weight = 1f;

    void Start()
{
    item = new Item(); // ← 永遠是新的，不和其他物件共用
    item.itemName = itemName;
    item.itemIcon = itemIcon;
    item.stackSize = defaultStack;
    item.packageType = packageType;
    item.weight = weight;
}


    public Item GetItem()
{
    // 回傳新的 item 副本
    return new Item
    {
        itemName = item.itemName,
        itemIcon = item.itemIcon,
        stackSize = item.stackSize,
        packageType = item.packageType,
        weight = item.weight
        
    };
}


    public void Initialize(Item source)
    {
        item = new Item();
        item.itemName = source.itemName;
        item.itemIcon = source.itemIcon;
        item.stackSize = source.stackSize;
        item.packageType = source.packageType;

        itemName = source.itemName;
        itemIcon = source.itemIcon;
        packageType = source.packageType;
    }
}
