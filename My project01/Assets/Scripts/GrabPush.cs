using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GrabPush : MonoBehaviour
{

    public PlayerController playerController;
    public LayerMask pushableMask;
    public Transform hand;
    public float grabRange = 0.3f;
    FixedJoint2D joint;
    GameObject grabbedbox;
    public Rigidbody2D rb;

   

    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            TryGrab();
            rb.mass = 1;
            
        }
        else if (Input.GetKeyUp(KeyCode.F) )
        {
            Release();
            rb.mass = 100;
        }
    }

    public void TryGrab()
    {
        Collider2D col = Physics2D.OverlapCircle(hand.position, grabRange, pushableMask);
        if (col && col.attachedRigidbody)
        {
            grabbedbox = col.gameObject;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
            joint = grabbedbox.GetComponent<FixedJoint2D>();
            joint.enabled = true;
            joint.connectedBody = this.gameObject.GetComponent<Rigidbody2D>();
            playerController.moveSpeed = 2.5f;
            
        }
    }

    public void Release()
    {
        if (joint == null) return;
        joint.enabled = false;
        rb.constraints = RigidbodyConstraints2D.None; 
        playerController.moveSpeed = 4f;
    }
    
    void OnDrawGizmosSelected()
    {
        if (!hand) return;
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(hand.position, grabRange);
    }
    

}
