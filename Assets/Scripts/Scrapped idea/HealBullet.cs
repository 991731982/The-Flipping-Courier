using UnityEngine;

public class HealOnHitBullet : MonoBehaviour
{
    public int healAmount = 20; // 每次命中敌人时恢复的生命值
    public string enemyTag = "Enemy"; // 敌人的Tag
    public string playerTag = "Player"; // 玩家的Tag

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            // 找到场景中的玩家
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);

            if (player != null)
            {
                // 获取玩家的 PlayerHealthDisplay 脚本
                PlayerHealthDisplay playerHealth = player.GetComponent<PlayerHealthDisplay>();

                if (playerHealth != null)
                {
                    // 调用 Heal 方法回复玩家生命值
                    playerHealth.Heal(healAmount);
                    Debug.Log($"Player healed by {healAmount} HP!");
                }
            }

            // 销毁子弹
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            // 找到场景中的玩家
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);

            if (player != null)
            {
                PlayerHealthDisplay playerHealth = player.GetComponent<PlayerHealthDisplay>();

                if (playerHealth != null)
                {
                    playerHealth.Heal(healAmount);
                    Debug.Log($"Player healed by {healAmount} HP!");
                }
            }

            Destroy(gameObject);
        }
    }
}
