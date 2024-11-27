using UnityEngine;

public class CameraCheckPoint : MonoBehaviour
{
    public float newOffsetZ = -15.0f; // 新的 Z 偏移
    public float newYPosition = 10.0f; // 新的 Y 轴高度
    private CameraFollow cameraFollow; // 引用 CameraFollow 脚本

    void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 更新摄像机的 Z 偏移和 Y 轴高度
            cameraFollow.UpdateCameraOffset(newOffsetZ);
            cameraFollow.fixedYPosition = newYPosition;
            Debug.Log($"Checkpoint Triggered: Z = {newOffsetZ}, Y = {newYPosition}");
        }
    }
}
