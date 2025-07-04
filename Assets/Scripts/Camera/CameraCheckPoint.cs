using UnityEngine;

public class CameraCheckPoint : MonoBehaviour
{
    public float newOrthographicSize = 10.0f; // For orthographic camera
    //public float newOffsetZ = -15.0f;         // For perspective camera
    public float newYPosition = 10.0f;        // Fixed Y position

    private Camera mainCamera;
    private CameraFollow cameraFollow;

    void Start()
    {
        mainCamera = Camera.main;
        cameraFollow = mainCamera.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (mainCamera.orthographic)
            {
                // Update orthographic size
                mainCamera.orthographicSize = newOrthographicSize;
                Debug.Log($"[Checkpoint] Set Orthographic Size = {newOrthographicSize}");
            }
            /*else
            {
                // Update Z offset in CameraFollow if perspective
                if (cameraFollow != null)
                {
                    cameraFollow.UpdateCameraOffset(newOffsetZ);
                    Debug.Log($"[Checkpoint] Set Perspective Offset Z = {newOffsetZ}");
                }
            }*/

            // Always update Y position if CameraFollow is available
            if (cameraFollow != null)
            {
                cameraFollow.fixedYPosition = newYPosition;
                Debug.Log($"[Checkpoint] Set Y Position = {newYPosition}");
            }
        }
    }
}