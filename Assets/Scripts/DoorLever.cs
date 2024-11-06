using UnityEngine;
using System.Collections;

public class DoorLever : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(0, 5, 0); // Direction to move when opened
    public float moveSpeed = 2.0f; // Speed at which the door moves
    private bool doorOpened = false; // Track if the door has already opened

    public void OpenDoor()
    {
        if (!doorOpened)
        {
            StartCoroutine(OpenDoorRoutine());
            doorOpened = true;
        }
    }

    private IEnumerator OpenDoorRoutine()
    {
        Vector3 targetPosition = transform.position + moveDirection;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        Debug.Log("Door opened!");
    }
}
