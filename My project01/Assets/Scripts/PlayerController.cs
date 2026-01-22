using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] public float moveSpeed = 4f; 
    [SerializeField] public float walkSpeed = 2f; 

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float windJumpMultiplier = 1.3f; 

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wind Effect")]
    [SerializeField] private float windHorizontalBonus = 2f; 
    [SerializeField] private float windExitTolerance = 0.3f; // 离开风区后保持状态的缓冲时间

    private bool isInWind = false; 
    private float windTimer; 

    private Rigidbody2D rb;
    private Animator anim;
    private float inputX;
    private bool isGrounded;
    private bool facingRight = true;
    public bool canMove;

    private MovingPlatform2D currentPlatform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = true;
    }

    private void Update()
    {
        isGrounded = IsOnGround();
        inputX = canMove ? Input.GetAxisRaw("Horizontal") : 0f;

        // 容错计时器逻辑
        if (windTimer > 0)
        {
            windTimer -= Time.deltaTime;
            isInWind = true;
        }
        else
        {
            isInWind = false;
        }

        if (!canMove)
            rb.velocity = new Vector2(0f, rb.velocity.y);

        // 跳跃逻辑
        if (canMove && Input.GetButtonDown("Jump") && (isGrounded || isInWind))
        {
            float finalJumpForce = jumpForce;
            Vector2 finalJumpDir = Vector2.up;

            if (isInWind)
            {
                finalJumpForce *= windJumpMultiplier;
                finalJumpDir += new Vector2(inputX * 0.4f, 0); 
                windTimer = 0; // 跳跃瞬间结束风场状态
            }

            rb.velocity = new Vector2(rb.velocity.x, 0f); 
            rb.AddForce(finalJumpDir.normalized * finalJumpForce, ForceMode2D.Impulse);
            isGrounded = false; 
        }

        if (inputX > 0.01f && !facingRight) Flip();
        else if (inputX < -0.01f && facingRight) Flip();

        // 动画控制
        if (anim != null)
        {
            bool isMoving = Mathf.Abs(inputX) > 0.01f && isGrounded && canMove;
            bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

            anim.SetBool("IsRunning", isRunning);
            anim.SetBool("IsWalking", isMoving && !isRunning);

            // 优先判断是否在风中，播放漂浮动画
            anim.SetBool("IsInWind", isInWind);
            anim.SetBool("IsJumping", !isGrounded && !isInWind);
        }
    }

    private void FixedUpdate()
    {
        float baseSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed : walkSpeed;
        float speed = isInWind ? baseSpeed + windHorizontalBonus : baseSpeed;
        float vx = inputX * speed;

        if (currentPlatform != null && isGrounded)
        {
            vx += currentPlatform.PlatformVelocity.x;
        }

        // 仅在水平方向控制速度，垂直方向完全交给物理引擎（Area Effector）
        rb.velocity = new Vector2(vx, rb.velocity.y);
        
        if (!isInWind && rb.drag > 0)
        {
            rb.drag = Mathf.MoveTowards(rb.drag, 0, Time.fixedDeltaTime * 2f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Wind")) 
        {
            windTimer = windExitTolerance; // 只要在区域内，就刷新计时器
            rb.drag = 1.0f; // 保持轻微空气阻力，让悬停更稳
        }
    }

    public bool IsOnGround()
    {
        if (!groundCheck) return false;
        // 在风中时，只要有向上的趋势，就认为不在地面
        if (isInWind && rb.velocity.y > 0.01f) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        var platform = col.collider.GetComponent<MovingPlatform2D>();
        if (!platform) return;

        for (int i = 0; i < col.contactCount; i++)
        {
            if (col.GetContact(i).normal.y > 0.5f)
            {
                currentPlatform = platform;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        var platform = col.collider.GetComponent<MovingPlatform2D>();
        if (platform && currentPlatform == platform)
            currentPlatform = null;
    }
}