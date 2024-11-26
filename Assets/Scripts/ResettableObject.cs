using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    void Start()
    {
        // ��¼��ʼλ�ú���ת
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // ��ȡ�������������У�
        rb = GetComponent<Rigidbody>();
    }

    // ��������λ�ú���ת
    public void ResetPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // ���ø����ٶ�
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
