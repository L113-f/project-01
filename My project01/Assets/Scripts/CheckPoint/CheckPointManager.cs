using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance;

    [HideInInspector]
    public Vector3 lastCheckpointPosition;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            lastCheckpointPosition = player.transform.position;
        }
    }

    public void UpdateCheckPoint(Vector3 newPos)
    {
        lastCheckpointPosition = newPos;
        Debug.Log("存档成功"+newPos);
    }

    
}
