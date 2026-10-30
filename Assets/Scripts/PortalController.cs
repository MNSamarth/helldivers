using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform destination;
    private readonly HashSet<Collider2D> arrivingColliders = new HashSet<Collider2D>();

    private bool IsInsidePortal(Collider2D playerCollider)
    {
        if (playerCollider == null || !playerCollider.enabled)
            return false;

        foreach (Collider2D trigger in GetComponents<Collider2D>())
        {
            if (trigger.enabled && trigger.isTrigger && trigger.Distance(playerCollider).isOverlapped)
                return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        // Rearm only after the player has physically cleared the arrival portal.
        arrivingColliders.RemoveWhere(collider => !IsInsidePortal(collider));
    }

    private void OnDisable()
    {
        arrivingColliders.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D body = other.attachedRigidbody;
        Transform player = body != null ? body.transform : other.transform;
        if (!player.CompareTag("Player") || destination == null || arrivingColliders.Contains(other))
            return;

        PortalController arrivalPortal = destination.GetComponentInParent<PortalController>();
        if (arrivalPortal != null)
        {
            foreach (Collider2D playerCollider in player.GetComponentsInChildren<Collider2D>())
                arrivalPortal.arrivingColliders.Add(playerCollider);
        }

        if (body != null)
            body.position = destination.position;
        else
            player.position = destination.position;
    }
}
