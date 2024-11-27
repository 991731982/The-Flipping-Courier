using UnityEngine;

public class Drag : MonoBehaviour
{
    public Transform player;               // Reference to the player
    public float dragDistance = 5f;        // Maximum distance to start dragging
    public float followSpeed = 5f;         // Speed factor for the object to follow the player
    public Vector3 dragOffset = new Vector3(1f, 0, 1f); // Offset from player position to prevent pushing
    public LayerMask groundLayer;          // Layer mask for the ground

    private bool isDragging = false;
    private bool isCollidingWithPlayer = false; // Flag to stop movement if in contact with player
    private Rigidbody rb;
    private bool isOnRightSide = true;     // Tracks if object was initially on the right side of the player

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth movement
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Prevent passing through objects
    }

    void Update()
    {
        // Check if player is within dragDistance and press 'T' to toggle dragging
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isDragging)
            {
                // Release the object
                isDragging = false;
                Debug.Log("Object released.");
            }
            else if (Vector3.Distance(transform.position, player.position) <= dragDistance)
            {
                // Start dragging and determine initial side
                isDragging = true;
                isOnRightSide = transform.position.x >= player.position.x; // Set side based on initial position
              
            }
        }
    }

    void FixedUpdate()
    {
        // Stop dragging if the object is out of range
        if (isDragging && Vector3.Distance(transform.position, player.position) > dragDistance)
        {
            isDragging = false;
            Debug.Log("Dragging stopped due to distance.");
            return;
        }

        // If dragging and not colliding with the player, move the object smoothly
        if (isDragging && !isCollidingWithPlayer)
        {
            // Apply offset based on initial side
            Vector3 sideOffset = dragOffset;
            if (!isOnRightSide)
            {
                sideOffset.x = -Mathf.Abs(dragOffset.x); // Flip offset to keep object on left
            }

            Vector3 targetPosition = player.position + sideOffset;
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition); // Use MovePosition to smoothly move the object with collision detection
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If the object collides with the player, stop moving
        if (other.transform == player)
        {
            isCollidingWithPlayer = true;
            rb.velocity = Vector3.zero; // Stop velocity to avoid pushing the player
            Debug.Log("Object is in contact with the player, movement stopped.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the object exits collision with the player, resume movement
        if (other.transform == player)
        {
            isCollidingWithPlayer = false;
            Debug.Log("Object is no longer in contact with the player, movement resumed.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If the object collides with any other object (not the player or ground), stop dragging
        if (collision.transform != player && (groundLayer.value & (1 << collision.gameObject.layer)) == 0)
        {
            isDragging = false;
            rb.velocity = Vector3.zero; // Stop all movement
            //Debug.Log("Object collided with another object, dragging stopped.");
        }
    }
}
