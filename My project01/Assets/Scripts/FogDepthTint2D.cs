using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogDepthTint2D : MonoBehaviour
{
    [Range(0f, 1f)] public float fogDepth = 1f; // 0=近(黑)，1=远(更灰)
    [Range(0f, 1f)] public float nearGray = 0.0f;
    [Range(0f, 1f)] public float farGray  = 0.35f; // Limbo 远景常用 0.25~0.45

    SpriteRenderer sr;

    void Awake() { sr = GetComponent<SpriteRenderer>(); Apply(); }
    void OnValidate() { if (!sr) sr = GetComponent<SpriteRenderer>(); Apply(); }

    void Apply()
    {
        float g = Mathf.Lerp(nearGray, farGray, fogDepth);
        sr.color = new Color(g, g, g, sr.color.a);
    }
}
