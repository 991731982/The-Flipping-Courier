using UnityEngine;

public class Detector : MonoBehaviour
{
    public DoorLever doorLever; // Reference to the DoorController script on the door
    public string boxTag = "Box"; // Tag to identify the box (make sure the box has this tag)

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger zone has the specified tag (box)
        if (other.CompareTag(boxTag))
        {
            doorLever.OpenDoor(); // Trigger the door to open
            Debug.Log("Box reached the bottom trigger, door is opening!");

            // Optionally disable this trigger to prevent multiple activations
            gameObject.SetActive(false);
        }
    }
}
