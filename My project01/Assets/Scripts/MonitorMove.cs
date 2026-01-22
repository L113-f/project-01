using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorMove : MonoBehaviour
{
    [Header("巡逻点（只在Start读取一次）")]
    public Transform pointA;
    public Transform pointB;

    [Header("移动参数")]
    public float speed = 2.5f;
    public float waitAtEnds = 0.2f;
    public float arriveEpsilon = 0.01f;

    float minX, maxX;   // 缓存后的范围
    float fixedY, fixedZ;
    int dir = 1;        // 1往右，-1往左
    float waitTimer;

    void Start()
    {
        if (!pointA || !pointB)
        {
            Debug.LogError("PatrolX_CachePointsOnce: pointA/pointB 没有赋值");
            enabled = false;
            return;
        }

        // ✅ 只在这里读一次两点的X（世界坐标）
        float xA = pointA.position.x;
        float xB = pointB.position.x;
        minX = Mathf.Min(xA, xB);
        maxX = Mathf.Max(xA, xB);

        // ✅ 只沿X动：锁住当前Y/Z
        fixedY = transform.position.y;
        fixedZ = transform.position.z;

        // 初始方向：靠近哪端就先往另一端走（更像巡逻）
        float x = transform.position.x;
        float distToMin = Mathf.Abs(x - minX);
        float distToMax = Mathf.Abs(x - maxX);
        dir = (distToMin < distToMax) ? 1 : -1;

        // 可选：把自己夹进范围里，避免一开始就在外面
        float clampedX = Mathf.Clamp(x, minX, maxX);
        transform.position = new Vector3(clampedX, fixedY, fixedZ);
    }

    void Update()
    {
        if (waitTimer > 0f) { waitTimer -= Time.deltaTime; return; }

        float x = transform.position.x + dir * speed * Time.deltaTime;

        // ✅ 永远不越界（严格在两点之间）
        x = Mathf.Clamp(x, minX, maxX);

        transform.position = new Vector3(x, fixedY, fixedZ);

        bool atMin = Mathf.Abs(x - minX) <= arriveEpsilon;
        bool atMax = Mathf.Abs(x - maxX) <= arriveEpsilon;

        if (atMin || atMax)
        {
            if (waitAtEnds > 0f) waitTimer = waitAtEnds;
            dir *= -1;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!pointA || !pointB) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointA.position, pointB.position);
        Gizmos.DrawWireSphere(pointA.position, 0.08f);
        Gizmos.DrawWireSphere(pointB.position, 0.08f);
    }
#endif
}
