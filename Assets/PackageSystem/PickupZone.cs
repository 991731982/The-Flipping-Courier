using UnityEngine;

public class PickupZone : MonoBehaviour
{
    public GameObject promptUI; // 拖入提示UI（Canvas）

    private bool playerInRange = false;

    void Start()
    {
        promptUI.SetActive(false); // 一開始不顯示
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            Pickup();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            promptUI.SetActive(true);
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            promptUI.SetActive(false);
            playerInRange = false;
        }
    }

 void Pickup()
{
    ItemScript itemScript = GetComponentInParent<ItemScript>(); // ✅ 從父物件拿到 ItemScript
    if (itemScript == null) return;

    Item item = itemScript.GetItem();
    Inventory playerInventory = FindObjectOfType<Inventory>();
    playerInventory.AddItem(item);

    promptUI.SetActive(false);
    itemScript.gameObject.SetActive(false); 
}

}
