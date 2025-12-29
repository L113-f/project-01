using System.Collections;
using UnityEngine;

public class SlidingDoor2D : MonoBehaviour
{
    [Header("门要移动的本体（不填就用自己）")]
    public Transform doorVisual;

    [Header("开门位移（本地坐标）")]
    public Vector3 openOffset = new Vector3(1.5f, 0f, 0f);

    [Header("开门时间（秒）")]
    public float openDuration = 0.35f;

    [Header("开门后禁用碰撞（可选）")]
    public Collider2D[] collidersToDisable;

    private Vector3 closedLocalPos;
    private bool opened;

    void Awake()
    {
        if (!doorVisual) doorVisual = transform;
        closedLocalPos = doorVisual.localPosition;
    }

    public void Open()
    {
        if (opened) return;
        opened = true;

        if (collidersToDisable != null)
        {
            foreach (var c in collidersToDisable)
                if (c) c.enabled = false;
        }

        StopAllCoroutines();
        StartCoroutine(OpenCo());
    }

    private IEnumerator OpenCo()
    {
        Vector3 from = closedLocalPos;
        Vector3 to = closedLocalPos + openOffset;

        float t = 0f;
        float dur = Mathf.Max(0.0001f, openDuration);

        while (t < 1f)
        {
            t += Time.deltaTime / dur;
            float eased = t * t * (3f - 2f * t); // SmoothStep
            doorVisual.localPosition = Vector3.Lerp(from, to, eased);
            yield return null;
        }

        doorVisual.localPosition = to;
    }
}
