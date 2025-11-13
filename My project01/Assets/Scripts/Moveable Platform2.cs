using UnityEngine;

public class MoveablePlatform2 : MonoBehaviour
{
    public enum MoveMode { AlwaysLoop, LeverControlled }

    [Header("路径")]
    public Transform PosA, PosB;
    public Transform startPos;

    [Header("移动")]
    [SerializeField] private float speed = 2f;

    [Header("模式")]
    public MoveMode mode = MoveMode.AlwaysLoop;
    public Lever2D lever; 

    private Transform target;

    void Start()
    {
        
        target = startPos ? startPos : PosB;
        
        if (startPos == PosA) target = PosB;
        else if (startPos == PosB) target = PosA;
    }

    void Update()
    {
        if (!PosA || !PosB) return;

        
        if (mode == MoveMode.LeverControlled)
        {
            if (!lever) return;        
            if (!lever.triggered) return;  
        }

        if (Vector2.Distance(transform.position, PosA.position) < 0.1f) target = PosB;
        if (Vector2.Distance(transform.position, PosB.position) < 0.1f) target = PosA;

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.transform.SetParent(transform); // 让玩家跟平台一起移动
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.transform.parent == transform)
            collision.transform.SetParent(null);
    }

    void OnDrawGizmosSelected()
    {
        if (PosA && PosB)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(PosA.position, PosB.position);
            Gizmos.DrawSphere(PosA.position, 0.05f);
            Gizmos.DrawSphere(PosB.position, 0.05f);
        }
    }
}
