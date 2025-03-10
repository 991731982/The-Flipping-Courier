using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public string mainMenuScene = "MainMenu";
    public string gameScene = "Protect-Level1";

    public AudioClip startGameSound; // �[���_ʼ��Ч
    public AudioClip backgroundMusic; // ��������
    private AudioSource audioSource;

    private void Start()
    {
        // ��� AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // ׌��������ѭ�h����
        audioSource.playOnAwake = false; // ��׌ Unity �ԄӲ���
        audioSource.volume = 0.5f; // �O��������С

        // ���ű�������
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
        // �����c����Ч
        if (startGameSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(startGameSound);
        }

        // �ȴ���Ч�����꣨���O��Ч 0.5 �룩
        yield return new WaitForSeconds(0.5f);

        // ֹͣ��������
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // �]�Ԉ������d���{
        SceneManager.sceneLoaded += OnSceneLoaded;

        // ���d�[�����
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

            UnityEngine.Debug.Log("SampleScene loaded, main menu unloaded, and lighting updated.");
        }
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("Game is exiting.");
        UnityEngine.Application.Quit();
    }
}
