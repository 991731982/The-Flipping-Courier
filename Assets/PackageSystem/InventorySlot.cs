using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI countText;

    public bool alwaysShowCount = false; // 可在 Inspector 中控制

public void SetSlot(Item item)
{
     Debug.Log("SetSlot called");
    iconImage.sprite = item.itemIcon;
    iconImage.enabled = true;

    countText.text = item.stackSize.ToString();

    // 保險起見手動啟用
    countText.gameObject.SetActive(true);
    countText.color = new Color(0, 0, 0, 1); // 黑色，完全不透明
    countText.fontSize = 36;
}



public void ClearSlot()
{
    Debug.Log("ClearSlot called");
    iconImage.sprite = null;          // 沒有圖片
    iconImage.enabled = false;        // 🔥 直接隱藏這個 Image 組件
    countText.text = "";              // 清掉數字
}

}
