using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class DelayedEvent : MonoBehaviour
{
    [Serializable] public struct DelayedEventObject
    {
        public float delay;
        public UnityEvent events;
    }

    public DelayedEventObject[] delayedEvents;

    public void Invoke()
    {
        foreach (DelayedEventObject deo in delayedEvents)
        {
            StartCoroutine(IEInvoke(deo));
        }
    }

    private IEnumerator IEInvoke(DelayedEventObject deo)
    {
        yield return new WaitForSeconds(deo.delay);
        deo.events.Invoke();
    }

    public void DebugText(string text)
    {
        Debug.Log(text);
    }
}
