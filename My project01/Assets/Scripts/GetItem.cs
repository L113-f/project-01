using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Fungus;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    public GameObject Monitor;
    public bool inRange;
    public float destroyDelay = 0.5f;
    

    public Flowchart flowchart;

    public bool picked;
    void Update()
    {
        if(picked)
            return;
        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PickRoutine());
        }
    }

    IEnumerator PickRoutine()
    {
        picked = true;
        flowchart.SetBooleanVariable("GetItem",true);
        flowchart.SetIntegerVariable("TalkStage", 2);

        yield return new WaitForSeconds(destroyDelay);
        if(Monitor)
        {
            Destroy(Monitor);   
        }
        
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            inRange = true;
        }
    }
}
