using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveablePlatform2 : MonoBehaviour
{
    public Transform PosA, PosB;
    [SerializeField] private float speed = 2f;
    Transform tartget;
    [SerializeField] public Transform startPos;
    void Start()
    {
        tartget = startPos;
    }


    void Update()
    {
        if (Vector2.Distance(transform.position, PosA.position) < .1f) tartget = PosB;
        if (Vector2.Distance(transform.position, PosB.position) < .1f) tartget = PosA;

        transform.position = Vector2.MoveTowards(transform.position, tartget.position, speed * Time.deltaTime);


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = this.transform;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }

}
