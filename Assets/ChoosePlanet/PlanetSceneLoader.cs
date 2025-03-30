using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetClickToScene : MonoBehaviour
{
    [Tooltip("ݔ���������x����Ҫ�ГQ�Ĉ������Q")]
    public string targetSceneName;

    private void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            UnityEngine.Debug.LogWarning("δ�O��Ŀ�ˈ������Q��");
        }
    }
}
