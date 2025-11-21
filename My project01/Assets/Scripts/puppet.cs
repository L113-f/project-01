using System.Collections;
using UnityEngine;

public class puppet : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D playerRb;

    [Header("木头人节奏")]
    public float backDuration = 3f;   
    public float watchDuration = 2f;   

    [Header("旋转")]
    public float frontY = 0f;          
    public float backY = 180f;        
    public float turnDuration = 0.5f;  

    [Header("判定")]
    public float moveThreshold = 0.1f; 

    private bool isWatching = false;   
    private float timer;
    public bool isDetected;            

    private Coroutine turnRoutine;   

    void Start()
    {
      
        SetInstant(backY);
        timer = backDuration;
    }

    void Update()
    {
     
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ToggleState();
        }

        // 在“看你”状态 + 玩家在检测范围内 → 检测是否在动
        if (isWatching && isDetected && playerRb != null)
        {
            float speed = playerRb.velocity.magnitude;
            if (speed > moveThreshold)
            {
                var mgr = NoiseManager.Instance;
                if (mgr)
                {
                    mgr.noise = mgr.maxNoise;
                }
                Debug.Log("木头人转身时你动了 → 噪声拉满！");
            }
        }
    }

    void ToggleState()
    {
        isWatching = !isWatching;

        
        float targetY = isWatching ? frontY : backY;
        float nextDuration = isWatching ? watchDuration : backDuration;

        if (turnRoutine != null)
            StopCoroutine(turnRoutine);
        turnRoutine = StartCoroutine(RotateTo(targetY, turnDuration));

        timer = nextDuration;
    }

    IEnumerator RotateTo(float targetY, float duration)
    {
        float elapsed = 0f;
        Vector3 startEuler = transform.localEulerAngles;

     
        float startY = startEuler.y;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newY = Mathf.LerpAngle(startY, targetY, t);
            Vector3 e = transform.localEulerAngles;
            e.y = newY;
            transform.localEulerAngles = e;
            yield return null;
        }

      
        Vector3 finalEuler = transform.localEulerAngles;
        finalEuler.y = targetY;
        transform.localEulerAngles = finalEuler;
    }

    
    void SetInstant(float yAngle)
    {
        Vector3 e = transform.localEulerAngles;
        e.y = yAngle;
        transform.localEulerAngles = e;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isDetected = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isDetected = false; 
    }
}
