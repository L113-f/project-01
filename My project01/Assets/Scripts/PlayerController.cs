using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] public float moveSpeed = 4f; // 跑步速度（Shift）
    [SerializeField] public float walkSpeed = 2f; // 正常走路速度

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;     // 放在脚底
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private float inputX;
    private bool isGrounded;
    private bool facingRight = true;
    public bool canMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = true;
    }

    private void Update()
    {
        // ===== 地面检测（只算一次） =====
        isGrounded = IsOnGround();

        // ===== 水平输入 =====
        inputX = canMove ? Input.GetAxisRaw("Horizontal") : 0f;
        if(!canMove)
            rb.velocity = new Vector2(0f, 0f);

        // ===== 跳跃（仅在落地时） =====
        if (canMove && Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f); // 防止叠加
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false; // 刚起跳立即视为离地
        }

        // ===== 翻转朝向 =====
        if (inputX > 0.01f && !facingRight) Flip();
        else if (inputX < -0.01f && facingRight) Flip();

        // ===== 动画状态：走路 / 跑步 / 跳跃 / Idle =====
        if (anim != null)
        {
            bool isMoving = Mathf.Abs(inputX) > 0.01f && isGrounded && canMove;
            bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);  // 按住 Shift = 跑步

            // 跑步 / 走路 只在地面上才生效
            anim.SetBool("IsRunning", isRunning);
            anim.SetBool("IsWalking", isMoving && !isRunning);

            // 跳跃：只要不在地面上就认为在空中（简单版）
            bool isJumping = !isGrounded;
            anim.SetBool("IsJumping", isJumping);
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        // Shift 控制速度：跑/走
        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed : walkSpeed;
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
    }

    public bool IsOnGround()
    {
        if (!groundCheck) return false;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        return isGrounded;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    private void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
