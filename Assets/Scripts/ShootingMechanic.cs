using UnityEngine;

public class ShootingMechanic : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;  // 圆球的预制体
    public GameObject supplyBulletPrefab;
    public GameObject EnemyBulletPrefab;
    public GameObject healBulletPrefab; // 恢复子弹的预制体
    public float projectileSpeed = 20f; // 圆球飞行速度
    public Transform shootPoint;        // 发射点
    public int maxAmmo = 30;            // 最大备弹量
    private int currentAmmo;            // 当前剩余子弹量

    [Header("Crosshair Settings")]
    public Texture2D crosshairTexture;  // 准星贴图
    public float crosshairSize = 32f;   // 准星大小

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
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0)) // 左键发射普通子弹
        {
            if (currentAmmo > 0)
            {
                ShootProjectile(projectilePrefab); // 发射普通子弹
            }
            else
            {
                Debug.Log("Out of ammo!");
            }
        }

        if (Input.GetMouseButtonDown(1)) // 右键发射补给子弹
        {
            ShootProjectile(EnemyBulletPrefab); // 发射补给子弹
        }
    }

    private void ShootProjectile(GameObject bulletPrefab)
    {
        if (bulletPrefab == null || shootPoint == null)
        {
            Debug.LogWarning("Projectile Prefab or Shoot Point is missing!");
            return;
        }

        // 创建子弹
        GameObject projectile = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = -transform.forward * projectileSpeed;
        }

        // 减少弹药（仅普通子弹减少）
        if (bulletPrefab == projectilePrefab)
        {
            currentAmmo--;
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

        // 显示当前剩余子弹数量
        GUIStyle ammoStyle = new GUIStyle
        {
            fontSize = 24,
            normal = { textColor = Color.white }
        };
        GUI.Label(new Rect(10, 10, 200, 50), $"Ammo: {currentAmmo}/{maxAmmo}", ammoStyle);
    }

    private void OnDrawGizmos()
    {
        if (shootPoint != null)
        {
            // 在 Scene 视图中绘制发射方向
            Gizmos.color = Color.red;
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + -transform.forward * 10f); // 绘制方向线
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;

        // 确保弹药量不会超过最大值
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        Debug.Log($"Ammo replenished! Current Ammo: {currentAmmo}/{maxAmmo}");
    }
}
