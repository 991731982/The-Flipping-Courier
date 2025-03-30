using System.Diagnostics;
using UnityEngine;

public class HeavyBoxTrigger : MonoBehaviour
{
    public float massThreshold = 400f; // �O���|���T��������@��ֵ���|�l�½�
    public float dropDistance = 10f; // �½����x
    public float dropSpeed = 5f; // �½��ٶȣ����{�����O�� 0 ׃˲�g�½���

    private bool shouldDrop = false;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position + Vector3.down * dropDistance; // Ӌ���½����λ��
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;

        // �z���Ƿ��� Box���K�����|���Ƿ��^�T��
        if (collision.gameObject.CompareTag("Box") && rb != null && rb.mass >= massThreshold)
        {
            UnityEngine.Debug.Log("���� Box ײ������ɫ�����_ʼ�½���");
            shouldDrop = true;
        }
    }

    void Update()
    {
        if (shouldDrop)
        {
            // ׌��ɫ���Ӿ����½�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dropSpeed * Time.deltaTime);

            // �z���Ƿ��_Ŀ��λ��
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                shouldDrop = false; // ֹͣ�Ƅ�
            }
        }
    }
}
