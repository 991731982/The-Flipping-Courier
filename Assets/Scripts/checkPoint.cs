using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    public Color activatedColor = Color.green; // Checkpoint激活后的颜色
    public AudioClip checkpointSound; // Checkpoint激活时的音效
    public GameObject uiNotification; // 用于显示UI提示的对象

    private Renderer checkpointRenderer;
    private AudioSource audioSource;
    private bool isActivated = false; // 防止重复触发

    private void Start()
    {
        // 获取Renderer和AudioSource组件
        checkpointRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        // 如果UI对象已设置，则默认隐藏
        if (uiNotification != null)
        {
            uiNotification.SetActive(false);
            Debug.Log("UI Notification is set and hidden at the start.");
        }
        else
        {
            Debug.LogWarning("UI Notification is not assigned!");
        }

        // 检查是否存在 AudioSource
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the CheckPoint object!");
        }
        else
        {
            Debug.Log("AudioSource component found on CheckPoint object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查是否是玩家并且Checkpoint尚未被激活
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true; // 设置激活状态
            checkPointRespawn player = other.GetComponent<checkPointRespawn>();

            if (player != null)
            {
                player.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint reached at position: " + transform.position);

                // 重置所有可重置的物体
                ResetAllObjects();

                // 显示提示UI
                ShowNotification();

                // 改变物品颜色
                ChangeCheckpointColor();

                // 播放音效
                PlayCheckpointSound();
            }
            else
            {
                Debug.LogWarning("Player object does not have a checkPointRespawn component!");
            }
        }
    }

    private void ResetAllObjects()
    {
        Debug.Log("Resetting all objects to their original positions...");
        ResettableObject[] resettableObjects = FindObjectsOfType<ResettableObject>();
        foreach (ResettableObject obj in resettableObjects)
        {
            obj.ResetPosition();
            Debug.Log("Object reset: " + obj.gameObject.name);
        }
    }

    private void ShowNotification()
    {
        if (uiNotification != null)
        {
            uiNotification.SetActive(true); // 显示UI提示
            Debug.Log("UI Notification is displayed.");
            Invoke("HideNotification", 2f); // 2秒后隐藏UI
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
            Debug.Log("UI Notification is hidden.");
        }
    }

    private void ChangeCheckpointColor()
    {
        if (checkpointRenderer != null)
        {
            checkpointRenderer.material.color = activatedColor; // 设置为激活颜色
            Debug.Log("Checkpoint color changed to activatedColor.");
        }
        else
        {
            Debug.LogWarning("Renderer component is missing on the CheckPoint object!");
        }
    }

    private void PlayCheckpointSound()
    {
        if (checkpointSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(checkpointSound); // 播放音效
            Debug.Log("Checkpoint sound played!");
        }
        else
        {
            if (checkpointSound == null)
                Debug.LogWarning("AudioClip is missing! Please assign a valid AudioClip.");
            if (audioSource == null)
                Debug.LogWarning("AudioSource is missing! Please ensure the object has an AudioSource component.");
        }
    }
}
