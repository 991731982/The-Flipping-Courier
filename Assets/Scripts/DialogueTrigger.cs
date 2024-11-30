using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject uiImage; // Ҫ��ʾ�����ص�ͼƬ UI ����
    public AudioClip triggerSound; // Ҫ���ŵ���Ч
    public AudioSource audioSource; // ��Ч����ƵԴ
    private bool isDisplayed = false; // ��ǰ UI �Ƿ�����ʾ
    private float displayTime = 5f; // UI ��ʾ�ĳ���ʱ��
    private float timer = 0f; // ��ʱ��

    private void Start()
    {
        // ȷ�� UI ��ʼ״̬Ϊ����
        if (uiImage != null)
        {
            uiImage.SetActive(false); // ���� UI
            Debug.Log("UI is hidden at the start.");
        }
        else
        {
            Debug.LogWarning("UI Image is not assigned!");
        }

        // ȷ����ƵԴ�ѷ���
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
        Debug.Log($"Object entered the trigger: {other.name}"); // ���������������

        // ����Ƿ�����Ҵ���
        if (other.CompareTag("Player") && !isDisplayed)
        {
            Debug.Log("Player detected entering the trigger."); // ��ҽ��봥������־
            if (uiImage != null)
            {
                // ��ʾͼƬ UI
                uiImage.SetActive(true);
                isDisplayed = true;
                timer = 0f; // ���ü�ʱ��
                Debug.Log("UI is displayed.");
            }

            // ������Ч
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
            Debug.Log($"Non-player object entered the trigger: {other.name}"); // ����Ҵ���������Ϣ
        }
    }

    private void Update()
    {
        // ��� UI ����ʾ����ʼ��ʱ
        if (isDisplayed)
        {
            timer += Time.deltaTime; // ���Ӽ�ʱ��
            if (timer >= displayTime)
            {
                // ʱ�䵽�ˣ����� UI
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
