using UnityEngine;
using UnityEngine.UI;

public class GetBullet : MonoBehaviour
{
    public int ammoToAdd = 10; // 每次进入检查点增加的弹药量
    public AudioClip pickupSound; // 碰撞时播放的音效
    public GameObject uiNotification; // 显示提示的UI对象

    private AudioSource audioSource; // 音效组件
    private bool isCollected = false; // 防止重复触发

    private void Start()
    {
        // 初始化 AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the GetBullet object!");
        }
        else
        {
            Debug.Log("AudioSource component found on GetBullet object.");
        }

        // 隐藏 UI Notification
        if (uiNotification != null)
        {
            uiNotification.SetActive(false); // 确保 UI 默认隐藏
            Debug.Log("UI Notification is set and hidden at the start.");
        }
        else
        {
            Debug.LogWarning("UI Notification is not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter called by {other.name}"); // 日志输出
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true; // 设置为已触发状态

            // 获取玩家的 ShootingMechanic 脚本
            ShootingMechanic shootingMechanic = other.GetComponent<ShootingMechanic>();

            if (shootingMechanic != null)
            {
                // 增加玩家弹药
                shootingMechanic.AddAmmo(ammoToAdd);
                Debug.Log($"Player picked up ammo. Ammo increased by {ammoToAdd}!");
            }
            else
            {
                Debug.LogWarning("Player does not have a ShootingMechanic script attached!");
            }

            // 播放音效
            PlayPickupSound();

            // 显示 UI 提示
            ShowNotification();

            // 销毁对象（如果需要）
            Destroy(gameObject, 1f); // 延迟1秒销毁，给音效和UI足够时间
        }
    }

    private void PlayPickupSound()
    {
        Debug.Log("PlayPickupSound() called."); // 调试日志
        if (pickupSound != null && audioSource != null)
        {
            Debug.Log("Playing pickup sound through PlayOneShot().");
            audioSource.PlayOneShot(pickupSound); // 播放音效
        }
        else
        {
            if (pickupSound == null)
                Debug.LogWarning("Pickup sound clip is missing!");
            if (audioSource == null)
                Debug.LogWarning("AudioSource is missing!");
        }
    }

    private void ShowNotification()
    {
        if (uiNotification != null)
        {
            uiNotification.SetActive(true); // 显示 UI 提示
            Debug.Log("UI Notification displayed.");
            Invoke("HideNotification", 2f); // 2秒后隐藏 UI
        }
        else
        {
            Debug.LogWarning("UI Notification GameObject is not assigned!");
        }
    }

    private void HideNotification()
    {
        if (uiNotification != null)
        {
            uiNotification.SetActive(false);
            Debug.Log("UI Notification hidden.");
        }
    }
}
