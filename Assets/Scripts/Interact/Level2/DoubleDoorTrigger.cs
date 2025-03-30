using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject trigger1; // 第一個觸發區域
    public GameObject trigger2; // 第二個觸發區域
    public float riseSpeed = 2f; // 門上升的速度
    public float riseDistance = 3f; // 門上升的距離
    public LayerMask boxLayer; // 設置 Box 的 Layer 確保只偵測箱子

    private bool trigger1Active = false;
    private bool trigger2Active = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * riseDistance;
    }

    void FixedUpdate()
    {
        trigger1Active = CheckTrigger(trigger1);
        trigger2Active = CheckTrigger(trigger2);

        if (trigger1Active && trigger2Active)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * riseSpeed);
        }
    }

    bool CheckTrigger(GameObject trigger)
    {
        Collider[] colliders = Physics.OverlapBox(trigger.transform.position, trigger.transform.localScale / 2, Quaternion.identity, boxLayer);
        return colliders.Length > 0; // 如果偵測到物體，返回 true
    }
}
