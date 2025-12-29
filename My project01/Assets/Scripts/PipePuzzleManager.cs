using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PipePuzzleManager : MonoBehaviour
{
    [Header("所有管道（全部为 true 才连通）")]
    public PipeAngleGoal[] allPipes;

    [Header("连通时触发（false->true）")]
    public UnityEvent onSolved;

    [Header("断开时触发（true->false）")]
    public UnityEvent onUnsolved;

    [Header("当前是否已连通（会动态变化）")]
    public bool solved;

    // ✅不限制拉杆：永远允许
    public bool CanPull()
    {
        return true;
    }

    // ✅旋转动画结束后再检查（即使已 solved 也要检查，可能会被转坏）
    public void CheckSolvedDelayed(float delaySeconds)
    {
        StopAllCoroutines();
        StartCoroutine(CheckSolvedDelayCo(Mathf.Max(0f, delaySeconds)));
    }

    private IEnumerator CheckSolvedDelayCo(float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        CheckSolved();
    }

    public void CheckSolved()
    {
        bool prev = solved;
        bool now = true;

        if (allPipes == null || allPipes.Length == 0)
        {
            now = false;
        }
        else
        {
            for (int i = 0; i < allPipes.Length; i++)
            {
                var p = allPipes[i];
                if (!p) continue;

                p.Evaluate();
                if (!p.isMatched)
                {
                    now = false;
                    break;
                }
            }
        }

        solved = now;

        // ✅只在状态变化时触发事件
        if (!prev && now) onSolved?.Invoke();        // 连通
        else if (prev && !now) onUnsolved?.Invoke(); // 断开
    }
}
