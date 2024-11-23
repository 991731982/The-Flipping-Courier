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

    private bool isSideScroller = false; // 是否切换为横向视角
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravityController = GetComponent<GravityController>();
        groundjump = new Vector3(0.0f, 2.0f, 0.0f);
        roofjump = new Vector3(0.0f, -2.0f, 0.0f);
        animator = GetComponent<Animator>();

        // 隐藏鼠标光标并锁定到游戏窗口
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 获取主摄像机
        mainCamera = Camera.main;
    }

    void OnTriggerEnter(Collider other)
    {
        // 检测是否碰到 Checkpoint
        if (other.CompareTag("Checkpoint"))
        {
            SwitchToSideScroller();
        }
    }

    void SwitchToSideScroller()
    {
        isSideScroller = true;

        // 修改摄像机位置和方向
        mainCamera.transform.position = new Vector3(transform.position.x - 5f, transform.position.y + 2f, transform.position.z);
        mainCamera.transform.rotation = Quaternion.Euler(0, 90, 0); // 横向视角

        // 锁定摄像机的跟随行为（可以用脚本实现动态跟随玩家）
    }

    void Update()
    {
        if (isSideScroller)
        {
            SideScrollerMovement();
        }
        else
        {
            FreeMovement();
        }
    }

    // 自由视角移动逻辑
    void FreeMovement()
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

    // 横向视角移动逻辑
    void SideScrollerMovement()
    {
        Vector3 moveDirection = Vector3.zero;
        bool isMoving = false;

        // 横向移动输入
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = Vector3.left;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection = Vector3.right;
            isMoving = true;
        }

        moveDirection *= movementSpeed;

        // 设置动画参数
        animator.SetBool("IsRunning", isMoving);
        animator.SetBool("IsIdle", !isMoving);

        // 应用移动速度，仅在 X 轴移动
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, 0);

        // 跳跃逻辑
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpTime < 1)
        {
            Vector3 jumpDirection = groundjump; // 横向视角只考虑向上的跳跃
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
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, 0)); // 横向视角固定 Z 轴
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
