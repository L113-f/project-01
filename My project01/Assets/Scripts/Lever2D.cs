using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever2D : MonoBehaviour
{
    
    public bool triggered;
    public bool isInside;

    void Update() {
        if (isInside && Input.GetKeyDown(KeyCode.E))
        {
            triggered = !triggered;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
        }
    } 

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            isInside = false;
        }
    } 
}
