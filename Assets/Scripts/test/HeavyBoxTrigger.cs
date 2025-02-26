using System.Diagnostics;
using UnityEngine;

public class HeavyBoxTrigger : MonoBehaviour
{
    public float massThreshold = 400f; // 設定質量門檻，大於這個值才觸發下降
    public float dropDistance = 10f; // 下降距離
    public float dropSpeed = 5f; // 下降速度（可調整，設為 0 變瞬間下降）

    private bool shouldDrop = false;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position + Vector3.down * dropDistance; // 計算下降後的位置
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;

        // 檢查是否是 Box，並且其質量是否超過門檻
        if (collision.gameObject.CompareTag("Box") && rb != null && rb.mass >= massThreshold)
        {
            UnityEngine.Debug.Log("重物 Box 撞擊！白色盒子開始下降！");
            shouldDrop = true;
        }
    }

    void Update()
    {
        if (shouldDrop)
        {
            // 讓白色盒子緩慢下降
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dropSpeed * Time.deltaTime);

            // 檢查是否到達目標位置
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                shouldDrop = false; // 停止移動
            }
        }
    }
}
