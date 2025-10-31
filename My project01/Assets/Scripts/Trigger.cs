using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public string playerTag = "Player";
    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag(playerTag))
        {
            triggered = true;
            Debug.Log("被探照灯发现，失败！");
            // TODO: 弹UI或重载关卡
        }
    }

    
}
