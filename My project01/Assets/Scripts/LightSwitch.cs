using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Lever2D lever;

    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();  
    }

    void Update()
    {
        if(lever.triggered)
        {
            spriteRenderer.color = new Color(0.55f,1f,0f,1f);
        }
    }
}
