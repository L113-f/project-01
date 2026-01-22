using UnityEngine;

public class Detect : MonoBehaviour
{
    [Header("遮挡检测起点（摄像头/灯的眼睛位置）")]
    public Transform startPoint;

    [Header("玩家Tag（建议打在玩家根物体）")]
    public string playerTag = "Player";

    [Header("遮挡层（墙/石头/地形等），不要包含 Player 层）")]
    public LayerMask obstacleMask;

    [Header("射线起点偏移：避免射线一开始就打到自己")]
    public float offset = 0.03f;

    [Header("玩家身体点（挂在玩家身上的空物体：Head/Chest/Legs...）")]
    public Transform[] bodyPoints;

    [Header("探测区 Collider（不填就自动用本物体的 Collider2D，必须是 Trigger）")]
    public Collider2D detectArea;

    void Awake()
    {
        if (!detectArea) detectArea = GetComponent<Collider2D>();
        if (!startPoint) startPoint = transform;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // ✅更稳：子碰撞体也能识别玩家
        if (!other.transform.root.CompareTag(playerTag)) return;

        // ✅任意一个点：在光圈里 + 无遮挡 -> GG（更符合“被照到就死”）
        if (AnyPointSeen())
        {
            GameOver();
        }
    }

    bool AnyPointSeen()
    {
        if (!detectArea || !startPoint || bodyPoints == null || bodyPoints.Length == 0)
            return false;

        Vector2 origin = startPoint.position;

        foreach (var pt in bodyPoints)
        {
            if (!pt) continue;

            // 点必须在探测区里
            if (!detectArea.OverlapPoint(pt.position))
                continue;

            // 点到灯之间必须无遮挡
            if (!IsBlocked(origin, pt.position))
                return true;
        }

        return false;
    }

    bool IsBlocked(Vector2 origin, Vector2 target)
    {
        Vector2 dir = target - origin;
        float dist = dir.magnitude;
        if (dist <= 0.0001f) return false;

        Vector2 start = origin + dir.normalized * offset;

        RaycastHit2D hit = Physics2D.Raycast(start, dir.normalized, dist, obstacleMask);
        return hit.collider != null;
    }

    void GameOver()
    {
        Debug.Log("game over");
        // TODO: 在这里调用你的死亡/重开关卡逻辑
        // 例如：GameManager.Instance.GameOver();
        enabled = false; // ✅防止同一帧疯狂重复触发（可选）
    }
}
