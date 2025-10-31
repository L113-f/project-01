using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MoveablePlatform : MonoBehaviour
{
    //public Transform leftPoint;
    //public Transform rightPoint;

    public float totalWidth = 6f;
    public float speed = 2.5f;
    public float waitDur = .2f;
    public bool startLeft = true; // 默认向左

    public Rigidbody2D rb;
    public Vector3 leftWorld;
    public Vector3 rightWorld;

    private Vector3 target;
    private float wait;
    private int dir;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        float half = totalWidth * .5f;
        leftWorld = transform.position + Vector3.left * half;
        rightWorld = transform.position + Vector3.right * half;

        transform.position = startLeft ? leftWorld : rightWorld;
        dir = startLeft ? 1 : -1;
        target = (dir > 0) ? rightWorld : leftWorld;

    }


    void FixedUpdate()
    {
        if (wait > 0)
        {
            wait -= Time.fixedDeltaTime;
            return;
        }

        Vector3 cur = transform.position;
        Vector3 next = Vector3.MoveTowards(cur, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        if ((next - target).sqrMagnitude < 0.0001f)
        {
            dir *= -1;
            target = (dir > 0) ? rightWorld : leftWorld;
            wait = waitDur;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = this.transform;
        }
    }






}
