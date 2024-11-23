using UnityEngine;
using TMPro; // 引入 TextMeshPro 的命名空间

public class PlayerHealthDisplay : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // 最大血量
    private int currentHealth;  // 当前血量

    [Header("UI Settings")]
    public TextMeshProUGUI healthText; // 用于显示血量的 TMP 文本对象

    void Start()
    {
        // 初始化血量
        currentHealth = maxHealth;

        // 更新血量显示
        UpdateHealthDisplay();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 确保血量在 0 和 maxHealth 范围内
        UpdateHealthDisplay();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 确保血量在 0 和 maxHealth 范围内
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        }
        else
        {
            Debug.LogWarning("Health Text (TMP) is not assigned!");
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // 在这里处理玩家死亡逻辑
    }
}
