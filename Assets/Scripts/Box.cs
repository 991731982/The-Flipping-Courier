using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int hitsToDestroy = 3;         // Hits required to destroy the box
    public float fallAmount = 0.1f;      // Amount the box moves down each hit
    public GameObject smallCubePrefab;   // Prefab for the small cube to spawn
    public Vector3 spawnOffset = new Vector3(0, 1, 0); // Offset for spawning the small cube

    public int currentHits = 0;          // Track current hits
    private bool canRegisterHit = true;  // Control when hits can be registered

    void Start()
    {
        // Ensure the box has a Rigidbody and set it to kinematic (so it doesn't move, but still collides)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weight") && canRegisterHit)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                // 强制调整法线方向：将负的法线点积值转为正值
                float dotProduct = Vector3.Dot(contact.normal, transform.up);
                if (dotProduct < 0)
                {
                    dotProduct = -dotProduct;
                }

                Debug.Log($"Contact normal: {contact.normal}, Adjusted Dot product: {dotProduct}, Relative velocity Y: {collision.relativeVelocity.y}");

                // 判断是否为顶部碰撞
                if (collision.relativeVelocity.y < 0 && dotProduct > 0.4f) // 放宽条件
                {
                    currentHits++;
                    Debug.Log("Weight hit the box from the top! Current hits: " + currentHits);

                    // 向下移动箱子
                    transform.position -= new Vector3(0, fallAmount, 0);

                    // 如果达到受击次数
                    if (currentHits >= hitsToDestroy)
                    {
                        SpawnSmallCube(); // 生成小立方体
                        Destroy(gameObject); // 销毁箱子
                        return; // 退出循环，避免多次处理
                    }

                    // 冷却计时
                    StartCoroutine(HitCooldown());
                    return; // 退出循环，避免多次处理
                }
            }

            // 如果没有符合条件的碰撞点
            Debug.Log("Weight hit the side or at a low angle, not counted.");
        }
    }

    private void SpawnSmallCube()
    {
        // 使用偏移量计算生成位置
        Vector3 spawnPosition = transform.position + spawnOffset;

        // 实例化小立方体预制体
        Instantiate(smallCubePrefab, spawnPosition, Quaternion.identity);

        Debug.Log("Small cube spawned!");
    }

    // Coroutine to handle the cooldown period between hits
    private IEnumerator HitCooldown()
    {
        canRegisterHit = false;
        yield return new WaitForSeconds(2f);  // Wait for 2 seconds
        canRegisterHit = true;
    }
}
