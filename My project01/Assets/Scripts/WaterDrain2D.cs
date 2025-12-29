using UnityEngine;
using UnityEngine.Events;

public class WaterDrain2D : MonoBehaviour
{
    [Header("触发源（抽水机拉杆）")]
    public Lever2D lever;

    [Header("管道谜题（必须先 solved 才能抽水）")]
    public PipePuzzleManager puzzle;

    [Header("缩放排水")]
    public float shrinkSpeed = 1.5f;   // 每秒减少多少 scaleY
    public bool oneShot = true;        // 只排一次

    [Header("排完后处理")]
    public bool disableGameObject = false; // true: SetActive(false)
    public SpriteRenderer sr;
    public Collider2D[] collidersToDisable;

    [Header("排水完成事件（在这里绑定：水底推拉门 Open）")]
    public UnityEvent onDrainFinished;

    bool draining;
    bool finished;

    // 关键：记录“水底”的世界坐标（保持不变）
    float bottomWorldY;

    void Reset()
    {
        sr = GetComponent<SpriteRenderer>();
        collidersToDisable = GetComponents<Collider2D>();
    }

    void Awake()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (collidersToDisable == null || collidersToDisable.Length == 0)
            collidersToDisable = GetComponents<Collider2D>();

        bottomWorldY = (sr != null) ? sr.bounds.min.y : transform.position.y;

        // ✅订阅拉杆事件：玩家“拉一下”的瞬间触发
        if (lever != null)
            lever.onPulled.AddListener(OnLeverPulled);
    }

    void OnDestroy()
    {
        if (lever != null)
            lever.onPulled.RemoveListener(OnLeverPulled);
    }

    // ✅只有在“管道已连通”后，拉抽水机拉杆才会开始排水
    void OnLeverPulled()
    {
        if (finished) return;
        if (oneShot && draining) return;

        if (puzzle != null && !puzzle.solved)
        {
            Debug.Log("管道未连通，抽水机无法启动");
            return;
        }

        draining = true;

        // 开始排水时刷新一次底部（避免你在编辑器挪过位置）
        bottomWorldY = (sr != null) ? sr.bounds.min.y : transform.position.y;
    }

    void Update()
    {
        if (finished) return;
        if (!draining) return;

        // 1) scale.y -> 0
        Vector3 s = transform.localScale;
        s.y = Mathf.MoveTowards(s.y, 0f, shrinkSpeed * Time.deltaTime);
        transform.localScale = s;

        // 2) 位置补偿：让“水底”保持 bottomWorldY 不动
        if (sr != null)
        {
            float currentHeight = sr.bounds.size.y; // 缩放后的世界高度
            Vector3 p = transform.position;
            p.y = bottomWorldY + currentHeight * 0.5f; // center = bottom + height/2
            transform.position = p;
        }

        // 3) 排完：触发开门 + 禁用水
        if (s.y <= 0.0001f)
        {
            finished = true;

            // ✅排水完成：开门事件（只触发一次）
            onDrainFinished?.Invoke();

            if (disableGameObject)
            {
                gameObject.SetActive(false);
                return;
            }

            if (sr != null) sr.enabled = false;
            foreach (var c in collidersToDisable)
                if (c != null) c.enabled = false;
        }
    }
}
