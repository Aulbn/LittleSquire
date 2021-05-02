using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(TriggerboxEventInvoker))]
public class TriggerboxController : MonoBehaviour
{
    private TriggerboxEventInvoker eventInvoker;
    private List<GameObject> collidedActors = new List<GameObject>();

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "d_FilterSelectedOnly@2x");
    }

    void Start()
    {
        eventInvoker = GetComponent<TriggerboxEventInvoker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collidedActors.Add(other.gameObject);
            eventInvoker.InvokeActivateEvents();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the trigger is caused by a puzzlebox or player
        if (other.CompareTag("Player"))
        {
            collidedActors.Remove(other.gameObject);

            // if no actors are colliding with the triggerplate, deactivate all connected prefabs
            if (collidedActors.Count == 0)
            {
                eventInvoker.InvokeDeactivateEvents();
            }
        }
    }
}
