using UnityEngine;
using UnityEngine.UI;   // Text / Image 都继承自 Graphic

public class RecBlink : MonoBehaviour
{
    // 要闪烁的那个红点，可以是 Image 或 Text
    public Graphic target;

    // 闪烁间隔时间（秒）
    public float interval = 0.5f;

    private float timer = 0f;
    private bool visible = true;

    void Awake()
    {
        // 如果没手动拖，就默认用自己身上的 Graphic
        if (target == null)
            target = GetComponent<Graphic>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            visible = !visible;
            if (target != null)
                target.enabled = visible;   // 打开/关闭显示
        }
    }
}
