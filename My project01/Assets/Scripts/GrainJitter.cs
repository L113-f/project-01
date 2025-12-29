using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrainJitter : MonoBehaviour
{
    public float jitterPixels = 2f;
    public float interval = 0.05f;

    private RectTransform rt;
    private Vector2 basePos;
    private float t;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        basePos = rt.anchoredPosition;
    }

    void OnEnable()
    {
        t = 0f;
        rt.anchoredPosition = basePos;
    }

    
    void Update()
    {
        
        t += Time.unscaledDeltaTime;
        if (t < interval) return;
        t = 0f;

        float x = Random.Range(-jitterPixels, jitterPixels);
        float y = Random.Range(-jitterPixels, jitterPixels);
        rt.anchoredPosition = basePos + new Vector2(x, y);
    }
}
