using UnityEngine;
using TMPro; // 引入 TextMeshPro 的命名空间

public class ShootingMechanic : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;  // 普通子弹预制体
    public GameObject supplyBulletPrefab; // 补给子弹预制体
    public GameObject healBulletPrefab; // 恢复子弹预制体
    public GameObject spawnEnemyBulletPrefab; // 生成敌人的子弹预制体
    public float projectileSpeed = 20f; // 子弹飞行速度
    public Transform shootPoint;        // 发射点
    public int maxAmmo = 99;            // 最大备弹量
    private int currentAmmo;            // 当前剩余子弹量

    private int specialBulletIndex = 0; // 右键轮流发射的子弹索引

    [Header("UI Settings")]
    public Texture2D crosshairTexture;  // 准星贴图
    public float crosshairSize = 32f;   // 准星大小
    public TextMeshProUGUI ammoText;    // TMP 文本，用于显示弹药数量

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }

        if (shootPoint == null)
        {
            Debug.LogError("Shoot point not assigned!");
        }

        // 初始化子弹数量
        currentAmmo = 10;
        UpdateAmmoText(); // 初始化显示弹药数量
    }

    void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        // 左键发射普通子弹
        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0)
            {
                ShootProjectile(projectilePrefab);
                UpdateAmmoText(); // 更新弹药数量显示
            }
            else
            {
                Debug.Log("Out of ammo!");
            }
        }

        // 右键轮流发射三种特殊子弹
        if (Input.GetMouseButtonDown(1))
        {
            FireSpecialBullet();
        }
    }

    private void FireSpecialBullet()
    {
        // 根据索引选择特殊子弹
        GameObject selectedBullet = null;

        switch (specialBulletIndex)
        {
            case 0:
                selectedBullet = supplyBulletPrefab; // 补给子弹
                break;
            case 1:
                selectedBullet = healBulletPrefab; // 恢复子弹
                break;
            case 2:
                selectedBullet = spawnEnemyBulletPrefab; // 生成敌人的子弹
                break;
        }

        if (selectedBullet != null)
        {
            ShootProjectile(selectedBullet);
        }

        // 循环切换索引
        specialBulletIndex = (specialBulletIndex + 1) % 3;
    }

    private void ShootProjectile(GameObject bulletPrefab)
    {
        if (bulletPrefab == null || shootPoint == null)
        {
            Debug.LogWarning("Bullet prefab or shoot point is not assigned!");
            return;
        }

        // 实例化子弹
        Vector3 offset = shootPoint.forward * 0.5f; // 偏移量，避免子弹生成在玩家内部
        GameObject projectile = Instantiate(bulletPrefab, shootPoint.position + offset, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootPoint.forward * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("No Rigidbody attached to the projectile!");
        }

        // 减少普通子弹的弹药（特殊子弹不消耗）
        if (bulletPrefab == projectilePrefab)
        {
            currentAmmo--;
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo); // 确保弹药量在合理范围
        UpdateAmmoText(); // 更新弹药数量显示
        Debug.Log($"Ammo replenished! Current Ammo: {currentAmmo}/{maxAmmo}");
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

    private void OnGUI()
    {
        // 显示准星
        if (crosshairTexture != null)
        {
            float xMin = (Screen.width - crosshairSize) / 2;
            float yMin = (Screen.height - crosshairSize) / 2;
            GUI.DrawTexture(new Rect(xMin, yMin, crosshairSize, crosshairSize), crosshairTexture);
        }
    }
}
