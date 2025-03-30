using UnityEngine;
using TMPro; // 引入 TextMeshPro 的命名空间

public class ShootingMechanic : MonoBehaviour
{
    [Header("Ammo Settings")]
    public int maxAmmo = 99;            // 最大备弹量
    private int currentAmmo;            // 当前剩余子弹量

    [Header("UI Settings")]
    public TextMeshProUGUI ammoText;    // TMP 文本，用于显示弹药数量

    void Start()
    {
        currentAmmo = 10; // 初始化弹药数量
        UpdateAmmoText(); // 初始化显示弹药数量
    }

    public void AddAmmo(int amount)
    {
        int previousAmmo = currentAmmo; // 记录修改前的值
        currentAmmo += amount;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo); // 确保弹药量在合理范围

        Debug.Log($"Ammo changed: {previousAmmo} -> {currentAmmo} (Max: {maxAmmo})");
        UpdateAmmoText(); // 更新 UI
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo}/{maxAmmo}";
        }
        else
        {
            Debug.LogWarning("Ammo Text (TMP) is not assigned!");
        }
    }
}
