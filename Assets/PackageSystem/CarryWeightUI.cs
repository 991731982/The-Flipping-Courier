using UnityEngine;
using UnityEngine.UI;

public class CarryWeightUI : MonoBehaviour
{
    public Inventory inventory;
    public Slider weightSlider;
    public Image fillImage;

    public float maxCarryWeight = 100f; // ✅ 玩家最大可承重

    void OnEnable()
    {
        Inventory.OnInventoryChange += UpdateWeightBar;
    }

    void OnDisable()
    {
        Inventory.OnInventoryChange -= UpdateWeightBar;
    }

    void Start()
    {
        UpdateWeightBar();
    }

    void UpdateWeightBar()
    {
        float totalWeight = 0f;

        foreach (Item item in inventory.GetInventoryList())
        {
            totalWeight += item.weight * item.stackSize;
        }

        float percent = totalWeight / maxCarryWeight;
        weightSlider.value = percent;

        // 設定顏色區間
        if (percent <= 0.6f)
            fillImage.color = Color.green;
        else if (percent <= 0.8f)
            fillImage.color = Color.yellow;
        else if (percent <= 0.95f)
            fillImage.color = new Color(1f, 0.65f, 0f); // 橙色
        else
            fillImage.color = Color.red;
    }
}
