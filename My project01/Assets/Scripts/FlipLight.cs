using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipLight : MonoBehaviour
{
    [Header("节奏")]
    public float waitDur = 3f;      
    public float flipDuration = 0.35f;

    [Header("目标X角")]
    public float targetX = 180f;      

    private float wait;
    public bool flipped;             
    private bool isFlipping;          
    private float t;                  

    private Quaternion fromRot;      
    private Quaternion toRot;        

    void Start()
    {
        wait = waitDur;
       
        var e = transform.localEulerAngles;
        float x0 = flipped ? targetX : 0f;
        transform.localRotation = Quaternion.Euler(x0, e.y, e.z);
    }

    void Update()
    {
       
        if (isFlipping)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, flipDuration);
            transform.localRotation = Quaternion.Slerp(fromRot, toRot, Mathf.Clamp01(t));

            if (t >= 1f)            
            {
                isFlipping = false;
                flipped = !flipped;   
                wait = waitDur;       
            }
            return;
        }

       
        if (wait > 0f)
        {
            wait -= Time.deltaTime;
            return;
        }

       
        var eNow = transform.localEulerAngles;
        float toX = flipped ? 0f : targetX;     
        fromRot = transform.localRotation;
        toRot   = Quaternion.Euler(toX, eNow.y, eNow.z);

        t = 0f;
        isFlipping = true;
    }
}
