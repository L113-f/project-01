using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightFlipper : MonoBehaviour
{
[Header("层级引用")]
    [SerializeField] private Transform pivot;       // 负责旋转的父物体
    [SerializeField] private GameObject detectArea;  // 检测区域

    [Header("时间设置")]
    [SerializeField] private float stayTime = 2.5f;   // 停留时间
    [SerializeField] private float rotateDuration = 0.4f; // ✅ 旋转动作耗时（转得越快，这个值越小）

    [Header("旋转角度")]
    [SerializeField] private float yAngleRight = 0f;
    [SerializeField] private float yAngleLeft = 180f;

    private bool isFacingRight = true;

    void Start()
    {
        if (pivot == null || detectArea == null) return;
        
        // 初始状态
        UpdateRotationImmediate();
        StartCoroutine(FlipRoutine());
    }

    // 核心逻辑：循环控制转身和等待
    IEnumerator FlipRoutine()
    {
        while (true)
        {
            // 1. 在当前方向停留
            yield return new WaitForSeconds(stayTime);

            // 2. 开始转身，立即关闭检测
            detectArea.SetActive(false);
            
            // 3. 执行旋转过程
            isFacingRight = !isFacingRight;
            Quaternion startRot = pivot.localRotation;
            Quaternion endRot = Quaternion.Euler(0, isFacingRight ? yAngleRight : yAngleLeft, 0);
            
            float elapsed = 0f;
            while (elapsed < rotateDuration)
            {
                elapsed += Time.deltaTime;
                // 使用 Lerp 配合极短的时间，能产生非常精准的运动控制
                pivot.localRotation = Quaternion.Slerp(startRot, endRot, elapsed / rotateDuration);
                yield return null;
            }

            // 4. ✅ 旋转完成，强制对齐目标角度，并立即开启检测
            pivot.localRotation = endRot;
            detectArea.SetActive(true);
        }
    }

    private void UpdateRotationImmediate()
    {
        float targetY = isFacingRight ? yAngleRight : yAngleLeft;
        pivot.localRotation = Quaternion.Euler(0, targetY, 0);
        detectArea.SetActive(true);
    }
}
