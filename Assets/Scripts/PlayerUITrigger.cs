using UnityEngine;

public class PlayerUITrigger : MonoBehaviour
{
    public GameObject uiElement; // ��Ҫ��ʾ/���ص� UI Ԫ��

    void Start()
    {
        // ȷ�� UI Ԫ�س�ʼ״̬Ϊ����
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
    }

    // ����ҽ��봥��������ʱ����ʾ UI
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UITrigger")) // ȷ����������������ȷ�ı�ǩ
        {
            if (uiElement != null)
            {
                uiElement.SetActive(true);
            }
        }
    }

    // ������뿪����������ʱ������ UI
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UITrigger"))
        {
            if (uiElement != null)
            {
                uiElement.SetActive(false);
            }
        }
    }
}
