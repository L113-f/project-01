using UnityEngine;

public class MonitorTriggerZone : MonoBehaviour
{
    [Header("遮挡检测起点（摄像头眼睛位置）")]
    public Transform eyePoint;

    [Header("玩家Tag")]
    public string playerTag = "Player";

    [Header("石头/墙层")]
    public LayerMask obstacleMask;

    void Reset()
    {
        // 自动把自己设为Trigger（防呆）
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // ✅ 用Tag判定（用root防止子物体Collider没tag）
        if (!other.transform.root.CompareTag(playerTag)) return;

        if (!eyePoint) eyePoint = transform; // 没配就用自己

        // 取玩家根物体（通常就是你的Player对象）
        Transform playerRoot = other.transform.root;

        // ✅ 进入范围内了，检查是否被石头挡住
        bool blocked = IsFullyBlocked(playerRoot);

        if (!blocked)
        {
            Debug.Log("GG");
            // TODO: 这里调用你的GameOver/重置逻辑
        }
    }

    bool IsFullyBlocked(Transform playerRoot)
    {
        Vector2 origin = eyePoint.position;

        // 优先拿玩家身上的Collider2D，用bounds取头/身/脚
        Collider2D col = playerRoot.GetComponent<Collider2D>();
        if (col != null)
        {
            Bounds b = col.bounds;
            Vector2 top = new Vector2(b.center.x, b.max.y);
            Vector2 mid = b.center;
            Vector2 bot = new Vector2(b.center.x, b.min.y);

            bool topBlocked = Physics2D.Linecast(origin, top, obstacleMask);
            bool midBlocked = Physics2D.Linecast(origin, mid, obstacleMask);
            bool botBlocked = Physics2D.Linecast(origin, bot, obstacleMask);

            return topBlocked && midBlocked && botBlocked;
        }

        // 如果玩家根节点没Collider，就退化成用位置点
        return Physics2D.Linecast(origin, (Vector2)playerRoot.position, obstacleMask);
    }
}
