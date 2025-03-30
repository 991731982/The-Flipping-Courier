using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetClickToScene : MonoBehaviour
{
    [Tooltip("輸入或從下拉選擇想要切換的場景名稱")]
    public string targetSceneName;

    private void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            UnityEngine.Debug.LogWarning("未設定目標場景名稱！");
        }
    }
}
