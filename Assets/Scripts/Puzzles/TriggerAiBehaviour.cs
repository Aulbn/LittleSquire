using UnityEngine;

public class TriggerAiBehaviour : MonoBehaviour
{
    [SerializeField] private AI.AISystem ai = null;

    public void MoveTo(Transform transform)
    {
        ai.SetNewDestination(transform.position);
        ai.SetMoveSpeed();
        ai.SetState(new AI.WalkingState(ai));
    }

    public void CommandOnStay(Transform transform)
    {
        ai.OnStay(transform.position);
    }
}
