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

    private bool isCursorLocked = true; // 当前鼠标是否锁定

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravityController = GetComponent<GravityController>();
        groundjump = new Vector3(0.0f, 2.0f, 0.0f);
        roofjump = new Vector3(0.0f, -2.0f, 0.0f);
        animator = GetComponent<Animator>();

        // 初始化锁定鼠标光标
        LockCursor();

        // 监听窗口焦点变化事件
        Application.focusChanged += OnApplicationFocusChanged;
    }

    void Update()
    {
        // ESC 键切换鼠标状态
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }

        Vector3 moveDirection = Vector3.zero;
        bool isMoving = false;

        // 获取摄像机的方向
        Transform camTransform = Camera.main.transform;
        Vector3 camForward = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized; // 摄像机的前方向（水平）
        Vector3 camRight = new Vector3(camTransform.right.x, 0, camTransform.right.z).normalized;      // 摄像机的右方向（水平）

        // 基于摄像机方向的移动输入
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += camForward;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= camForward;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= camRight;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += camRight;
            isMoving = true;
        }

        // 移动方向归一化并乘以速度
        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        moveDirection *= movementSpeed;

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

    void RotateCharacter(Vector3 moveDirection)
    {
        // 确保移动方向非零向量
        if (moveDirection.magnitude > 0.1f)
        {
            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));

            // 根据模型默认方向调整偏移量
            Quaternion offset = Quaternion.Euler(0, 180, 0); // 如果模型前向是 X 轴，调整为 90 度偏移
            targetRotation *= offset;

            // 平滑旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isCursorLocked = true;
        Debug.Log("Cursor locked.");
    }

    void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isCursorLocked = false;
        Debug.Log("Cursor unlocked.");
    }

    void ToggleCursorLock()
    {
        if (isCursorLocked)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    void OnApplicationFocusChanged(bool hasFocus)
    {
        if (hasFocus && isCursorLocked)
        {
            LockCursor();
        }
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
}
