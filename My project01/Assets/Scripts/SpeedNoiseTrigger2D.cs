using UnityEngine;


public class SpeedNoiseTrigger2D : MonoBehaviour
{
 
    public string playerTag = "Player";      
    public float noisePerSpeed = 1f;        
    public float minSpeedThreshold = 0.1f;   

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (NoiseManager.Instance == null) return;

        
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        float speed = rb.velocity.magnitude;  

        if (speed < minSpeedThreshold) return; 

      
        float noiseToAdd = speed * noisePerSpeed * Time.deltaTime;

        NoiseManager.Instance.AddNoise(noiseToAdd);
    }
}
