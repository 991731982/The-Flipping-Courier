using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera will follow (CubeCharacter)
    public Vector3 offset; // Offset from the target's position
    public float smoothSpeed = 0.125f; // Smoothing speed for the camera movement

    void LateUpdate()
    {
        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the smoothed position to the camera
        transform.position = smoothedPosition;

        // Optional: Make the camera always look at the target
        // transform.LookAt(target);
    }
}