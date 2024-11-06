using UnityEngine;
using System.Collections;

public class DoorLever1 : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(0, -10, 0); // Direction to move when opened
    public float moveSpeed = 2.0f; // Speed at which the door moves
    private bool doorOpened = false; // Track if the door has already opened
    private Vector3 initialPosition;

    public bool shouldOpenDoor; // New public variable to control door state

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (shouldOpenDoor)
        {
            if (!doorOpened)
            {
                StopAllCoroutines();
                StartCoroutine(OpenDoorRoutine1());
            }
        }
        else
        {
            if (doorOpened)
            {
                StopAllCoroutines();
                StartCoroutine(CloseDoorRoutine1());
            }
        }
    }

    private IEnumerator OpenDoorRoutine1()
    {
        Vector3 targetPosition = initialPosition + moveDirection;
        doorOpened = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        Debug.Log("Door opened!");
    }

    private IEnumerator CloseDoorRoutine1()
    {
        doorOpened = false;

        while (Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = initialPosition;
        Debug.Log("Door closed!");
    }
}
