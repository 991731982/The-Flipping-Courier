using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 跟随的目标
    public float mouseSensitivity = 500f; // 鼠标灵敏度
    public float pitchMin = -45f; // 最低垂直视角（向下）
    public float pitchMax = 80f; // 最高垂直视角（向上）
    public float distanceFromTarget = 10.0f; // 摄像机与目标的距离
    public float smoothSpeed = 0.1f; // 平滑跟随速度
    public float verticalOffset = 1.8f; // 上下偏移量

    private float yaw = 0f; // 水平方向旋转角度
    private float pitch = 0f; // 垂直方向旋转角度
    private Vector3 currentVelocity; // 平滑移动的速度

    void Start()
    {
        // 锁定鼠标到屏幕中央并隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 初始化摄像机旋转
        if (target != null)
        {
            yaw = target.eulerAngles.y;
            pitch = target.eulerAngles.x;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.unscaledDeltaTime;

        // 更新旋转角度
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // 计算目标位置
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distanceFromTarget);
        desiredPosition.y += verticalOffset;

        // 平滑移动摄像机
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed * Time.unscaledDeltaTime);

        // 始终看向目标
        transform.LookAt(target.position + Vector3.up * verticalOffset);
    }
}
