using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player reached the checkpoint
        if (other.CompareTag("Player"))
        {
            checkPointRespawn player = other.GetComponent<checkPointRespawn>();
            if (player != null)
            {
                player.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint reached at position: " + transform.position);

                // Call the method to reset all resettable objects
                ResetAllObjects();
            }
        }
    }

    private void ResetAllObjects()
    {
        Debug.Log("Resetting all objects to their original positions...");
        ResettableObject[] resettableObjects = FindObjectsOfType<ResettableObject>();
        foreach (ResettableObject obj in resettableObjects)
        {
            obj.ResetPosition();
            Debug.Log("Object reset: " + obj.gameObject.name);
        }
    }
}
