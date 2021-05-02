using UnityEngine;

public class ShakingObject : MonoBehaviour
{
    [SerializeField] private bool shouldBreak;
    [SerializeField] private float shakeTime = 1f;
    [SerializeField] private float shakeIntensity = 0.7f;
    [SerializeField] private bool shouldResetPosition;
    [SerializeField] private GameObject crackedVersion;

    private bool isShaking;
    private Vector3 startPosition;
    private float shakeTimer;

    private Transform cachedTransform;

    public bool IsShaking { get => isShaking; }

    private void Start()
    {
        cachedTransform = transform;
        startPosition = cachedTransform.position;
    }

    private void Update()
    {
        if (!isShaking) return;

        shakeTimer += Time.deltaTime;
        cachedTransform.position = startPosition + Random.insideUnitSphere * (shakeIntensity * Time.deltaTime);

        if (!(shakeTimer >= shakeTime)) return;
        shakeTimer = 0f;
        if (shouldResetPosition)
        {
            cachedTransform.position = startPosition;
        }
        isShaking = false;
        if (shouldBreak)
            StartBreaking();


    }
    [ContextMenu("EARTHQUAKE!")]
    public void StartShaking()
    {
        isShaking = true;
        startPosition = cachedTransform.position;
    }

    public void StartBreaking()
    {
        Instantiate(crackedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
