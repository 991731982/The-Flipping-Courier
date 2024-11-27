using UnityEngine;

public class CubeCharacterController : MonoBehaviour
{
    public Vector3 groundjump;
    public Vector3 roofjump;
    public float jumpForce = 2.0f;
    public bool isGrounded;
    private Rigidbody rb;
    public int jumpTime = 0;
    public int movementspeed = 5;
    private GravityController gravityController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravityController = GetComponent<GravityController>();
        groundjump = new Vector3(0.0f, 2.0f, 0.0f);
        roofjump = new Vector3(0.0f, -2.0f, 0.0f);
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // Detect ground or roof based on gravity and ignore side contacts
            if (!gravityController.gravityFlipped && contact.normal.y > 0.5f)
            {
                isGrounded = true;
                jumpTime = 0;
            }
            else if (gravityController.gravityFlipped && contact.normal.y < -0.5f)
            {
                isGrounded = true;
                jumpTime = 0;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        bool exitingFromGroundOrCeiling = false;

        foreach (ContactPoint contact in collision.contacts)
        {
            // Only set isGrounded to false if exiting from ground or ceiling contact
            if ((!gravityController.gravityFlipped && contact.normal.y > 0.5f) ||
                (gravityController.gravityFlipped && contact.normal.y < -0.5f))
            {
                exitingFromGroundOrCeiling = true;
                break;
            }
        }

        if (exitingFromGroundOrCeiling)
        {
            isGrounded = false;
        }
    }


    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        // Movement input
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left * movementspeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right * movementspeed;
        }

        // Cap horizontal movement speed when grounded
        if (isGrounded && rb.velocity.magnitude > movementspeed)
        {
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, rb.velocity.z);
        }

        // Rotate the player to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            // Adjust rotation to consider character's default forward axis
            float angle = -Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpTime < 1)
        {
            Vector3 jumpDirection = gravityController.gravityFlipped ? roofjump : groundjump;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpTime++;
        }
    }

}
