using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    void Start()
    {
        // Record the original position and rotation only once at the beginning of the scene
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Get the Rigidbody component if it exists
        rb = GetComponent<Rigidbody>();
    }

    // Reset the object to its original position, rotation, and clear any velocity
    public void ResetPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // If there's a Rigidbody, reset its velocity and angular velocity
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
