using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthDisplay : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // 最大血量
    private int currentHealth = 50;  // 当前血量，初始化为 50

    [Header("UI Settings")]
    public Slider healthBar; // 用于显示血量的 Slider（生命值条）
    public TextMeshProUGUI healthText; // TMP 文本，用于显示数字生命值
    public Image fillImage;  // Slider 的填充图像

    [Header("Animation Settings")]
    public float animationSpeed = 10f; // 血量条动画速度

    private float targetHealthPercentage; // 目标血量百分比

    void Start()
    {
        // 初始化血量并更新显示
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetHealthPercentage = (float)currentHealth / maxHealth;
        UpdateHealthDisplay(true); // 初始化时直接设置值，不需要动画
    }

    void Update()
    {
        // 平滑更新血量条
        if (healthBar != null)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, targetHealthPercentage, Time.deltaTime * animationSpeed);

            // 更新颜色渐变
            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(Color.red, Color.green, healthBar.value);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 确保血量在范围内
        targetHealthPercentage = (float)currentHealth / maxHealth; // 更新目标血量百分比
        UpdateHealthDisplay();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 确保血量在范围内
        targetHealthPercentage = (float)currentHealth / maxHealth; // 更新目标血量百分比
        UpdateHealthDisplay();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void UpdateHealthDisplay(bool instantUpdate = false)
    {
        // 如果需要立即更新，不通过动画
        if (instantUpdate && healthBar != null)
        {
            healthBar.value = targetHealthPercentage;

            // 更新颜色渐变
            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(Color.red, Color.green, healthBar.value);
            }
        }

        // 更新 TMP 数字显示
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // 在这里处理玩家死亡逻辑
    }

    public void RestoreFullHealth()
    {
        currentHealth = maxHealth; // 将血量恢复到最大
        targetHealthPercentage = (float)currentHealth / maxHealth; // 更新目标百分比
        UpdateHealthDisplay(true); // 立即更新 UI
    }
}
