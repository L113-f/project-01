using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleSwingTest : MonoBehaviour
{
    public Rigidbody2D ropeBody;        // 手动拖：绳子（链子）末节的刚体
    public Transform handPoint;         // 玩家手的位置
    public KeyCode grabKey = KeyCode.E;
    public KeyCode releaseKey = KeyCode.Space;
    public float extraTangentSpeed = 1.5f;

    Rigidbody2D rb;
    HingeJoint2D joint;
    bool attached;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!handPoint) handPoint = transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(grabKey) && !attached)
        {
            Attach();
        }
        if (Input.GetKeyDown(releaseKey) && attached)
        {
            Release();
        }
    }

    void Attach()
    {
        if (!ropeBody) { Debug.LogWarning("没有指定 ropeBody"); return; }

        joint = gameObject.AddComponent<HingeJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = ropeBody;
        joint.enableCollision = false;

        // 玩家侧 anchor：手的位置
        joint.anchor = transform.InverseTransformPoint(handPoint.position);
        // 绳子侧 anchor：直接用ropeBody中心（简单粗暴，先能动）
        joint.connectedAnchor = Vector2.zero;

        attached = true;
        Debug.Log("已挂到绳子上");
    }

    void Release()
    {
        if (!joint || !ropeBody) { Cleanup(); return; }

        Vector2 p = handPoint ? (Vector2)handPoint.position : (Vector2)transform.position;
        Vector2 vPoint = ropeBody.GetPointVelocity(p);

        Vector2 radial = p - ropeBody.worldCenterOfMass;
        if (radial.sqrMagnitude < 1e-6f) radial = Vector2.up * 1e-3f;
        Vector2 tangent = new Vector2(-radial.y, radial.x).normalized;
        tangent *= Mathf.Sign(Vector2.Dot(tangent, vPoint));

        Cleanup();

        Vector2 extra = tangent * extraTangentSpeed;
        rb.velocity = vPoint + extra;

        Debug.Log("释放绳子，速度：" + rb.velocity);
    }

    void Cleanup()
    {
        if (joint) Destroy(joint);
        joint = null;
        attached = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!handPoint) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(handPoint.position, 0.15f);
    }
}
