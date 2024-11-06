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
    private Animator animator;
    private bool facingRight = true; // Track the direction the character is facing

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravityController = GetComponent<GravityController>();
        groundjump = new Vector3(0.0f, 2.0f, 0.0f);
        roofjump = new Vector3(0.0f, -2.0f, 0.0f);
        animator = GetComponent<Animator>();
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
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
        bool isMoving = false;

        // Move left and rotate to face left
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left * movementspeed;
            isMoving = true;

            if (facingRight)
            {
                RotateCharacter(false); // Rotate to face left
            }
        }
        // Move right and rotate to face right
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right * movementspeed;
            isMoving = true;

            if (!facingRight)
            {
                RotateCharacter(true); // Rotate to face right
            }
        }

        // Set animator parameter for running and idle based on movement
        animator.SetBool("IsRunning", isMoving);
        animator.SetBool("IsIdle", !isMoving);

        // Apply horizontal velocity
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, rb.velocity.z);

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpTime < 1)
        {
            Vector3 jumpDirection = gravityController.gravityFlipped ? roofjump : groundjump;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpTime++;
        }
    }

    // Rotate character based on direction
    void RotateCharacter(bool faceRight)
    {
        facingRight = faceRight;
        transform.rotation = Quaternion.Euler(0, faceRight ? -90 : 90, 0);
    }
}
