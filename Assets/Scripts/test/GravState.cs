using UnityEngine;
using System.Collections;

public class GravState : MonoBehaviour
{
    private float originalMass;                   // 初始质量
    private float originalDrag;                   // 初始阻力
    private Bullet.BulletType originalState = Bullet.BulletType.Light; // 初始状态（可自定义）

    private Bullet.BulletType currentState;       // 当前状态 (Heavy / Light)
    private Rigidbody rb;

    [Header("子弹效果参数")]
    public float lightDrag = 3f;                  // Light 子弹时的空气阻力
    public float heavyMultiplier = 3f;            // Heavy 子弹的质量倍数
    public float heavyFallBoost = 20f;            // Heavy 子弹额外加速度
    public float effectDuration = 5f;             // 效果持续时间（秒）

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 记录初始质量和阻力
            originalMass = rb.mass;
            originalDrag = rb.drag;

            // 假设物体初始状态为 Light（你也可以改成 None/Heavy/自定义）
            currentState = originalState;

            Debug.Log($"{gameObject.name} 初始质量: {originalMass}, 初始阻力: {originalDrag}");
        }
        else
        {
            Debug.LogError($"{gameObject.name} 没有 Rigidbody，无法受子弹影响！");
        }
    }

    // 当子弹触发碰撞
    void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            Debug.Log($"{gameObject.name} 被 {bullet.bulletType} 子弹击中！");
            ApplyEffect(bullet.bulletType);
            Destroy(other.gameObject); // 销毁子弹
        }
    }

    void ApplyEffect(Bullet.BulletType bulletType)
    {
        if (rb == null) return;

        // 每次都强制刷新状态（如果你想避免重复生效，可加逻辑判断）
        if (bulletType == Bullet.BulletType.Heavy)
        {
            currentState = Bullet.BulletType.Heavy;
            // 质量变为原始的 heavyMultiplier 倍
            rb.mass = originalMass * heavyMultiplier;
            rb.drag = 0f;         // 让物体下落更快
            rb.useGravity = true;
            Debug.Log($"{gameObject.name} 变成 Heavy, mass={rb.mass}, drag={rb.drag}");
        }
        else // bulletType == Bullet.BulletType.Light
        {
            currentState = Bullet.BulletType.Light;
            rb.mass = originalMass * 0.5f;   // 变轻，假设是原始质量的一半
            rb.drag = lightDrag;            // 下落速度减慢
            rb.useGravity = true;
            Debug.Log($"{gameObject.name} 变成 Light, mass={rb.mass}, drag={rb.drag}");
        }

        // **启动协程**，在 5 秒后恢复原始状态
        StopAllCoroutines();  // 防止多个子弹效果叠加时重复计时
        StartCoroutine(RevertEffectAfterTime(effectDuration));
    }

    // 协程：等待指定秒数后，恢复原始状态
    IEnumerator RevertEffectAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        // 恢复到初始参数
        rb.mass = originalMass;
        rb.drag = originalDrag;
        rb.useGravity = true;   // 或者你自己的默认重力状态
        currentState = originalState;  // 重置回初始状态（如 Light / None / 等等）

        Debug.Log($"{gameObject.name} 效果结束，恢复原始状态: mass={rb.mass}, drag={rb.drag}");
    }

    // 在 Heavy 状态下，额外加速下落（可选）
    void FixedUpdate()
    {
        if (rb != null && currentState == Bullet.BulletType.Heavy)
        {
            Vector3 gravityDir = Physics.gravity.normalized;
            rb.AddForce(gravityDir * heavyFallBoost, ForceMode.Acceleration);
        }
    }
}
