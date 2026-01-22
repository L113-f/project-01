using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight360Rotation : MonoBehaviour
{
    [Header("旋转设置")]
    [Tooltip("每秒旋转的角度。正数顺时针，负数逆时针。")]
    [SerializeField] private float rotateSpeed = 45f; 

    [Header("轴向设置")]
    [Tooltip("通常 2D 游戏旋转 Y 轴，如果是俯视角 2D 可能是 Z 轴")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up; 

    [Header("状态控制")]
    public bool isRotating = true;

    void Update()
    {
        if (isRotating)
        {
            // 使用 Rotate 方法是最稳健的持续旋转方案
            // 它会自动处理角度叠加，不会有 360 度回弹的问题
            transform.Rotate(rotationAxis * rotateSpeed * Time.deltaTime);
        }
    }

    // 提供一个公开方法，方便以后通过按钮或触发器暂停旋转
    public void ToggleRotation(bool state)
    {
        isRotating = state;
    }
}
