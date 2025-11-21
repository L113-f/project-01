using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Fungus 引用")]
    public Flowchart flowchart;
    public string talkStageVarName = "TalkStage";  // Fungus 里的整型变量名

    [Header("不同阶段对应的 Block 名")]
    public string firstTalkBlock = "FirstTalk";        // TalkStage == 0
    public string noItemBlock   = "Talk_NoItem";       // TalkStage == 1
    public string afterItemBlock = "Talk_AfterItem";   // TalkStage >= 2

    [Header("交互设置")]
    public string playerTag = "Player";
    public KeyCode talkKey = KeyCode.E;

    private bool canSay;

    void Update()
    {
        if (!canSay) return;
        if (!flowchart) return;

        // 如果 Fungus 正在播放对话，就先别重复触发
        if (flowchart.GetExecutingBlocks().Count > 0)
            return;

        if (Input.GetKeyDown(talkKey))
        {
            Say();
        }
    }

    public void Say()
    {
        if (flowchart == null) return;

        // 从 Fungus 里读取当前阶段
        int stage = flowchart.GetIntegerVariable(talkStageVarName);

        string blockToRun = null;

        if (stage == 0)
        {
            blockToRun = firstTalkBlock;
        }
        else if (stage == 1)
        {
            blockToRun = noItemBlock;
        }
        else // 2 及以上都走第二段对话（反抗/顺从）
        {
            blockToRun = afterItemBlock;
        }

        if (!string.IsNullOrEmpty(blockToRun) && flowchart.HasBlock(blockToRun))
        {
            flowchart.ExecuteBlock(blockToRun);
        }
        else
        {
            Debug.LogWarning($"Flowchart 里找不到 Block：{blockToRun}");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            canSay = true;
            // TODO：这里可以弹一个 UI 提示“按 E 对话”
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            canSay = false;
            // TODO：这里可以隐藏“按 E 对话”提示
        }
    }
}
