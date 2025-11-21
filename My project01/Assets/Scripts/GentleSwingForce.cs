using UnityEngine;

public class GentleSwingForce : MonoBehaviour
{
    public float force = 0.5f;      
    public float frequency = 0.5f;  

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
     
        float s = Mathf.Sin(Time.time * Mathf.PI * 2f * frequency);

     
        Vector2 f = new Vector2(s * force, 0f);
        rb.AddForce(f, ForceMode2D.Force);
    }
}
