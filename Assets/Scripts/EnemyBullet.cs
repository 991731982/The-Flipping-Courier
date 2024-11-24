using UnityEngine;

public class SpawnEnemyOnHitBullet : MonoBehaviour
{
    public GameObject enemyPrefab; // 新敌人的预制体
    public string enemyTag = "Enemy"; // 敌人的Tag
    public string playerTag = "Player"; // 玩家的Tag
    public float spawnRadius = 3f; // 新敌人生成在玩家附近的半径

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            // 找到玩家
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);

            if (player != null && enemyPrefab != null)
            {
                // 生成新敌人
                Vector3 spawnPosition = player.transform.position + Random.insideUnitSphere * spawnRadius;
                spawnPosition.y = player.transform.position.y; // 保持高度一致
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                Debug.Log("New enemy spawned near the player!");
            }

            // 销毁子弹
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);

            if (player != null && enemyPrefab != null)
            {
                Vector3 spawnPosition = player.transform.position + Random.insideUnitSphere * spawnRadius;
                spawnPosition.y = player.transform.position.y;
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                Debug.Log("New enemy spawned near the player!");
            }

            Destroy(gameObject);
        }
    }
}
