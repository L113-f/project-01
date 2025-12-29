using UnityEngine;

public class Detect : MonoBehaviour
{
    public Transform startPoint;
    public string PlayerTag = "Player";

    [Header("遮挡层（墙/地形等），不要包含 Player 层）")]
    public LayerMask Layers;

    public float offset = 0.03f;
    public bool once = false;
    public bool locked;

    [Header("玩家身体点（挂在玩家身上的空物体：Head/Chest/Legs...）")]
    public Transform[] bodyPoints;

    [Header("探测区 Collider（不填就自动用本物体的 Collider2D，必须是 Trigger）")]
    public Collider2D detectArea;

    void Awake()
    {
        if (!detectArea) detectArea = GetComponent<Collider2D>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (locked && once) return;
        if (!collision.CompareTag(PlayerTag)) return;

        // ✅全部点：在探测区内 + 无遮挡  才死亡
        if (AllPointsHaveLineOfSight())
        {
            locked = true;
            Debug.Log("game over");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!once && other.CompareTag(PlayerTag))
            locked = false;
    }

    bool AllPointsHaveLineOfSight()
    {
        if (startPoint == null) return false;
        if (detectArea == null) return false;
        if (bodyPoints == null || bodyPoints.Length == 0) return false;

        for (int i = 0; i < bodyPoints.Length; i++)
        {
            var pt = bodyPoints[i];

            // ✅更严格：点没绑就直接不通过，避免“只剩一个点也会死”
            if (pt == null) return false;

            // ✅点必须在探测区里（光圈里）
            if (!detectArea.OverlapPoint(pt.position))
                return false;

            // ✅点到灯之间必须无遮挡
            if (!HasLineOfSightToPoint(pt.position))
                return false;
        }

        return true;
    }

    bool HasLineOfSightToPoint(Vector2 point)
    {
        Vector2 origin = startPoint.position;
        Vector2 dir = point - origin;
        float dist = dir.magnitude;

        if (dist <= 0.0001f) return true;

        Vector2 start = origin + dir.normalized * offset;

        RaycastHit2D hit = Physics2D.Raycast(start, dir.normalized, dist, Layers);

        return hit.collider == null;
    }
}
