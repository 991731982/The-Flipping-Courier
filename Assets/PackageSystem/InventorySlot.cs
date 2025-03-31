using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI countText;

    private Item currentItem;

    public void SetSlot(Item item)
    {
        currentItem = item;

        iconImage.sprite = item.itemIcon;
        iconImage.enabled = true;
        countText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }

    public void ClearSlot()
    {
        currentItem = null;

        iconImage.sprite = null;
        iconImage.enabled = false;
        countText.text = "";
    }

    public Item GetItem()
    {
        return currentItem;
    }

    public void SetSelected(bool selected)
    {
        transform.localScale = selected ? Vector3.one * 1.2f : Vector3.one;
    }
}
