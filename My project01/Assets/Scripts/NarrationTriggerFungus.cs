using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class NarrationTriggerFungus : MonoBehaviour
{
    [Header("Fungus")]
    public Flowchart flowchart;
    public string blockName;
    public string playerTag = "Player";
    public bool triggerOnce = true;

    [Header("相机缩进（可选）")]
    public bool doCameraZoom = false;       // 勾上才会缩进
    public Transform zoomTarget;            // 镜头要对准的目标（比如监视器）
    public float zoomSize = 3f;             // 缩进去后 Orthographic Size
    public float zoomInTime = 0.4f;         // 缩进时间
    public float zoomHoldTime = 1.2f;       // 停留时间
    public float zoomOutTime = 0.5f;        // 拉回时间

    private bool triggered;

    public bool Once;

    public bool CameraTrigger;



    public PlayerController player;

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(!CameraTrigger)
        {   
            if(!Once)
                return;
            player.canMove = false;
        }

       
        
        if (!other.transform.root.CompareTag("Player")) return;
        if (triggerOnce && triggered) return;
        if (!flowchart) return;

        // 1. 触发 Fungus Block（播旁白）
        if (flowchart.HasBlock(blockName))
        {
            flowchart.ExecuteBlock(blockName);
        }

        // 2. 可选：做一次相机缩进
        if (doCameraZoom && zoomTarget && CMZoom2D.Instance != null)
        {
            CMZoom2D.Instance.ZoomTo(
                zoomTarget,
                zoomSize,
                zoomInTime,
                zoomHoldTime,
                zoomOutTime
            );
        }

        triggered = true;




    }

    public void CanMove()
    {
        player.canMove = true;
        Once = false;
    }

    
}
