using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform2D : MonoBehaviour
{
    [Header("路径点（把两个空物体拖进来）")]
    public Transform pointA;
    public Transform pointB;

    [Header("移动参数")]
    public float speed = 2f;
    public float arriveThreshold = 0.02f;
    public float waitTimeAtEnds = 0f; // 0=不停顿

    [Header("能被平台带着走的对象层（一般选 Player）")]
    public LayerMask passengerMask;

    [Header("是否由平台脚本直接推乘客移动（不推荐；角色控制器常会覆盖位移）")]
    public bool carryPassengersByPlatform = false;

    // ✅ 推荐用法：玩家脚本读取这两个值来叠加移动
    public Vector2 PlatformDelta { get; private set; }
    public Vector2 PlatformVelocity { get; private set; }

    Rigidbody2D rb;
    Vector2 target;
    bool waiting;

    readonly HashSet<Transform> passengers = new HashSet<Transform>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        if (pointA != null && pointB != null)
            target = pointB.position; // 默认从A走向B
    }

    void FixedUpdate()
    {
        if (!pointA || !pointB || waiting)
        {
            PlatformDelta = Vector2.zero;
            PlatformVelocity = Vector2.zero;
            return;
        }

        Vector2 current = rb.position;
        Vector2 next = Vector2.MoveTowards(current, target, speed * Time.fixedDeltaTime);

        // ✅ 平台本帧位移（最可靠：用 current/next 直接算，不依赖 transform 的刷新时机）
        PlatformDelta = next - current;
        PlatformVelocity = PlatformDelta / Time.fixedDeltaTime;

        rb.MovePosition(next);

        // （可选）平台直接推乘客：不推荐；如果你玩家脚本也在叠加 delta 会“双倍位移”
        if (carryPassengersByPlatform && PlatformDelta != Vector2.zero)
        {
            foreach (var t in passengers)
            {
                if (!t) continue;

                var prb = t.GetComponent<Rigidbody2D>();
                if (prb && prb.bodyType == RigidbodyType2D.Dynamic)
                    prb.MovePosition(prb.position + PlatformDelta);
                else
                    t.position += (Vector3)PlatformDelta;
            }
        }

        // 到端点：换方向（可停顿）
        if (Vector2.Distance(next, target) <= arriveThreshold)
        {
            target = (Vector2.Distance(target, pointA.position) < 0.001f)
                ? (Vector2)pointB.position
                : (Vector2)pointA.position;

            if (waitTimeAtEnds > 0f)
                StartCoroutine(WaitAtEnd());
        }
    }

    System.Collections.IEnumerator WaitAtEnd()
    {
        waiting = true;
        // 等待期间平台不动，所以 delta/velocity 会在 FixedUpdate 里被清零
        yield return new WaitForSeconds(waitTimeAtEnds);
        waiting = false;
    }

    void OnCollisionEnter2D(Collision2D col) => TryAddPassenger(col);
    void OnCollisionStay2D(Collision2D col) => TryAddPassenger(col);
    void OnCollisionExit2D(Collision2D col) => passengers.Remove(col.transform);

    void TryAddPassenger(Collision2D col)
    {
        if (((1 << col.gameObject.layer) & passengerMask) == 0) return;

        // 只在“站在平台上方”时才算乘客（看法线）
        for (int i = 0; i < col.contactCount; i++)
        {
            var n = col.GetContact(i).normal;
            if (n.y > 0.5f)
            {
                passengers.Add(col.transform);
                return;
            }
        }
    }
}
