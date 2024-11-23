using UnityEngine;

public class CubeCharacterController : MonoBehaviour
{
    public Vector3 groundjump;
    public Vector3 roofjump;
    public float jumpForce = 2.0f;
    public bool isGrounded;
    private Rigidbody rb;
    public int jumpTime = 0;
    public float movementSpeed = 5f; // 移动速度
    private GravityController gravityController;
    private Animator animator;

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

        // 获取摄像机的方向
        Transform camTransform = Camera.main.transform;
        Vector3 camForward = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized; // 摄像机的前方向（水平）
        Vector3 camRight = new Vector3(camTransform.right.x, 0, camTransform.right.z).normalized;      // 摄像机的右方向（水平）

        // 基于摄像机方向的移动输入
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += camForward * movementSpeed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= camForward * movementSpeed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= camRight * movementSpeed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += camRight * movementSpeed;
            isMoving = true;
        }

        // 设置动画参数
        animator.SetBool("IsRunning", isMoving);
        animator.SetBool("IsIdle", !isMoving);

        // 应用移动速度
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        // 跳跃逻辑
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpTime < 1)
        {
            Vector3 jumpDirection = gravityController.gravityFlipped ? roofjump : groundjump;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpTime++;
        }

        // 根据移动方向旋转角色
        if (isMoving)
        {
            RotateCharacter(moveDirection);
        }
    }

    // 根据移动方向旋转角色
    void RotateCharacter(Vector3 moveDirection)
    {
        // 确保移动方向非零向量
        if (moveDirection.magnitude > 0.1f)
        {
            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));

            // 如果模型默认前向是 X轴正方向，则增加 Y轴旋转偏移
            Quaternion offset = Quaternion.Euler(0, -180, 0); // -90 度调整为实际偏移量
            targetRotation *= offset;

            // 平滑旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

}
