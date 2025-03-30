using UnityEngine;
using System.Collections;

public class DoorLever : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(0, 5, 0); // ���ƶ��ķ���
    public float moveSpeed = 2.0f; // ���ƶ����ٶ�

    private Vector3 initialPosition; // �ŵĳ�ʼλ��
    private bool isMoving = false;   // ��ֹͬʱ���ж���ƶ�����
    private bool isOpen = false;     // �ŵ�״̬���Ƿ��Ѵ򿪣�

    // ��ʼ��
    void Start()
    {
        initialPosition = transform.position; // �����ʼλ��
    }

    // ����
    public void OpenDoor()
    {
        if (!isMoving && !isOpen)
        {
            StartCoroutine(OpenDoorRoutine());
        }
    }

    // �ر���
    public void CloseDoor()
    {
        if (!isMoving && isOpen)
        {
            StartCoroutine(CloseDoorRoutine());
        }
    }

    // �����Ŵ򿪵Ķ���
    private IEnumerator OpenDoorRoutine()
    {
        isMoving = true;
        Vector3 targetPosition = initialPosition + moveDirection; // ���ڳ�ʼλ�ü���Ŀ��λ��

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; // ȷ������λ�þ�׼
        isMoving = false;
        isOpen = true; // ����״̬
        Debug.Log("Door opened!");
    }

    // �����ŹرյĶ���
    private IEnumerator CloseDoorRoutine()
    {
        isMoving = true;
        Vector3 targetPosition = initialPosition; // Ŀ��λ���ǳ�ʼλ��

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; // ȷ������λ�þ�׼
        isMoving = false;
        isOpen = false; // ����״̬
        Debug.Log("Door closed!");
    }

    // �����ŵ�״̬��λ�ã���ѡ��
    public void ResetDoor()
    {
        StopAllCoroutines(); // ֹͣ�����������е�Э��
        transform.position = initialPosition; // �ָ�����ʼλ��
        isMoving = false; // �����ƶ�״̬
        isOpen = false; // �����ŵ�״̬
        Debug.Log("Door reset to initial position.");
    }
}
