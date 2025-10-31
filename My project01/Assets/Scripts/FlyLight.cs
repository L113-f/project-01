using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyLight : MonoBehaviour
{
    public Transform leftLight;
    public Transform rightLight;
    
    
    public float speed = 3f;
    public float waitDur = .5f;


    private Vector3 leftWorld;            
    private Vector3 rightWorld; 
    private Vector3 target;
    private float wait;



    private void Start() {
        target = rightLight.position;
        transform.position = leftLight.position;
    }

    private void Update()
    {
        if (wait > 0f)
        {
            wait -= Time.deltaTime;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.02f)
        {
            target = (target == rightLight.position) ? leftLight.position : rightLight.position;
            wait = waitDur;
        }


    }
    
    private void OnDrawGizmosSelected()
    {
        if (leftLight && rightLight)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(leftLight.position, rightLight.position);
            Gizmos.DrawSphere(leftLight.position, 0.05f);
            Gizmos.DrawSphere(rightLight.position, 0.05f);
        }
    }

}
