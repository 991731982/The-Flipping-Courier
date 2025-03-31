using UnityEngine;
using UnityEngine.UI;

public class TimerProgressBar : MonoBehaviour
{
    public Slider timerSlider;
    public float totalTime = 60f; // 可自訂總時間
    private float timer = 0f;

    [Header("Star Icons")]
    public Image[] stars; // 三顆星的 UI Image
    public float[] thresholds; // 每顆星的消失門檻百分比（例如 [0.3, 0.6, 0.9]）

    private bool[] starStates; // 追蹤哪些星還在

    void Start()
    {
        timer = 0f;
        timerSlider.value = 0f;

        // 初始化星星狀態陣列
        starStates = new bool[stars.Length];
        for (int i = 0; i < stars.Length; i++)
        {
            starStates[i] = true;
            if (stars[i] != null)
                stars[i].enabled = true;
        }
    }

    void Update()
    {
        if (timer < totalTime)
        {
            timer += Time.deltaTime;
            float percent = timer / totalTime;
            timerSlider.value = percent;

            UpdateStars(percent);
        }
    }

    void UpdateStars(float percent)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (starStates[i] && percent >= thresholds[i])
            {
                stars[i].enabled = false;
                starStates[i] = false;
            }
        }
    }

    public void ResetTimer()
    {
        timer = 0f;
        timerSlider.value = 0f;
        for (int i = 0; i < stars.Length; i++)
        {
            starStates[i] = true;
            if (stars[i] != null)
                stars[i].enabled = true;
        }
    }

    public float GetRemainingStarCount()
    {
        int count = 0;
        foreach (bool star in starStates)
        {
            if (star) count++;
        }
        return count;
    }
}
