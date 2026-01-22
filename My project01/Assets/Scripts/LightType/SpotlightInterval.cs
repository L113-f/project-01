using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightInterval : MonoBehaviour
{
    public GameObject DetectRoot;
    public GameObject LightRoot;

    public float onTime = 1.5f;
    public float offTime = 1f;

    public bool random = false;
    public Vector2 onRange = new Vector2(1f, 2f);
    public Vector2 offRange = new Vector2(0.6f, 1.4f);

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 10f; // 旋转平滑速度

    float timer;
    bool isOn;
    float curOn, curOff;

    // 重点：保持 Z 为 -90
    private Quaternion rotOn;
    private Quaternion rotOff;

    void Awake()
    {
        // 初始化旋转目标：保持 Z 轴固定为 -90
        rotOn = Quaternion.Euler(0f, 0f, -90f);   // 亮起：X为0
        rotOff = Quaternion.Euler(90f, 0f, -90f); // 熄灭：X为90

        PickDur();
        isOn = true;
        LightRoot.transform.localRotation = rotOn;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 根据当前的 isOn 状态确定目标旋转
        Quaternion targetRot = isOn ? rotOn : rotOff;

        // 使用 Slerp 平滑插值
        LightRoot.transform.localRotation = Quaternion.Slerp(
            LightRoot.transform.localRotation, 
            targetRot, 
            Time.deltaTime * rotationSpeed
        );

        // 状态切换逻辑
        if (isOn && timer >= curOn)
        {
            SwitchState(false);
        }
        else if (!isOn && timer >= curOff)
        {
            SwitchState(true);
        }
    }

    private void SwitchState(bool toOn)
    {
        timer = 0f;
        isOn = toOn;
        PickDur();

        // 碰撞检测（DetectRoot）的开关逻辑：
        // 如果你希望灯完全转上去再关闭检测，可以用这个逻辑；
        // 如果想瞬间切换，就保持现在的代码：
        DetectRoot.SetActive(toOn); 
    }

    public void PickDur()
    {
        if (!random)
        {
            curOn = onTime;
            curOff = offTime;
        }
        else
        {
            curOn = Random.Range(onRange.x, onRange.y);
            curOff = Random.Range(offRange.x, offRange.y);
        }
    }
}