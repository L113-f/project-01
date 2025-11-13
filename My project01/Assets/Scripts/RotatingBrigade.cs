using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBrigade : MonoBehaviour
{

    public Lever2D lever2D;
    public float motorSpeed = 10f;
  
    private HingeJoint2D joint;

    private Rigidbody2D rb;
    private bool onMotor;
    private bool stopped;

  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<HingeJoint2D>();

        joint.useMotor = false;
        rb.angularVelocity = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;

       
        
    }

   
    void Update()
    {
        if (stopped) return;

        if (lever2D.triggered && !onMotor)
        {
            StartRotating();
        }
        else if(!lever2D.triggered && onMotor)
        {
            StopRotating();
        }
    }

    public void StartRotating()
    {



        if (rb.bodyType != RigidbodyType2D.Dynamic)
            rb.bodyType = RigidbodyType2D.Dynamic;

        rb.freezeRotation = false;
        
        

        var m = joint.motor;
        m.motorSpeed = motorSpeed;
        joint.motor = m;

        joint.useMotor = true;
        onMotor = true;

       
    }

    public void StopRotating()
    {
        joint.useMotor = false;
        rb.angularVelocity = 0;
        rb.freezeRotation = true;
        rb.velocity = Vector2.zero;
        if (rb.bodyType != RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Kinematic;

        onMotor = false;

       

    }

 
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Wall"))
        {
            StopAndSettle();
            stopped = true;
        }
           
    }

    private void StopAndSettle()
    {
        joint.useMotor = false;
        rb.angularVelocity = 0f;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;
        onMotor = false;
        
    }
}
