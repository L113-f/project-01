using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
  
    public Lever2D lever;            

   
    public float liftDistance = 3f;    
    public float speed = 3f;          
    public bool disableColliderWhenOpen = true;

    Rigidbody2D rb;
    Collider2D col;
    Vector3 closedPos, openPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;                 
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; 
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        closedPos = transform.position;
        openPos   = closedPos + Vector3.up * liftDistance;
    }

    void FixedUpdate()
    {

        bool wantOpen = lever && lever.triggered;

        Vector3 target = wantOpen ? openPos : closedPos;
        Vector3 next   = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        if (disableColliderWhenOpen && col)
        {
            bool atOpen = (next - openPos).sqrMagnitude < 0.0001f;
            col.enabled = !atOpen;
        }
    }
   
}
