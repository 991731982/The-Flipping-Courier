using UnityEngine;

public class Detector : MonoBehaviour
{
    public DoorLever doorLever; // DoorLever �ű�������
    public string boxTag = "Box"; // ���ڱ�ʶ���ӵı�ǩ

    // �����ӽ��봥������ʱ
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(boxTag))
        {
            doorLever.OpenDoor(); // ����
            Debug.Log("Box entered, door is opening!");
        }
    }

    // �������뿪��������ʱ
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(boxTag))
        {
            doorLever.CloseDoor(); // �ر���
            Debug.Log("Box exited, door is closing!");
        }
    }
}
