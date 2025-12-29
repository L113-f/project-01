using System.Collections;
using System.Collections.Generic;
using UnityEngine; // 移除了多余的 System.Numerics，避免与 Unity 向量冲突

public class GrabPush : MonoBehaviour
{
    public PlayerController playerController;
    public LayerMask pushableMask;
    public Transform hand;
    public float grabRange = 0.3f;
    
    // 之前这些是手动拖拽的，现在改为私有，由代码自动寻找
    FixedJoint2D joint;
    GameObject grabbedbox;
    Rigidbody2D rb; // 这里的 rb 将在检测到箱子时动态赋值为箱子的 Rigidbody

    void Start()
    {
        // 初始确保没有错误的引用
    }

    void Update()
    {
        // 保留你原本的按键逻辑
        if (Input.GetKey(KeyCode.F))
        {
            TryGrab();
            // 只有当真的抓到了箱子（rb 不为空）时才修改质量
            if (rb != null)
            {
                rb.mass = 1;
            }
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            Release();
            // 注意：Release 逻辑里已经处理了质量恢复
        }
    }

    public void TryGrab()
    {
        // 1. 动态检测：探测手部范围内是否有 pushableMask 层的物体
        Collider2D col = Physics2D.OverlapCircle(hand.position, grabRange, pushableMask);
        
        // 2. 如果检测到了碰撞体，并且该物体有 Rigidbody2D
        if (col && col.attachedRigidbody)
        {
            // 动态给原本的变量赋值，这样你就不用在 Inspector 里手动拖了
            grabbedbox = col.gameObject;
            rb = col.attachedRigidbody; 
            
            // 执行你原有的功能逻辑
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
            
            // 获取【这个特定箱子】身上的 FixedJoint2D
            joint = grabbedbox.GetComponent<FixedJoint2D>();
            
            if (joint != null)
            {
                joint.enabled = true;
                // 将箱子的关节连接到玩家身上
                joint.connectedBody = this.gameObject.GetComponent<Rigidbody2D>();
                playerController.moveSpeed = 1.5f;
            }
        }
    }

    public void Release()
    {
        // 确保有抓到东西才执行释放
        if (rb != null)
        {
            // 恢复你设定的质量 100
            rb.mass = 100;
            rb.constraints = RigidbodyConstraints2D.None;
        }

        if (joint != null)
        {
            joint.connectedBody = null;
            joint.enabled = false;
        }

        // 执行你原有的功能：恢复玩家速度
        playerController.moveSpeed = 3f;

        // 彻底清空引用，以便下次可以抓取别的箱子
        joint = null;
        rb = null;
        grabbedbox = null;
    }
    
    void OnDrawGizmosSelected()
    {
        if (!hand) return;
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(hand.position, grabRange);
    }
}