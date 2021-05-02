using UnityEngine;

public class ShakeTrigger : MonoBehaviour
{
    [SerializeField] private bool triggerOnce;
    [SerializeField] private ShakingObject shakingObject;
    [SerializeField] private Transform position;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce)
            GetComponent<BoxCollider>().enabled = false;

        if (!shakingObject.IsShaking)
        {
            shakingObject.StartShaking();

            if (position != null)
            {
                GetComponent<TriggerAiBehaviour>().CommandOnStay(position);
            }
        }

    }
}
