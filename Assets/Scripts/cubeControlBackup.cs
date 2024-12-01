//using UnityEngine;
//using TMPro; // ���� TextMeshPro �������ռ�

//public class ShootingMechanic : MonoBehaviour
//{
//    [Header("Shooting Settings")]
//    public GameObject projectilePrefab;  // ��ͨ�ӵ�Ԥ����
//    public GameObject supplyBulletPrefab; // �����ӵ�Ԥ����
//    public GameObject healBulletPrefab; // �ָ��ӵ�Ԥ����
//    public GameObject spawnEnemyBulletPrefab; // ���ɵ��˵��ӵ�Ԥ����
//    public float projectileSpeed = 20f; // �ӵ������ٶ�
//    public Transform shootPoint;        // �����
//    public int maxAmmo = 99;            // ��󱸵���
//    private int currentAmmo;            // ��ǰʣ���ӵ���

//    private int specialBulletIndex = 0; // �Ҽ�����������ӵ�����

//    [Header("UI Settings")]
//    public Texture2D crosshairTexture;  // ׼����ͼ
//    public float crosshairSize = 32f;   // ׼�Ǵ�С
//    public TextMeshProUGUI ammoText;    // TMP �ı���������ʾ��ҩ����

//    private Camera mainCamera;

//    void Start()
//    {
//        mainCamera = Camera.main;
//        if (mainCamera == null)
//        {
//            Debug.LogError("Main Camera not found!");
//        }

//        if (shootPoint == null)
//        {
//            Debug.LogError("Shoot point not assigned!");
//        }

//        // ��ʼ���ӵ�����
//        currentAmmo = 10;
//        UpdateAmmoText(); // ��ʼ����ʾ��ҩ����
//    }

//    void Update()
//    {
//        HandleShooting();
//    }

//    private void HandleShooting()
//    {
//        // ���������ͨ�ӵ�
//        if (Input.GetMouseButtonDown(0))
//        {
//            if (currentAmmo > 0)
//            {
//                ShootProjectile(projectilePrefab);
//                UpdateAmmoText(); // ���µ�ҩ������ʾ
//            }
//            else
//            {
//                Debug.Log("Out of ammo!");
//            }
//        }

//        // �Ҽ������������������ӵ�
//        if (Input.GetMouseButtonDown(1))
//        {
//            FireSpecialBullet();
//        }
//    }

//    private void FireSpecialBullet()
//    {
//        // ��������ѡ�������ӵ�
//        GameObject selectedBullet = null;

//        switch (specialBulletIndex)
//        {
//            case 0:
//                selectedBullet = supplyBulletPrefab; // �����ӵ�
//                break;
//            case 1:
//                selectedBullet = healBulletPrefab; // �ָ��ӵ�
//                break;
//            case 2:
//                selectedBullet = spawnEnemyBulletPrefab; // ���ɵ��˵��ӵ�
//                break;
//        }

//        if (selectedBullet != null)
//        {
//            ShootProjectile(selectedBullet);
//        }

//        // ѭ���л�����
//        specialBulletIndex = (specialBulletIndex + 1) % 3;
//    }

//    private void ShootProjectile(GameObject bulletPrefab)
//    {
//        if (bulletPrefab == null || shootPoint == null)
//        {
//            Debug.LogWarning("Bullet prefab or shoot point is not assigned!");
//            return;
//        }

//        // ʵ�����ӵ�
//        Vector3 offset = shootPoint.forward * 0.5f; // ƫ�����������ӵ�����������ڲ�
//        GameObject projectile = Instantiate(bulletPrefab, shootPoint.position + offset, Quaternion.identity);

//        Rigidbody rb = projectile.GetComponent<Rigidbody>();
//        if (rb != null)
//        {
//            rb.velocity = shootPoint.forward * projectileSpeed;
//        }
//        else
//        {
//            Debug.LogWarning("No Rigidbody attached to the projectile!");
//        }

//        // ������ͨ�ӵ��ĵ�ҩ�������ӵ������ģ�
//        if (bulletPrefab == projectilePrefab)
//        {
//            currentAmmo--;
//        }
//    }

//    public void AddAmmo(int amount)
//    {
//        currentAmmo += amount;
//        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo); // ȷ����ҩ���ں���Χ
//        UpdateAmmoText(); // ���µ�ҩ������ʾ
//        Debug.Log($"Ammo replenished! Current Ammo: {currentAmmo}/{maxAmmo}");
//    }

//    private void UpdateAmmoText()
//    {
//        if (ammoText != null)
//        {
//            ammoText.text = $"{currentAmmo}/{maxAmmo}";
//        }
//        else
//        {
//            Debug.LogWarning("Ammo Text (TMP) is not assigned!");
//        }
//    }

//    private void OnGUI()
//    {
//        // ��ʾ׼��
//        if (crosshairTexture != null)
//        {
//            float xMin = (Screen.width - crosshairSize) / 2;
//            float yMin = (Screen.height - crosshairSize) / 2;
//            GUI.DrawTexture(new Rect(xMin, yMin, crosshairSize, crosshairSize), crosshairTexture);
//        }
//    }
//}
