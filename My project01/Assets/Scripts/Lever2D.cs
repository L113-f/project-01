using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Lever2D : MonoBehaviour
{
    [Header("模式")]
    public bool oneShot = false;

    [Header("状态")]
    public bool triggered;
    public bool isInside;

    [Header("拉动成功时触发")]
    public UnityEvent onPulled;

    [Header("拉杆视觉（不填就用自身）")]
    public Transform leverVisual;

    [Header("两档角度（相对初始角度的Z旋转，单位度）")]
    public float offAngle = 0f;     // 未拉状态
    public float onAngle  = -35f;   // 拉下去状态（按你想的方向改正负）

    [Header("转动动画时长（秒）")]
    public float rotateDuration = 0.12f;

    private bool animating;
    private Quaternion restRot;

    void Awake()
    {
        if (!leverVisual) leverVisual = transform;
        restRot = leverVisual.localRotation;

        // 开局就把视觉对齐到当前 triggered
        ApplyVisualInstant();
    }

    void Update()
    {
        if (!isInside) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (animating) return;

        if (oneShot)
        {
            if (triggered) return;   // oneShot: 已拉过就不再响应
            triggered = true;
        }
        else
        {
            triggered = !triggered;  // 可反复拉：切换状态
        }

        // ✅扳到位并停住（不回弹）
        StartCoroutine(RotateToState());

        // ✅通知外部（转管道等）
        onPulled?.Invoke();
    }

    private IEnumerator RotateToState()
    {
        animating = true;

        float targetAngle = triggered ? onAngle : offAngle;
        Quaternion from = leverVisual.localRotation;
        Quaternion to = restRot * Quaternion.Euler(0f, 0f, targetAngle);

        float t = 0f;
        float dur = Mathf.Max(0.0001f, rotateDuration);

        while (t < 1f)
        {
            t += Time.deltaTime / dur;
            float eased = t * t * (3f - 2f * t); // SmoothStep
            leverVisual.localRotation = Quaternion.Slerp(from, to, eased);
            yield return null;
        }

        leverVisual.localRotation = to;
        animating = false;
    }

    private void ApplyVisualInstant()
    {
        float targetAngle = triggered ? onAngle : offAngle;
        leverVisual.localRotation = restRot * Quaternion.Euler(0f, 0f, targetAngle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player")) isInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player")) isInside = false;
    }
}
