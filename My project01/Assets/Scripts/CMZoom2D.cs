using System.Collections;
using UnityEngine;
using Cinemachine;

public class CMZoom2D : MonoBehaviour
{
    public static CMZoom2D Instance { get; private set; }

    [Header("Cinemachine 虚拟相机")]
    public CinemachineVirtualCamera vcam;

    [Header("默认跟随目标（比如 Player）")]
    public Transform defaultFollow;

    [Header("默认视野（Orthographic Size）")]
    public float defaultSize = 5f;

    public float moveSpeed = 5f;  // 缩放/移动插值速度

    Coroutine currentRoutine;

    public PlayerController player;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (!vcam)
        {
            vcam = FindObjectOfType<CinemachineVirtualCamera>();
        }

        if (vcam != null)
        {
            // 初始用虚拟相机当前的 orthographicSize 当默认值
            defaultSize = vcam.m_Lens.OrthographicSize;
            if (defaultFollow == null && vcam.Follow != null)
                defaultFollow = vcam.Follow;
        }
    }

    /// <summary>
    /// 从当前画面缩进到 target，停一会，再拉回默认视角
    /// </summary>
    public void ZoomTo(Transform target, float zoomSize = 3f,
                       float zoomTime = 0.4f, float holdTime = 1.2f, float backTime = 0.5f)
    {
        if (vcam == null || target == null) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ZoomRoutine(target, zoomSize, zoomTime, holdTime, backTime));
    }

    IEnumerator ZoomRoutine(Transform target, float zoomSize, float zoomTime, float holdTime, float backTime)
    {

        if (player != null)
        {
            player.canMove = false;
        }
        if (vcam == null || target == null) yield break;

        // 记录原状态
        Transform oldFollow = vcam.Follow;
        float startSize = vcam.m_Lens.OrthographicSize;
        Vector3 startPos = vcam.transform.position;

        // 在缩进期间，不再让 vcam 跟随角色
        vcam.Follow = null;

        // 目标位置：对准监视器
        Vector3 targetPos = target.position;
        targetPos.z = startPos.z;

        // 1. 缩进
        float t = 0f;
        while (t < zoomTime)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / zoomTime);

            vcam.transform.position = Vector3.Lerp(startPos, targetPos, k);
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, zoomSize, k);

            yield return null;
        }

        // 2. 停留
        yield return new WaitForSeconds(holdTime);

        // 3. 拉回
        t = 0f;
        while (t < backTime)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / backTime);

            vcam.transform.position = Vector3.Lerp(targetPos, startPos, k);
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(zoomSize, startSize, k);

            yield return null;
        }

        // 恢复
        vcam.m_Lens.OrthographicSize = startSize;
        vcam.Follow = oldFollow != null ? oldFollow : defaultFollow;
        if (player != null)
        {
            player.canMove = true;
        }

    
        currentRoutine = null;
    }

    
}
