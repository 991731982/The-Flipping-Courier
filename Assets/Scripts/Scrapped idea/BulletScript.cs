using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public string enemyTag = "Enemy"; // 敌人的Tag
    public float bulletLifetime = 5f; // 子弹存在时间

    private void Start()
    {
        // 在一定时间后销毁子弹
        Destroy(gameObject, bulletLifetime);
    }

    private void OnCollisionEnter(Collision collision) // 检测碰撞
    {
        // 检查碰撞物体是否为敌人
        if (collision.gameObject.CompareTag(enemyTag))
        {
            // 获取敌人健康脚本
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                // 让敌人受到伤害
                enemyHealth.TakeDamage();
            }

            // 销毁子弹
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) // 如果用触发器检测
    {
        if (other.CompareTag(enemyTag))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage();
            }

            Destroy(gameObject);
        }
    }
}
