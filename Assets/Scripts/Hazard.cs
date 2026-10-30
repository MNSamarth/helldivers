using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private string failReason = "You stepped on a spike trap";
    [SerializeField] private bool slowPlayerInsteadOfFail;
    [SerializeField, Range(0.1f, 1f)] private float speedMultiplier = 0.5f;
    [SerializeField, Min(0f)] private float slowDuration = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform player = other.attachedRigidbody != null
            ? other.attachedRigidbody.transform : other.transform;
        if (player.CompareTag("Player"))
        {
            if (slowPlayerInsteadOfFail)
                player.GetComponent<PlayerMovement>()?.ApplySlow(speedMultiplier, slowDuration);
            else
                GameManager.Instance?.FailLevel(failReason);
        }
    }
}
