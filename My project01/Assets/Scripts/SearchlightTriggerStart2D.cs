using UnityEngine;

public class SearchlightTriggerStart2D : MonoBehaviour
{
    public SearchlightPatrolX2D[] targets;
    public string playerTag = "Player";
    public bool disableAfterTriggered = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        for (int i = 0; i < targets.Length; i++)
            if (targets[i]) targets[i].Activate();

        if (disableAfterTriggered)
            gameObject.SetActive(false);
    }
}
