using UnityEngine;

public class LeverActionRotate : MonoBehaviour
{
    public PipePuzzleManager manager;
    public PipeAngleGoal[] targets;

    // ✅给 Lever2D 的 onPulled 绑定这个函数
    public void Pull()
    {
        // 不限制：随便拉
        if (manager != null && !manager.CanPull()) return;

        float maxDur = 0f;

        if (targets != null)
        {
            foreach (var p in targets)
            {
                if (!p) continue;
                maxDur = Mathf.Max(maxDur, p.rotateDuration);
                p.Rotate90();
            }
        }

        // ✅关键：等旋转动画结束后再检查（否则你现在 0.5 秒动画会导致判定提前）
        if (manager != null)
        {
            manager.CheckSolvedDelayed(maxDur + 0.02f); // +0.02 给一丢丢缓冲
        }
    }
}
