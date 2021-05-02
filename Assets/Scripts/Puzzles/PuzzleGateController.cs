using UnityEngine;

public class PuzzleGateController : MonoBehaviour
{
    [SerializeField] private Transform gateMeshTransform;
    [SerializeField] private Vector3 relativeOpenTarget;
    [SerializeField] private float moveSpeed = 3;
    [Tooltip("Limit the speed the door moves as soon the distance is lower than the value")]
    [SerializeField] private float minimumDistanceTreshold = 0.5f;
    [SerializeField] private bool isOpen;
    private Vector3 gateOpenTarget;
    public AudioSource playGateSound;

    [SerializeField] private AudioSource finalOpenGateSound;
    [SerializeField] private AudioSource finalCloseGateSound;

    private int counter;
    private bool hasPlayedFinalSound = true;

    public bool IsOpen { get => isOpen; }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + relativeOpenTarget, 0.2f);
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }

    void Start()
    {
        SetDefaults();
    }

    void Update()
    {
        UpdatePosition();
    }

    private void SetDefaults()
    {
        gateOpenTarget = transform.position + relativeOpenTarget;
        if (isOpen)
        {
            gateMeshTransform.position = gateOpenTarget;
        }
        else
        {
            gateMeshTransform.localPosition = Vector3.zero;
        }
    }

    private void UpdatePosition()
    {
        // calculate vector distances to gate target position
        float distanceToOpen = (gateMeshTransform.position - gateOpenTarget).magnitude;
        float distanceToClose = (gateMeshTransform.position - transform.position).magnitude;

        // clamp the values so you don't get super small values
        distanceToOpen = Mathf.Clamp(distanceToOpen, 0.01f, 1000f);
        distanceToClose = Mathf.Clamp(distanceToClose, 0.01f, 1000f);

        if (isOpen)
        {
            MoveGate(gateOpenTarget, distanceToOpen);
        }
        else
        {
            MoveGate(transform.position, distanceToClose);
        }
    }

    private void MoveGate(Vector3 target, float distance)
    {
        gateMeshTransform.transform.position = Vector3.MoveTowards(gateMeshTransform.position,
            target,
            Time.deltaTime * moveSpeed * (distance < minimumDistanceTreshold ?
            minimumDistanceTreshold : distance)
        );

        if (!hasPlayedFinalSound && gateMeshTransform.position == target)
        {
            if (playGateSound != null)
                playGateSound.Stop();

            if (isOpen)
            {
                if (finalOpenGateSound != null)
                    finalOpenGateSound.Play();
            }
            else
            {
                if (finalCloseGateSound != null)
                 finalCloseGateSound.Play();
            }
            hasPlayedFinalSound = true;
        }
    }

    public void OpenGate()
    {
        isOpen = true; 
        hasPlayedFinalSound = false;
        if (playGateSound != null)
            playGateSound.Play();
    }
    public void CloseGate()
    {
        isOpen = false;
        hasPlayedFinalSound = false;
        if (playGateSound != null)
            playGateSound.Play();
    }

    public void PlateSteppedOn()
    {
        counter++;
        if (counter == 2)
        {
            OpenGate();
            AI.AISystem ai = FindObjectOfType<AI.AISystem>();
            ai.SetState(new AI.FollowState(ai));
        }
    }

    public void PlateSteppedOff()
    {
        if (isOpen)
            return;
        counter--;
    }
}

