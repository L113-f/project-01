using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public float leftAngle;
    public float rightAngle;

   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = moveSpeed;
    }

    void Update()
    {
        Push();
    }
    public void Push()
    {
        if (transform.rotation.z > 0 && transform.rotation.z < rightAngle && rb.angularVelocity > 0 && rb.angularVelocity < moveSpeed)
        {
            rb.angularVelocity = moveSpeed;
        }
        else if (transform.rotation.z < 0 && transform.rotation.z > leftAngle && rb.angularVelocity < 0 && rb.angularVelocity > moveSpeed * -1)
        {
            rb.angularVelocity = moveSpeed * -1;           
        }
    }

   
    
}
