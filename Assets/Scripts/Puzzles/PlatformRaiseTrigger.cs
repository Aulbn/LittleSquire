using System;
using UnityEngine;

public class PlatformRaiseTrigger : MonoBehaviour
{
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private Transform position;
    private void OnTriggerEnter(Collider other)
    {
        if (position != null)
            GetComponent<TriggerAiBehaviour>().CommandOnStay(position);
        other.gameObject.transform.parent = transform.root;
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerOnce)
            GetComponent<BoxCollider>().enabled = false;
        other.gameObject.transform.parent = null;
    }
}
