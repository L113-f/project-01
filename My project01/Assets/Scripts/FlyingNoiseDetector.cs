using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingNoiseDetector : MonoBehaviour
{

    public Lever2D lever;
    public Transform player;
    public Rigidbody2D playerRb;


    public float chaseRadius = 8f;      
    public float stopDistance = 1.0f;   
    public bool returnToStart = true;  
    public float returnSpeed = 2f;      
    public float chaseSpeed = 3f; 
    
    public float noise = 0f;           
    public float maxNoise = 100f;       
    public float gainPerSpeed = 1f;    
    public float speedThreshold = 0.1f; 
    public float decayRate = 5f;  

    public Slider noiseSlider;

    public bool isPlayerInside;   
    public float currentSpeed;          
    public float distToPlayer;

    private Rigidbody2D rb;
    private Vector3 startPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    void Start()
    {
        startPos = transform.position; 
    }

    void Update()
    {
        float time = Time.deltaTime;
        if (!NoiseManager.Instance) return;
        
        if(isPlayerInside)
        {
            currentSpeed = playerRb.velocity.magnitude;
        }
        else
        {
            currentSpeed = 0f;
        }

        float deltaNoise = 0f;


        if(isPlayerInside && currentSpeed > speedThreshold)
        {
            deltaNoise += currentSpeed * gainPerSpeed * time;
        }
        else
        {
            deltaNoise -= decayRate * time;
        }

        

        if (Mathf.Abs(deltaNoise) > 0f)
        {
            NoiseManager.Instance.AddNoise(deltaNoise);
        }


    }

    void FixedUpdate()
    {
        
        if(!lever.triggered) return;

        Vector2 pos = transform.position;
        Vector2 playerPos = player.position;
        distToPlayer = Vector2.Distance(pos, playerPos);

        
        if (distToPlayer <= chaseRadius)
        {
            
            if (distToPlayer > stopDistance)
            {
                MoveTowards(playerPos, chaseSpeed);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
          
            if (returnToStart)
            {
                float distToStart = Vector2.Distance(pos, startPos);
                if (distToStart > 0.05f)
                    MoveTowards(startPos, returnSpeed);
                else
                    rb.velocity = Vector2.zero;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    void MoveTowards(Vector3 targetPos, float speed)
    {
        if (speed <= 0f) return;

        Vector3 cur = transform.position;
        Vector3 dir = (targetPos - cur).normalized;
        Vector3 next = cur + dir * speed * Time.fixedDeltaTime;
        rb.MovePosition(next);
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

     void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

       
        Collider2D col = GetComponent<Collider2D>();
        if (col is CircleCollider2D circle)
        {
            Gizmos.color = Color.cyan;
            float r = circle.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
            Gizmos.DrawWireSphere(transform.position, r);
        }
    }


}
