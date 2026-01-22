using UnityEngine;

public class SearchlightPatrolX2D : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float speed = 2f;

    [Header("触发后启用的对象（灯光整套 GameObject）")]
    public GameObject lightObjectToEnable;

    [Header("启动后先去离当前更远的点")]
    public bool startToFarthest = true;

    bool active;
    float minX, maxX;
    float fixedY;
    int dir; // -1 往左，+1 往右

    void Awake()
    {
        if (lightObjectToEnable) lightObjectToEnable.SetActive(false);
        active = false;
    }

    public void Activate()
    {
        if (active) return;
        if (!pointA || !pointB)
        {
            Debug.LogError("[SearchlightPatrolX2D] pointA/pointB 没拖！");
            return;
        }

        // ✅ 缓存世界坐标（避免点跟着动）
        float ax = pointA.position.x;
        float bx = pointB.position.x;
        minX = Mathf.Min(ax, bx);
        maxX = Mathf.Max(ax, bx);

        fixedY = transform.position.y;

        if (lightObjectToEnable) lightObjectToEnable.SetActive(true);
        active = true;

        // ✅ 启动先朝“离当前位置更远”的端点移动
        float x = transform.position.x;
        if (startToFarthest)
        {
            float dToMin = Mathf.Abs(x - minX);
            float dToMax = Mathf.Abs(x - maxX);
            dir = (dToMin >= dToMax) ? -1 : +1; // 更远的是 minX 就先往左，否则往右
        }
        else
        {
            dir = +1;
        }

        // 启动就先夹到范围内一次
        x = Mathf.Clamp(x, minX, maxX);
        transform.position = new Vector3(x, fixedY, transform.position.z);

        Debug.Log($"[SearchlightPatrolX2D] Activated on {gameObject.name}  rangeX=[{minX},{maxX}] dir={dir}");
    }

    void LateUpdate()
    {
        if (!active) return;

        float x = transform.position.x;
        x += dir * speed * Time.deltaTime;

        // ✅ 强制夹住 + 到边缘反向
        if (x >= maxX) { x = maxX; dir = -1; }
        else if (x <= minX) { x = minX; dir = +1; }

        transform.position = new Vector3(x, fixedY, transform.position.z);
    }

    void OnDrawGizmosSelected()
    {
        if (!pointA || !pointB) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
