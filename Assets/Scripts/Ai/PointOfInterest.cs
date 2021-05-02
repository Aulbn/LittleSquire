using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    private Vector3 focusPoint;

    [Tooltip("Time to inspect the point of interest")]
    [SerializeField] private float inspectTime = 5f;
    [Tooltip("Distance to the focus point")]
    [SerializeField] private float focusDistance = 1.5f;

    public float InspectTime { get => inspectTime; }
    public Vector3 FocusPoint { get => focusPoint; }

    private void Awake()
    {
        focusPoint = transform.position + (transform.up * 0.1f) + (transform.forward * focusDistance);

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, float.MaxValue))
        {
            transform.position = hit.point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + (transform.up * 0.1f), transform.position + (transform.up * 0.1f) + (transform.forward * focusDistance));
        Gizmos.DrawSphere(transform.position + (transform.up * 0.1f) + (transform.forward * focusDistance), .1f);
    }
}
