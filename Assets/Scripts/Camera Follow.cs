using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 跟随的目标
    public float fixedYPosition = 5.0f; // 初始 Y 轴高度，可动态更新
    public float smoothSpeed = 0.125f; // 摄像机整体移动的平滑速度
    public float yTransitionSpeed = 0.5f; // Y 轴过渡的速度（可调）
    public float offsetX = 0.0f; // X 轴偏移量
    [SerializeField] private float offsetZ = -10.0f; // 初始 Z 偏移量，可动态更新

    private float currentYPosition; // 当前 Y 轴位置
    private float currentOffsetZ; // 当前 Z 偏移量
    private float targetOffsetZ; // 目标 Z 偏移量
    private float yVelocity = 0.0f; // 用于 SmoothDamp 的速度变量
    public float offsetTransitionSpeed = 5.0f; // Z 偏移过渡速度

    void Start()
    {
        // 初始化 Y 轴位置和 Z 偏移
        currentYPosition = fixedYPosition;
        currentOffsetZ = offsetZ; // 将初始 Z 偏移赋值为当前值
        targetOffsetZ = offsetZ; // 初始化目标 Z 偏移
    }

    void LateUpdate()
    {
        // 每帧同步 Inspector 中的 offsetZ 到目标值
        targetOffsetZ = offsetZ;

        // 平滑过渡 Z 轴偏移量到目标值
        currentOffsetZ = Mathf.Lerp(currentOffsetZ, targetOffsetZ, Time.deltaTime * offsetTransitionSpeed);

        // 平滑过渡 Y 轴高度到目标值
        currentYPosition = Mathf.SmoothDamp(currentYPosition, fixedYPosition, ref yVelocity, 1f / yTransitionSpeed);

        // 计算摄像机目标位置
        Vector3 desiredPosition = new Vector3(
            target.position.x + offsetX, // 跟随目标的 X 轴
            currentYPosition,            // 平滑过渡的 Y 轴高度
            target.position.z + currentOffsetZ // 平滑过渡的 Z 偏移量
        );

        // 平滑移动摄像机到目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 保持摄像机面朝 +Z 方向
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }

    // 更新摄像机目标 Z 偏移的方法
    public void UpdateCameraOffset(float newOffsetZ)
    {
        offsetZ = newOffsetZ; // 动态更新 offsetZ
    }
}
