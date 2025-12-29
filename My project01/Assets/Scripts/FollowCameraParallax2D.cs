using UnityEngine;

public class FollowCameraParallax2D : MonoBehaviour
{
    public Transform targetCamera;        
    [Range(0f, 1f)] public float followX = 1f; 
    [Range(0f, 1f)] public float followY = 0.3f;
    public Vector2 offset;              
    public bool keepZ = true;

    private Vector3 startCamPos;
    private Vector3 startPos;

    void Start()
    {
        if (targetCamera == null) targetCamera = Camera.main.transform;
        startCamPos = targetCamera.position;
        startPos = transform.position;
    }

    void LateUpdate()
    {
        Vector3 camDelta = targetCamera.position - startCamPos;

        Vector3 newPos = startPos + new Vector3(camDelta.x * followX, camDelta.y * followY, 0f)
                        + (Vector3)offset;

        if (keepZ) newPos.z = transform.position.z;

        transform.position = newPos;
    }
}
