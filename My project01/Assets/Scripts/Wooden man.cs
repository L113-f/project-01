using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodenman : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private float flipDuration = 0.5f; 
    [SerializeField] private float stayTime = 3f;      
    [SerializeField] private float movementThreshold = 0.1f; 

    private bool isWatching = false; 
    private bool isPlayerInArea = false; 
    private Rigidbody2D targetPlayerRb; 
    private float timer;
    private Quaternion targetRotation;
    private Quaternion rotBack = Quaternion.Euler(0, 0, 0);    
    private Quaternion rotFront = Quaternion.Euler(0, 180, 0); 

    void Start()
    {
        if (pivot == null) pivot = transform;
        targetRotation = rotBack;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= stayTime)
        {
            timer = 0;
            isWatching = !isWatching;
            targetRotation = isWatching ? rotFront : rotBack;
        }

        float step = (1f / flipDuration) * Time.deltaTime;
        pivot.localRotation = Quaternion.Slerp(pivot.localRotation, targetRotation, step * 5f);

        if (isPlayerInArea && isWatching && Quaternion.Angle(pivot.localRotation, rotFront) < 5f)
        {
            CheckPlayerMovement();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetPlayerRb = other.GetComponentInParent<Rigidbody2D>();
            if (targetPlayerRb != null)
            {
                isPlayerInArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = false;
            targetPlayerRb = null;
        }
    }

    void CheckPlayerMovement()
    {
        if (targetPlayerRb == null) return;

        if (targetPlayerRb.velocity.magnitude > movementThreshold)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // 这里是角色死亡的逻辑接口，可在此处调用死亡动画、UI弹出或场景重启
        Debug.Log("<color=red>检测到移动：触发死亡逻辑</color>");
    }
}