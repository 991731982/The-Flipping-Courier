using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("Grappling Settings")]
    public Transform grapplePoint;
    public float grappleRange = 15f;
    public float initialRopeLength = 5f;
    public float minRopeLength = 2f;
    public float maxRopeLength = 20f;
    public float ropeWindSpeed = 2f;

    [Header("Swing Physics Settings")]
    public float customGravity = 9.81f;
    public float baseSwingForce = 3f;
    public float maxSwingSpeed = 15f;
    public float airResistance = 0.1f;
    public float energyLoss = 0.1f;
    public float momentumTransfer = 0.8f;

    [Header("Advanced Swing Settings")]
    public float swingAmplification = 1.2f; // 摆动力放大系数
    public float maxSwingHeight = 10f; // 最大摆动高度
    public float optimalSwingAngle = 45f; // 最佳推力角度(度)
    public float momentumBonus = 0.5f; // 顺势推动时的额外动力
    public float counterSwingPenalty = 0.3f; // 逆势推动时的惩罚系数

    private Rigidbody rb;
    private ConfigurableJoint joint;
    private bool isSwinging = false;
    private float currentRopeLength;
    private Vector3 initialGrapplePos;
    private Vector3 swingStartPos;
    private float currentSwingPhase = 0f; // 当前摆动相位
    private float lastSwingHeight = 0f; // 上一帧的高度
    private float swingEnergy = 0f; // 累积的摆动能量
    private float timeSinceLastOptimalPush = 0f; // 记录最后一次最佳推力时间

    // 用于调试的变量
    private float debugSwingAngle = 0f;
    private bool isInOptimalPushZone = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.maxAngularVelocity = 50f;
    }

    void Update()
    {
        HandleInput();
        UpdateSwingPhase();
    }

    void FixedUpdate()
    {
        if (isSwinging)
        {
            ApplyCustomGravity();
            HandleSwinging();
            ApplyAirResistance();
            UpdateSwingMechanics();
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TryToGrapple();
        }

        if (isSwinging)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                currentRopeLength = Mathf.Max(minRopeLength, currentRopeLength - ropeWindSpeed * Time.deltaTime);
                UpdateRopeLength();
            }
            if (Input.GetKey(KeyCode.E))
            {
                currentRopeLength = Mathf.Min(maxRopeLength, currentRopeLength + ropeWindSpeed * Time.deltaTime);
                UpdateRopeLength();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ReleaseGrapple();
            }
        }
    }

    void UpdateSwingPhase()
    {
        if (!isSwinging) return;

        Vector3 toPlayer = transform.position - grapplePoint.position;
        Vector3 horizontalToPlayer = new Vector3(toPlayer.x, 0, toPlayer.z);

        // 计算当前摆动角度
        debugSwingAngle = Vector3.SignedAngle(Vector3.right, horizontalToPlayer, Vector3.forward);

        // 更新摆动相位
        float verticalVelocity = rb.velocity.y;
        float heightDifference = transform.position.y - lastSwingHeight;

        // 检测摆动方向变化
        if (Mathf.Abs(heightDifference) > 0.01f)
        {
            currentSwingPhase = (heightDifference > 0) ? 1f : -1f;
        }

        lastSwingHeight = transform.position.y;

        // 判断是否在最佳推力区域
        float swingAngleAbs = Mathf.Abs(debugSwingAngle);
        isInOptimalPushZone = swingAngleAbs > (optimalSwingAngle - 15f) &&
                             swingAngleAbs < (optimalSwingAngle + 15f);
    }

    void HandleSwinging()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalInput) < 0.1f && Mathf.Abs(verticalInput) < 0.1f)
        {
            swingEnergy *= (1f - energyLoss);
            return;
        }

        Vector3 swingDirection = (grapplePoint.position - transform.position).normalized;
        Vector3 perpendicularDirection = Vector3.Cross(swingDirection, Vector3.up).normalized;

        // 计算当前摆动高度
        float currentHeight = transform.position.y - grapplePoint.position.y;
        float heightRatio = Mathf.Clamp01(currentHeight / maxSwingHeight);

        // 计算推力系数
        float pushFactor = 1f;
        if (isInOptimalPushZone)
        {
            pushFactor = swingAmplification;
            timeSinceLastOptimalPush = 0f;

            // 检查是否在顺势推动
            if (Mathf.Sign(horizontalInput) == Mathf.Sign(currentSwingPhase))
            {
                pushFactor *= (1f + momentumBonus);
                swingEnergy += baseSwingForce * Time.fixedDeltaTime;
            }
            else
            {
                pushFactor *= counterSwingPenalty;
                swingEnergy *= counterSwingPenalty;
            }
        }
        else
        {
            timeSinceLastOptimalPush += Time.fixedDeltaTime;
            pushFactor = Mathf.Lerp(1f, 0.5f, timeSinceLastOptimalPush);
        }

        // 应用推力
        Vector3 force = Vector3.zero;
        if (horizontalInput != 0)
        {
            force += perpendicularDirection * horizontalInput * baseSwingForce * pushFactor;
        }
        if (verticalInput != 0)
        {
            force += Vector3.Cross(perpendicularDirection, swingDirection) * verticalInput * baseSwingForce * pushFactor;
        }

        // 考虑累积的摆动能量
        force *= (1f + swingEnergy * 0.1f);

        // 限制最大速度
        if (rb.velocity.magnitude < maxSwingSpeed)
        {
            rb.AddForce(force * rb.mass, ForceMode.Force);
        }

        // 高度限制
        if (currentHeight > maxSwingHeight)
        {
            Vector3 downForce = Vector3.down * (currentHeight - maxSwingHeight) * 10f;
            rb.AddForce(downForce * rb.mass, ForceMode.Force);
        }
    }

    void TryToGrapple()
    {
        if (grapplePoint != null && Vector3.Distance(transform.position, grapplePoint.position) <= grappleRange)
        {
            StartSwinging();
        }
    }

    void StartSwinging()
    {
        isSwinging = true;
        swingStartPos = transform.position;
        initialGrapplePos = grapplePoint.position;
        currentRopeLength = Vector3.Distance(transform.position, grapplePoint.position);
        swingEnergy = 0f;
        lastSwingHeight = transform.position.y;

        SetupJoint();
    }

    void SetupJoint()
    {
        if (joint != null) Destroy(joint);

        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint.position;

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        var limit = new SoftJointLimit
        {
            limit = currentRopeLength,
            bounciness = 0f
        };
        joint.linearLimit = limit;

        var spring = new SoftJointLimitSpring
        {
            spring = 0f,
            damper = 5f
        };
        joint.linearLimitSpring = spring;

        joint.massScale = 1f;
        joint.connectedMassScale = 1f;

        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;
    }

    void ApplyCustomGravity()
    {
        rb.AddForce(Vector3.down * customGravity * rb.mass, ForceMode.Force);
    }

    void ApplyAirResistance()
    {
        Vector3 velocity = rb.velocity;
        float velocityMagnitude = velocity.magnitude;
        Vector3 dragForce = -velocity.normalized * velocityMagnitude * velocityMagnitude * airResistance;
        rb.AddForce(dragForce, ForceMode.Force);
    }

    void UpdateSwingMechanics()
    {
        // 绳子张力
        Vector3 ropeDirection = (grapplePoint.position - transform.position).normalized;
        float tensionMagnitude = Vector3.Dot(rb.velocity, ropeDirection) * rb.mass / Time.fixedDeltaTime;

        if (tensionMagnitude > 0)
        {
            Vector3 tensionForce = ropeDirection * tensionMagnitude;
            rb.AddForce(tensionForce, ForceMode.Force);
        }

        // 自然能量损失
        swingEnergy *= (1f - energyLoss * Time.fixedDeltaTime);
    }

    void UpdateRopeLength()
    {
        if (joint != null)
        {
            var limit = joint.linearLimit;
            limit.limit = currentRopeLength;
            joint.linearLimit = limit;
        }
    }

    void ReleaseGrapple()
    {
        if (joint != null)
        {
            Destroy(joint);
        }

        isSwinging = false;

        // 保留动量并加入累积的摆动能量
        Vector3 releaseVelocity = rb.velocity;
        float velocityMagnitude = releaseVelocity.magnitude;
        Vector3 normalizedVelocity = releaseVelocity.normalized;

        // 将累积的摆动能量转化为额外的释放速度
        float finalSpeed = velocityMagnitude * momentumTransfer * (1f + swingEnergy * 0.2f);
        rb.velocity = normalizedVelocity * finalSpeed;

        swingEnergy = 0f;
    }

    void OnDrawGizmos()
    {
        if (!isSwinging || grapplePoint == null) return;

        // 绘制基本信息
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(grapplePoint.position, grappleRange);

        // 绘制当前绳索
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, grapplePoint.position);

        // 绘制最佳推力区域
        if (isInOptimalPushZone)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        // 绘制摆动相位指示器
        Gizmos.color = Color.blue;
        Vector3 phaseDirection = Vector3.right * currentSwingPhase;
        Gizmos.DrawRay(transform.position, phaseDirection);

        // 绘制累积能量指示器
        Gizmos.color = Color.yellow;
        float energyIndicatorSize = 0.5f + swingEnergy * 0.1f;
        Gizmos.DrawWireSphere(transform.position, energyIndicatorSize);
    }
}