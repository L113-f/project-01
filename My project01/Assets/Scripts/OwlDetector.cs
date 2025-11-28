using System.Collections;
using UnityEngine;

public class OwlDetector : MonoBehaviour
{
    public Transform player;

    public Lever2D lever;      // 在 Inspector 里把拉杆拖进来

    public Animator anim;

    public float startDelay = 1f;

    public float moveSpeed = 3f;

    public float maxChaseDistance = 20f;
    public float stopDistance = 1.0f;   // 和玩家保持的最小距离

    private Rigidbody2D rb;
    private bool isChasing = false;
    private bool hasStartedCoroutine = false;
    private float defaultScaleX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        defaultScaleX = transform.localScale.x;
    }

    private void Update()
    {
        if (lever == null || player == null)
            return;

        // 拉杆第一次被拉下（triggered 从 false -> true）
        if (lever.triggered && !hasStartedCoroutine)
        {
            hasStartedCoroutine = true;
            StartCoroutine(StartChaseAfterDelay());
        }

        // 你可以保留这段，也可以删掉，因为 FixedUpdate 里也会判断
        if (isChasing)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist > maxChaseDistance)
            {
                isChasing = false;
                if (anim != null)
                {
                    anim.SetBool("IsChasing", false);
                }
            }
        }
    }

    private IEnumerator StartChaseAfterDelay()
    {
        // 延迟 startDelay 秒
        yield return new WaitForSeconds(startDelay);

        isChasing = true;
        if (anim != null)
        {
            anim.SetBool("IsChasing", true); // Animator 里用这个切到追踪动画
        }
    }

    private void FixedUpdate()
    {
        if (!isChasing || player == null)
            return;

        Vector2 currentPos = rb.position;
        Vector2 toPlayer = (Vector2)player.position - currentPos;
        float dist = toPlayer.magnitude;

        // 超出最大追踪距离：停追并退出
        if (dist > maxChaseDistance)
        {
            isChasing = false;
            if (anim != null)
            {
                anim.SetBool("IsChasing", false);
            }
            return;
        }

        // 防止完全重合导致方向归一化出问题
        if (dist < 0.001f)
            return;

        // 指向玩家的单位方向
        Vector2 dir = toPlayer / dist;

        // 本帧理论上能走的步长
        float step = moveSpeed * Time.fixedDeltaTime;

        // 如果这一小步会冲进 stopDistance 以内，就缩短步长
        if (dist - step < stopDistance)
        {
            step = dist - stopDistance;
            if (step < 0f) step = 0f;  // 已经比 stopDistance 还近，就别动了
        }

        Vector2 newPos = currentPos + dir * step;
        rb.MovePosition(newPos);

        // 左右翻转
        if (dir.x > 0.01f)
            SetFacing(1);
        else if (dir.x < -0.01f)
            SetFacing(-1);
    }

    private void SetFacing(int dir)
    {
        // dir = 1 向右，dir = -1 向左
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(defaultScaleX) * dir;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
    
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxChaseDistance);
    }

}
