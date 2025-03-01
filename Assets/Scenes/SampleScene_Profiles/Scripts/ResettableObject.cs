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

    // 重置物体位置和旋转（忽略带有 Enemy 标签的物体）
    public void ResetPosition()
    {
        // 检查是否为 Enemy 标签
        if (CompareTag("Enemy"))
        {
            // 如果是 Enemy，直接返回，不进行重置
            return;
        }

        // 重置位置和旋转
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
