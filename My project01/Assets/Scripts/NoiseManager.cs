using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance { get; private set; }

    [Header("总噪声")]
    public float noise = 0f;
    public float maxNoise = 100f;

    [Tooltip("是否在没有探测器加噪声时也自动衰减")]
    public float baseDecayRate = 0f;   // 不想全局衰减就填 0

    [Header("UI")]
    public Slider noiseSlider;         // 建议 min=0, max=1
           // 可选，用颜色表示强度

    bool noiseReachedMax = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // 全局被动衰减（可选）
        if (baseDecayRate > 0f)
        {
            noise -= baseDecayRate * dt;
        }

        noise = Mathf.Clamp(noise, 0f, maxNoise);

        // 更新 UI
        if (noiseSlider)
        {
            noiseSlider.value = noise / maxNoise;
        }

       

        // 噪声满的事件（你可以在这里调用 GameOver）
        if (!noiseReachedMax && noise >= maxNoise)
        {
            noiseReachedMax = true;
            Debug.Log("总噪声已满 → 这里触发 GameOver / 敌人警觉");
            // TODO: 调用你的 GameManager
        }
        else if (noise < maxNoise * 0.9f)
        {
            // 留个小回退区间
            noiseReachedMax = false;
        }
    }

    /// <summary>
    /// 所有探测器都用这个接口加 / 减噪声（amount 可以是负数）
    /// </summary>
    public void AddNoise(float amount)
    {
        noise += amount;
    }
}
