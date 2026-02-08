using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHazard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.root.CompareTag("Player"))
        {
            if(GameManager.Instance!=null)
            {
                GameManager.Instance.PlayerDeath();
            }
        }
    }
}
