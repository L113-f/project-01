using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseDetector : MonoBehaviour
{
    
    [Header("Player Reference")]
    public Transform player;
    public Rigidbody2D rb;
    public bool isPlayerInside;
    public float currentSpeed; 
    

    [Header("Noise Elements")]
    public float noise = 0f;
    public float maxNoise = 100f;
    public float gainPerSpeed = 1f;
    public float speedThreshold = 0.1f; 
    public float decayRate = 5f;

    [Header("UI")]
    public Slider noiseSlider;


    void Update()
    {
        float time = Time.deltaTime;
        if (!NoiseManager.Instance) return;
        if(isPlayerInside)
        {
            currentSpeed = rb.velocity.magnitude;
        }
        else
        {
            currentSpeed = 0f;
        }

        if(isPlayerInside && currentSpeed > speedThreshold)
        {
            noise += currentSpeed * gainPerSpeed * time;
        }
        else
        {
            noise -= decayRate * time;
        }

        noise = Mathf.Clamp(noise, 0f, maxNoise);

        if (Mathf.Abs(noise) > 0f)
        {
            NoiseManager.Instance.AddNoise(noise);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInside = false;
    }
    
    
        
    

    
    
}
