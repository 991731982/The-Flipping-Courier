using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string mainMenuScene = "MainMenu";
    public string gameScene = "SampleScene";

    public void LoadGameScene()
    {
        // 注册回调以确保加载完成后切换 Lighting 设置
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 加载游戏场景
        SceneManager.LoadScene(gameScene, LoadSceneMode.Additive);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameScene)
        {
            // 设置 SampleScene 为活动场景
            SceneManager.SetActiveScene(scene);

            // 卸载主菜单场景
            SceneManager.UnloadSceneAsync(mainMenuScene);

            // 更新 Lighting 数据（重要）
            DynamicGI.UpdateEnvironment();

            // 取消回调
            SceneManager.sceneLoaded -= OnSceneLoaded;

            Debug.Log("SampleScene loaded, main menu unloaded, and lighting updated.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Game is exiting.");
        Application.Quit();
    }
}
