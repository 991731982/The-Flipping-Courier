using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; // 子弹存活时间

    void Start()
    {
        // 设置子弹的自动销毁时间
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检测是否撞到带有 "Enemy" 标签的对象
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 获取敌人的 EnemyHealth 脚本并调用 TakeDamage()
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage();
            }

            // 销毁子弹
            Destroy(gameObject);
        }
    }
}
