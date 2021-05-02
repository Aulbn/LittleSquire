using UnityEngine;
using UnityEngine.Events;

public class TriggerboxEventInvoker : MonoBehaviour
{
    public UnityEvent ActivateEvents;
    public UnityEvent DeactivateEvents;

    public void InvokeActivateEvents()
    {
        ActivateEvents.Invoke();
    }

    public void InvokeDeactivateEvents()
    {
        DeactivateEvents.Invoke();
    }
}
