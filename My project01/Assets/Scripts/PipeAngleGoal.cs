using UnityEngine;
using System.Collections;

public class PipeAngleGoal : MonoBehaviour
{
    [Header("正确角度（度）：填 0/90/180/270；可填多个（直管常用 0,180）")]
    public int[] matchAngles = new int[] { 0 };

    [Header("当前是否匹配（只读）")]
    public bool isMatched;

    [Header("旋转动画时长（秒）")]
    public float rotateDuration = 0.5f;

    private bool rotating;

    // ✅外部调用：旋转 90°（顺时针视觉通常是 -90）
    public void Rotate90()
    {
        if (rotating) return;
        StartCoroutine(Rotate90Co(rotateDuration));
    }

    private IEnumerator Rotate90Co(float dur)
    {
        rotating = true;

        float startZ = transform.localEulerAngles.z;
        float targetZ = startZ - 90f; // 顺时针
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, dur);
            float eased = t * t * (3f - 2f * t); // SmoothStep
            float z = Mathf.LerpAngle(startZ, targetZ, eased);
            transform.localRotation = Quaternion.Euler(0f, 0f, z);
            yield return null;
        }

        // ✅最终吸附到 0/90/180/270，避免误差
        int snapped = Snap90(transform.localEulerAngles.z);
        transform.localRotation = Quaternion.Euler(0f, 0f, snapped);

        Evaluate();

        rotating = false;
    }

    public void Evaluate()
    {
        int a = Snap90(transform.localEulerAngles.z);
        isMatched = ContainsAngle(matchAngles, a);
    }

    static bool ContainsAngle(int[] arr, int angle)
    {
        if (arr == null || arr.Length == 0) return false;
        for (int i = 0; i < arr.Length; i++)
        {
            if (Snap90(arr[i]) == angle) return true;
        }
        return false;
    }

    static int Snap90(float z)
    {
        int a = Mathf.RoundToInt(z) % 360;
        if (a < 0) a += 360;

        int snapped = Mathf.RoundToInt(a / 90f) * 90;
        snapped %= 360;
        return snapped;
    }

    void OnEnable()
    {
        Evaluate();
    }
}
