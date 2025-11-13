using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownElevator : MonoBehaviour
{
    [Header("路径点")]
    public Transform topPoint;         // 顶点
    public Transform bottomPoint;      // 底点

    [Header("运动参数")]
    public float speed = 2f;           // 速度

    [Header("启动方式")]
    public Lever2D lever;              // 拉杆（拉一次开始上升）
    public string playerTag = "Player";

    private Rigidbody2D rb;

    private enum State
    {
        IdleAtBottom,   // 在底部等待拉杆
        MovingUp,       // 向上
        WaitPlayerOnTop,// 在顶部等待玩家站上来
        MovingDown,     // 向下
        Finished        // 到达底部后结束
    }

    private State state = State.IdleAtBottom;
    private bool playerOnPlatform;     // 通过触发器检测玩家是否在平台上

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Start()
    {
        if (!topPoint || !bottomPoint)
        {
            Debug.LogError("[UpDownElevator] 需要设置 topPoint 和 bottomPoint");
            enabled = false;
            return;
        }

        // 开始时强制在底部
        transform.position = bottomPoint.position;
        state = State.IdleAtBottom;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.IdleAtBottom:
                // 等待拉杆被触发
                if (lever && lever.triggered)
                {
                    state = State.MovingUp;
                }
                break;

            case State.MovingUp:
                MoveTowards(topPoint.position, State.WaitPlayerOnTop);
                break;

            case State.WaitPlayerOnTop:
                // 在顶部静止，直到玩家站上平台
                if (playerOnPlatform)
                {
                    state = State.MovingDown;
                }
                break;

            case State.MovingDown:
                MoveTowards(bottomPoint.position, State.Finished);
                break;

            case State.Finished:
                // 全流程结束，啥也不干（需要重复的话可以自己再改）
                break;
        }
    }

    /// <summary>
    /// 通用移动：朝 targetPos 前进，到了就切状态
    /// </summary>
    void MoveTowards(Vector3 targetPos, State nextState)
    {
        Vector3 cur = transform.position;
        Vector3 next = Vector3.MoveTowards(cur, targetPos, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        if ((next - targetPos).sqrMagnitude < 0.0001f)
        {
            transform.position = targetPos;
            state = nextState;
        }
    }

    // 需要在平台上加一个触发器（额外的 BoxCollider2D，勾 isTrigger），
    // 覆盖玩家站立区域，用它来判断玩家是否在平台上
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerOnPlatform = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerOnPlatform = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (topPoint && bottomPoint)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(topPoint.position, bottomPoint.position);
            Gizmos.DrawSphere(topPoint.position, 0.05f);
            Gizmos.DrawSphere(bottomPoint.position, 0.05f);
        }
    }
}
