using UnityEngine;

public class ShootingMechanic : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;  // 圆球的预制体
    public float projectileSpeed = 20f; // 圆球飞行速度
    public Transform shootPoint;        // 发射点

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
    }

    void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0)) // 左键按下时射击
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null || shootPoint == null)
        {
            Debug.LogWarning("Projectile Prefab or Shoot Point is missing!");
            return;
        }

        // 获取角色正前方方向
        Vector3 shootDirection = -transform.forward;

        // 创建圆球并设置其方向和速度
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = shootDirection * projectileSpeed;
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

    private void OnDrawGizmos()
    {
        if (shootPoint != null)
        {
            // 在 Scene 视图中绘制发射方向
            Gizmos.color = Color.red;
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + -transform.forward * 10f); // 绘制方向线
        }
    }
}
