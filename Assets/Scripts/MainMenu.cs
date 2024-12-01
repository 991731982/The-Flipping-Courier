using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string mainMenuScene = "MainMenu";
    public string gameScene = "SampleScene";

    public void LoadGameScene()
    {
        // ע��ص���ȷ��������ɺ��л� Lighting ����
        SceneManager.sceneLoaded += OnSceneLoaded;

        // ������Ϸ����
        SceneManager.LoadScene(gameScene, LoadSceneMode.Additive);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameScene)
        {
            // ���� SampleScene Ϊ�����
            SceneManager.SetActiveScene(scene);

            // ж�����˵�����
            SceneManager.UnloadSceneAsync(mainMenuScene);

            // ���� Lighting ���ݣ���Ҫ��
            DynamicGI.UpdateEnvironment();

            // ȡ���ص�
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
