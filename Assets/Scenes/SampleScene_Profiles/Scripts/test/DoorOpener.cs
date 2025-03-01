using System.Collections;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public GameObject door;  // 拖入你的门对象
    public float openDistance = 3f; // 门打开的距离
    public float openSpeed = 2f; // 门打开的速度
    private bool isOpening = false; 
    private bool playerStanding = false;
    private float timer = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerStanding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStanding = false;
            timer = 0f; // 退出时重置计时
        }
    }

    private void Update()
    {
        if (playerStanding && !isOpening)
        {
            timer += Time.deltaTime;
            if (timer >= 3f) // 站立满3秒
            {
                StartCoroutine(OpenDoor());
                isOpening = true;
            }
        }
    }

    IEnumerator OpenDoor()
    {
        Vector3 targetPosition = door.transform.position + new Vector3(openDistance, 0, 0); // 门向X轴移动
        while (Vector3.Distance(door.transform.position, targetPosition) > 0.1f)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, targetPosition, Time.deltaTime * openSpeed);
            yield return null;
        }
    }
}
