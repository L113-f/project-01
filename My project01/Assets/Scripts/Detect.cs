using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Detect : MonoBehaviour
{
    public Transform startPoint;
    public string PlayerTag = "Player";

    public LayerMask Layers;

    public float offset = 0.03f;
    public bool once = false;
    public bool locked;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (locked && once) return;
        if (HasLineOfSight(collision))
        {
            locked = true;
            Debug.Log("game over");

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!once && other.CompareTag(PlayerTag))
        {
            locked = false;
        }
    }
    
    bool HasLineOfSight(Collider2D target)
    {
        Vector3 b = target.transform.position;
        Vector2 c = b;

        Vector2 origin = startPoint.position;

        Vector2 dir = c - origin;
        float dist = dir.magnitude;

        Vector2 start = origin + dir.normalized * offset;

        RaycastHit2D hit = Physics2D.Raycast(start, dir.normalized, dist, Layers);

        if (hit.collider == null)
        {
            return true;
        }

        return false;
        
        

        
    }
}
