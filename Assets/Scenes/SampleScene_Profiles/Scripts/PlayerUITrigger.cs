using UnityEngine;

public class PlayerUITrigger : MonoBehaviour
{
    public GameObject uiElement; // 需要显示/隐藏的 UI 元素

    void Start()
    {
        // 确保 UI 元素初始状态为隐藏
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
    }

    // 当玩家进入触发器区域时，显示 UI
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UITrigger")) // 确保触发器物体有正确的标签
        {
            if (uiElement != null)
            {
                uiElement.SetActive(true);
            }
        }
    }

    // 当玩家离开触发器区域时，隐藏 UI
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UITrigger"))
        {
            if (uiElement != null)
            {
                uiElement.SetActive(false);
            }
        }
    }
}
