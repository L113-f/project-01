using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public bool hasActivated = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 如果已经激活，直接拦截，不执行任何逻辑
        if (hasActivated) return;

        // 2. 获取碰撞体关联的“附加刚体”
        // 无论你的红斗篷、头部还是脚部碰撞，只要它们是 Player 的子物体，
        // collision.attachedRigidbody 拿到的都是父物体上那个唯一的 Rigidbody2D。
        Rigidbody2D rb = collision.attachedRigidbody;

        // 3. 只有当刚体存在，且刚体所在的物体标签是 Player 时才触发
        if (rb != null && rb.CompareTag("Player"))
        {
            // 4. 立即锁定，确保这一份脚本实例永远不会再次进入这个 if
            hasActivated = true;

            // 执行存档逻辑
            if (CheckPointManager.Instance != null)
            {
                CheckPointManager.Instance.UpdateCheckPoint(transform.position);
            }
        }
    }
}