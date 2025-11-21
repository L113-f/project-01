using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] public float moveSpeed = 4f;
    [SerializeField] public float walkSpeed= 2f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;     // 放在脚底
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float inputX;
    private bool isGrounded;
    private bool facingRight = true;
    public bool canMove ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    private void Update()
    {
        // 左右移动输入
        inputX = Input.GetAxisRaw("Horizontal");
        if (!canMove)
        {
            inputX = 0f;
            return;
        }

        // 跳跃（仅在落地时）
        if (Input.GetButtonDown("Jump") && IsOnGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f); // 防止叠加
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 翻转朝向（可选）
        if (inputX > 0.01f && !facingRight) Flip();
        else if (inputX < -0.01f && facingRight) Flip();
    }

    private void FixedUpdate()
    {
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
