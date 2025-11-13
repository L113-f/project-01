using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeVisual : MonoBehaviour
{
     public Transform topHook;        
    public Transform platformHook;  

    public float tilingPerUnit = 1f; 

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        if (!topHook) topHook = transform;
        lr.positionCount = 2;
       
    }

    void LateUpdate()
    {
        if (!platformHook) return;

       
        lr.SetPosition(0, topHook.position);
        lr.SetPosition(1, platformHook.position);

        float len = Vector2.Distance(topHook.position, platformHook.position);
        if (lr.material && lr.material.mainTexture)
        {
            Vector2 scale = lr.material.mainTextureScale;
            lr.material.mainTextureScale = new Vector2(Mathf.Max(0.01f, len * tilingPerUnit), scale.y);
        }
    }
}
