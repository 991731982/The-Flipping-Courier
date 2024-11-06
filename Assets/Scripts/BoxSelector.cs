using System.Collections;
using UnityEngine;
using TMPro;

public class BoxSelector : MonoBehaviour
{
    public TextMeshProUGUI countdownText;// 引用倒计时的 TextMeshProUGUI 组件
    private GameObject selectedObject;
    private bool isSelecting = false;
    private float countdownTime = 6f; // 倒计时时间

    void Update()
    {
        // 当按下 C 键时开始选择
        if (Input.GetKeyDown(KeyCode.C))
        {
            isSelecting = true;
            countdownTime = 6f; // 重置倒计时时间
            countdownText.gameObject.SetActive(true); // 显示倒计时文本
            StartCoroutine(CountdownCoroutine()); // 启动倒计时协程
        }

        // 如果正在选择且按住 C 键，选择最近的 box
        if (isSelecting && Input.GetKey(KeyCode.C))
        {
            SelectBox();
        }

        // 松开 C 键时，停止选中物体 6 秒后恢复重力
        if (isSelecting && Input.GetKeyUp(KeyCode.C) && selectedObject != null)
        {
            StartCoroutine(StopSelectedObjectTemporarily());
            selectedObject = null;
            isSelecting = false;
        }

    
    }

    void SelectBox()
    {
        // 查找场景中所有带有 "box" 标签的物体
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("float");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject box in boxes)
        {
            float distance = Vector3.Distance(transform.position, box.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                selectedObject = box;
            }
        }

        // 可选：高亮显示选中的物体
        if (selectedObject != null)
        {
            selectedObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    IEnumerator StopSelectedObjectTemporarily()
    {
        if (selectedObject != null)
        {
            Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 暂时静止物体
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;

                // 等待 6 秒
                yield return new WaitForSeconds(6f);

                // 恢复重力
                rb.isKinematic = false;
            }
        }
    }

    IEnumerator CountdownCoroutine()
    {
        while (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            countdownText.text = "Time Remaining: " + Mathf.Ceil(countdownTime).ToString() + "s";
            yield return null;
        }

        // 倒计时结束后隐藏文本
        countdownText.gameObject.SetActive(false);
    }
}
