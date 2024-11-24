using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float chaseRange = 10f; // The range within which the enemy will chase the player
    public float moveSpeed = 3f; // Speed at which the enemy will move towards the player
    public float stoppingDistance = 1f; // Distance at which the enemy stops before reaching the player

    private Rigidbody rb; // Reference to the Rigidbody component
    private bool isChasing = false;

    private bool canDamagePlayer = true; // 是否可以对玩家造成伤害

    void Start()
    {
        // 自动找到玩家对象，假设玩家对象的 Tag 为 "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found! Make sure the player has the 'Player' tag.");
        }

        // 获取 Rigidbody 组件
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the chase range
        if (distanceToPlayer <= chaseRange && distanceToPlayer > stoppingDistance)
        {
            isChasing = true; // Start chasing
        }
        else
        {
            isChasing = false; // Stop chasing
        }

        // If the enemy is chasing the player, move towards the player
        if (isChasing)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        // Calculate the direction from the enemy to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Preserve the current Y (vertical) velocity for gravity
        Vector3 velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);

        // Apply velocity to the Rigidbody to move the enemy horizontally while maintaining gravity
        rb.velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthDisplay playerHealth = collision.gameObject.GetComponent<PlayerHealthDisplay>();

            if (playerHealth != null)
            {
                // 减少玩家生命值
                playerHealth.TakeDamage(10);

                if (playerHealth.IsDead())
                {
                    // 玩家死亡，触发复活逻辑
                    checkPointRespawn playerRespawn = collision.gameObject.GetComponent<checkPointRespawn>();
                    if (playerRespawn != null)
                    {
                        playerRespawn.RespawnAtCheckpoint();
                    }
                }
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamagePlayer = false;
        yield return new WaitForSeconds(5f); // 5 秒冷却时间
        canDamagePlayer = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
