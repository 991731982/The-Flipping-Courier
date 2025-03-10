using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public string mainMenuScene = "MainMenu";
    public string gameScene = "Protect-Level1";

    public AudioClip startGameSound; // 遊戲開始音效
    public AudioClip backgroundMusic; // 背景音樂
    private AudioSource audioSource;

    private void Start()
    {
        // 添加 AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // 讓背景音樂循環播放
        audioSource.playOnAwake = false; // 不讓 Unity 自動播放
        audioSource.volume = 0.5f; // 設定音量大小

        // 播放背景音樂
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
    }

    public void LoadGameScene()
    {
        StartCoroutine(PlayClickAndLoadScene());
    }

    private IEnumerator PlayClickAndLoadScene()
    {
        // 播放點擊音效
        if (startGameSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(startGameSound);
        }

        // 等待音效播放完（假設音效 0.5 秒）
        yield return new WaitForSeconds(0.5f);

        // 停止背景音樂
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // 註冊場景加載回調
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 加載遊戲場景
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

            UnityEngine.Debug.Log("SampleScene loaded, main menu unloaded, and lighting updated.");
        }
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("Game is exiting.");
        UnityEngine.Application.Quit();
    }
}
