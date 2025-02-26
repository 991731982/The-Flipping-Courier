using UnityEngine;

public class BoxCollisionTrigger : MonoBehaviour
{
    public GameObject cubeToMove; // 右側的Cube
    public float moveDistance = 2f; // Cube上升的距離
    public float moveSpeed = 2f; // 上升速度

    private bool shouldMove = false;
    private Vector3 startPos;
    private Vector3 targetPos;

    void Start()
    {
        if (cubeToMove != null)
        {
            startPos = cubeToMove.transform.position;
            targetPos = startPos + Vector3.up * moveDistance;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box") && cubeToMove != null)
        {
            shouldMove = true;
        }
    }

    void Update()
    {
        if (shouldMove && cubeToMove != null)
        {
            cubeToMove.transform.position = Vector3.MoveTowards(cubeToMove.transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(cubeToMove.transform.position, targetPos) < 0.01f)
            {
                shouldMove = false; // 停止移動
            }
        }
    }
}
