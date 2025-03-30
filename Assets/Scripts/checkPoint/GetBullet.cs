using UnityEngine;
using UnityEngine.UI;

public class GetBullet : MonoBehaviour
{
    public int ammoToAdd = 10;          // 每次进入检查点增加的弹药量
    public AudioClip pickupSound;       // 碰撞时播放的音效
    public GameObject uiNotification;   // 显示提示的UI对象

    private AudioSource audioSource;    // 音效组件
    private bool isCollected = false;   // 防止重复触发

    private void Start()
    {
        // 初始化 AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the GetBullet object!");
        }

        // 隐藏 UI Notification
        if (uiNotification != null)
        {
            uiNotification.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter called by {other.name}");
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true; // 标记为已触发

            // 获取玩家的 ShootingMechanic 脚本
            ShootingMechanic shootingMechanic = other.GetComponent<ShootingMechanic>();

            if (shootingMechanic != null)
            {
                shootingMechanic.AddAmmo(ammoToAdd); // 增加弹药
                Debug.Log($"Player picked up ammo. Ammo increased by {ammoToAdd}!");
            }
            else
            {
                Debug.LogWarning("Player does not have a ShootingMechanic script attached!");
            }

            PlayPickupSound();    // 播放音效
            ShowNotification();  // 显示 UI 提示

            Destroy(gameObject, 1f); // 延迟销毁
        }
    }

    private void PlayPickupSound()
    {
        if (pickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound); // 播放音效
        }
    }

    private void ShowNotification()
    {
        if (uiNotification != null)
        {
            uiNotification.SetActive(true);
            Invoke("HideNotification", 2f); // 2秒后隐藏 UI
        }
    }

    private void HideNotification()
    {
        if (uiNotification != null)
        {
            uiNotification.SetActive(false);
        }
    }
}
