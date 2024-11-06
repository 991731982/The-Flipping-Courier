using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Box box; // Reference to the Box script
    public Vector3 moveDirection = new Vector3(0, 5, 0); // Direction the door will move when opened
    public float moveSpeed = 2.0f; // Speed at which the door moves
    private bool doorOpened = false; // Track if the door has already opened

    void Update()
    {
        // Check if the box has been hit enough times and the door has not yet opened
        if (box.currentHits >= box.hitsToDestroy && !doorOpened)
        {
            StartCoroutine(OpenDoor());
            doorOpened = true; // Ensure this only happens once
        }
    }

    private IEnumerator OpenDoor()
    {
        // Calculate the target position the door will move to
        Vector3 targetPosition = transform.position + moveDirection;

        // Move the door towards the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }

        // Ensure final position
        transform.position = targetPosition;
        Debug.Log("Door opened!");
    }
}