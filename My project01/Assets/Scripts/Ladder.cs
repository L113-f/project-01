using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private float vertical;
    public float speed = 8f;
    public bool isLadder;
    public bool isClimbing;

    public Rigidbody2D rb;



    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;

        }

    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 3f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            isLadder = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            isLadder = false;
        }
    }
}