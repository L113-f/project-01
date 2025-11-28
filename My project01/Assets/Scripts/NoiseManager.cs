using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance { get; private set; }

    [Header("噪声参数")]
    public float currentNoise = 0f;   
    public float maxNoise = 10f;     
    public float decayPerSecond = 1f; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
     
    }

    private void Update()
    {
        if (currentNoise <= 0f) return;

        currentNoise -= decayPerSecond * Time.deltaTime;
        if (currentNoise < 0f) currentNoise = 0f;
    }

  
    public void AddNoise(float amount)
    {
        if (amount <= 0f) return;
        currentNoise = Mathf.Clamp(currentNoise + amount, 0f, maxNoise);
       
    }
}
