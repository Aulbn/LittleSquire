using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class CameraEventTrigger : MonoBehaviour
{
    public List<GameObject> collidableObjects;
    public bool alwaysDrawTrigger;

    public enum DisableType
    {
        None, OnEnter, OnExit
    }
    public DisableType disableType;

    public UnityEvent enterEvents;
    public UnityEvent exitEvents;

    void OnDrawGizmos()
    {
        if (!alwaysDrawTrigger) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().bounds.size);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (collidableObjects.Contains(other.gameObject))
        {
            enterEvents.Invoke();
            if (disableType == DisableType.OnEnter)
                GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (collidableObjects.Contains(other.gameObject))
        {
            exitEvents.Invoke();
            if (disableType == DisableType.OnExit)
                GetComponent<BoxCollider>().enabled = false;
        }
    }
}
