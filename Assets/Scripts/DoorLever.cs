using UnityEngine;
using System.Collections;

public class DoorLever : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(0, 5, 0); // 门移动的方向
    public float moveSpeed = 2.0f; // 门移动的速度

    private Vector3 initialPosition; // 门的初始位置
    private bool isMoving = false;   // 防止同时进行多个移动操作
    private bool isOpen = false;     // 门的状态（是否已打开）

    // 初始化
    void Start()
    {
        initialPosition = transform.position; // 保存初始位置
    }

    // 打开门
    public void OpenDoor()
    {
        if (!isMoving && !isOpen)
        {
            StartCoroutine(OpenDoorRoutine());
        }
    }

    // 关闭门
    public void CloseDoor()
    {
        if (!isMoving && isOpen)
        {
            StartCoroutine(CloseDoorRoutine());
        }
    }

    // 控制门打开的动画
    private IEnumerator OpenDoorRoutine()
    {
        isMoving = true;
        Vector3 targetPosition = initialPosition + moveDirection; // 基于初始位置计算目标位置

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; // 确保最终位置精准
        isMoving = false;
        isOpen = true; // 更新状态
        Debug.Log("Door opened!");
    }

    // 控制门关闭的动画
    private IEnumerator CloseDoorRoutine()
    {
        isMoving = true;
        Vector3 targetPosition = initialPosition; // 目标位置是初始位置

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; // 确保最终位置精准
        isMoving = false;
        isOpen = false; // 更新状态
        Debug.Log("Door closed!");
    }

    // 重置门的状态和位置（可选）
    public void ResetDoor()
    {
        StopAllCoroutines(); // 停止所有正在运行的协程
        transform.position = initialPosition; // 恢复到初始位置
        isMoving = false; // 重置移动状态
        isOpen = false; // 重置门的状态
        Debug.Log("Door reset to initial position.");
    }
}
