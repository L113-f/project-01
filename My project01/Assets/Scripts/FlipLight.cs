using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipLight : MonoBehaviour
{
    public float waitDur = 3f;
    private float wait;
    public bool flipped;
    

    

    void Start()
    {
        wait = waitDur;
    }

    void Update()
    {
        if (wait >= 0)
        {
            wait -= Time.deltaTime;
        }

        if (wait < 0)
        {
            if (flipped == false)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 180);
                flipped = !flipped;
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                flipped = !flipped;
            }

            wait = waitDur;
        }
    }
}
