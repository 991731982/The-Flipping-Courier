using UnityEngine;

public class Detector1 : MonoBehaviour
{
    public DoorLever1 doorLever1; // Reference to the DoorLever1 script on the door
    public string boxTag = "Box"; // Tag to identify the box (make sure the box has this tag)

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger zone has the specified tag (box)
        if (other.CompareTag(boxTag))
        {
            doorLever1.shouldOpenDoor = true; // Set the door to open
            Debug.Log("Box reached the bottom trigger, door is opening!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the trigger zone has the specified tag (box)
        if (other.CompareTag(boxTag))
        {
            doorLever1.shouldOpenDoor = false; // Set the door to close
            Debug.Log("Box left the bottom trigger, door is closing!");
        }
    }
}
