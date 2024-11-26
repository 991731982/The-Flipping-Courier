using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    void Start()
    {
        // 记录初始位置和旋转
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // 获取刚体组件（如果有）
        rb = GetComponent<Rigidbody>();
    }

    // 重置物体位置和旋转
    public void ResetPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // 重置刚体速度
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
