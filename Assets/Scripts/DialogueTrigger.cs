using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject uiImage; // 要显示或隐藏的图片 UI 对象
    public AudioClip triggerSound; // 要播放的音效
    public AudioSource audioSource; // 音效的音频源
    private bool isDisplayed = false; // 当前 UI 是否已显示
    private float displayTime = 5f; // UI 显示的持续时间
    private float timer = 0f; // 计时器

    private void Start()
    {
        // 确保 UI 初始状态为隐藏
        if (uiImage != null)
        {
            uiImage.SetActive(false); // 隐藏 UI
            Debug.Log("UI is hidden at the start.");
        }
        else
        {
            Debug.LogWarning("UI Image is not assigned!");
        }

        // 确保音频源已分配
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is not assigned! Ensure it's added and configured.");
        }
        if (triggerSound == null)
        {
            Debug.LogWarning("TriggerSound is not assigned! Ensure an AudioClip is added.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Object entered the trigger: {other.name}"); // 输出触发对象名称

        // 检查是否是玩家触发
        if (other.CompareTag("Player") && !isDisplayed)
        {
            Debug.Log("Player detected entering the trigger."); // 玩家进入触发器日志
            if (uiImage != null)
            {
                // 显示图片 UI
                uiImage.SetActive(true);
                isDisplayed = true;
                timer = 0f; // 重置计时器
                Debug.Log("UI is displayed.");
            }

            // 播放音效
            if (audioSource != null && triggerSound != null)
            {
                audioSource.PlayOneShot(triggerSound);
                Debug.Log($"Trigger sound '{triggerSound.name}' played.");
            }
            else
            {
                Debug.LogWarning("AudioSource or TriggerSound is missing!");
            }
        }
        else
        {
            Debug.Log($"Non-player object entered the trigger: {other.name}"); // 非玩家触发调试信息
        }
    }

    private void Update()
    {
        // 如果 UI 已显示，则开始计时
        if (isDisplayed)
        {
            timer += Time.deltaTime; // 增加计时器
            if (timer >= displayTime)
            {
                // 时间到了，隐藏 UI
                if (uiImage != null)
                {
                    uiImage.SetActive(false);
                    isDisplayed = false;
                    Debug.Log("UI is hidden after timeout.");
                }
            }
        }
    }
}
