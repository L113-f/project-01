using UnityEngine;

public class RabbitListener : MonoBehaviour
{
      
    
    public float chaseNoiseThreshold = 4f;

   
    public float stopNoiseThreshold = 2f;

    [Header("追踪参数")]
    public float moveSpeed = 3f;          
    public float maxChaseDistance = 15f;  
    public Transform player;            

    private Animator anim;
    private Rigidbody2D rb;
    private bool isChasing = false;
    private float defaultScaleX;         

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        defaultScaleX = transform.localScale.x;
    }

    private void Update()
    {
        if (NoiseManager.Instance == null || player == null) return;

        float noise = NoiseManager.Instance.currentNoise;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ---- 状态切换 ----
        if (!isChasing && noise >= chaseNoiseThreshold)
        {
            StartChase();
        }
        else if (isChasing && (noise <= stopNoiseThreshold || distanceToPlayer > maxChaseDistance))
        {
            StopChase();
        }
    }

    private void FixedUpdate()
    {
        if (!isChasing || player == null) return;

        Vector2 currentPos = rb.position;
        Vector2 targetPos = player.position;

        
        targetPos.y = currentPos.y;

        Vector2 dir = (targetPos - currentPos).normalized;
        Vector2 newPos = currentPos + dir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        // 左右翻转
        if (dir.x > 0.01f)
            SetFacing(1);
        else if (dir.x < -0.01f)
            SetFacing(-1);
    }

    void StartChase()
    {
        isChasing = true;
        if (anim) anim.SetBool("IsChasing", true);   
    }

    void StopChase()
    {
        isChasing = false;
        if (anim) anim.SetBool("IsChasing", false);  
        rb.velocity = Vector2.zero;
    }

    void SetFacing(int dir)
    {
        // dir = 1 向右，dir = -1 向左
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(defaultScaleX) * dir;
        transform.localScale = scale;
    }
}
