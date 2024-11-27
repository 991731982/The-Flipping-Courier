using UnityEngine;

public class CameraCheckPoint : MonoBehaviour
{
    public float newOffsetZ = -15.0f; // 新的 Z 轴偏移
    public float newYPosition = 10.0f; // 新的 Y 轴位置
    private CameraFollow cameraFollow; // 引用 CameraFollow 脚本

    void Start()
    {
        // 获取场景中的 CameraFollow 脚本
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 检查是否是玩家触发
        {
            if (cameraFollow != null)
            {
                // 更新摄像机的 Z 偏移和 Y 轴位置
                cameraFollow.UpdateCameraOffset(newOffsetZ);
                cameraFollow.fixedYPosition = newYPosition;
                Debug.Log($"Checkpoint Triggered: Z = {newOffsetZ}, Y = {newYPosition}");
            }
        }
    }
}
