using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    public Color activatedColor = Color.green; // Checkpoint��������ɫ
    public AudioClip checkpointSound; // Checkpoint����ʱ����Ч
    public GameObject uiNotification; // ������ʾUI��ʾ�Ķ���

    private Renderer checkpointRenderer;
    private AudioSource audioSource;
    private bool isActivated = false; // ��ֹ�ظ�����

    private void Start()
    {
        // ��ȡRenderer��AudioSource���
        checkpointRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        // ���UI���������ã���Ĭ������
        if (uiNotification != null)
        {
            uiNotification.SetActive(false);
            Debug.Log("UI Notification is set and hidden at the start.");
        }
        else
        {
            Debug.LogWarning("UI Notification is not assigned!");
        }

        // ����Ƿ���� AudioSource
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
        // ����Ƿ�����Ҳ���Checkpoint��δ������
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true; // ���ü���״̬
            checkPointRespawn player = other.GetComponent<checkPointRespawn>();

            if (player != null)
            {
                player.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint reached at position: " + transform.position);

                // �������п����õ�����
                ResetAllObjects();

                // ��ʾ��ʾUI
                ShowNotification();

                // �ı���Ʒ��ɫ
                ChangeCheckpointColor();

                // ������Ч
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
            uiNotification.SetActive(true); // ��ʾUI��ʾ
            Debug.Log("UI Notification is displayed.");
            Invoke("HideNotification", 2f); // 2�������UI
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
            checkpointRenderer.material.color = activatedColor; // ����Ϊ������ɫ
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
            audioSource.PlayOneShot(checkpointSound); // ������Ч
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
