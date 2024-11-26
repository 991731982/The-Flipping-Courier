using UnityEngine;

public class Detector : MonoBehaviour
{
    public DoorLever doorLever; // DoorLever 脚本的引用
    public string boxTag = "Box"; // 用于标识箱子的标签

    // 当箱子进入触发区域时
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(boxTag))
        {
            doorLever.OpenDoor(); // 打开门
            Debug.Log("Box entered, door is opening!");
        }
    }

    // 当箱子离开触发区域时
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(boxTag))
        {
            doorLever.CloseDoor(); // 关闭门
            Debug.Log("Box exited, door is closing!");
        }
    }
}
