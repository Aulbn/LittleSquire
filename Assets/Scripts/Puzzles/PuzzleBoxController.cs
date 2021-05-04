using System;
using Cinemachine;
using UnityEngine;

public class PuzzleBoxController : MonoBehaviour, IInteract
{
    [SerializeField] private float moveIncrament = 1f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float timeBetweenPushes = 0.2f;
    public bool isMovable = true;
    private AudioManager pushAudio;

    [SerializeField]
    private bool shouldFindClosestsPoint;

    private Vector3 targetPosition;
    private float timer;
    private BoxCollider collider;
    [SerializeField] private LayerMask layerToIgnore;
    //private CharacterInteraction player;
    //public AudioSource playBoxSound;
    [SerializeField]private bool drawGizmos = true;
    private Vector3[] pushingPositions = new Vector3[4];

    private CinemachineVirtualCamera boxCamera;

    public CinemachineVirtualCamera BoxCamera => boxCamera;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        targetPosition = transform.position;
        layerToIgnore = ~layerToIgnore;
        boxCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        SetPushingOffsets();
        pushAudio = GetComponentInParent<AudioManager>();
    }

    void Update()
    {
        UpdateBoxPosition();
    }

    private void UpdateBoxPosition()
    {
        float distanceToTarget = (transform.position - targetPosition).magnitude;
        if (distanceToTarget <= 0.1f)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenPushes)
            {
                isMovable = true;
                timer = 0f;
            }
        }
        distanceToTarget = Mathf.Clamp(distanceToTarget, 1f, 1000f);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed * distanceToTarget);

    }

    public void Move(Vector3 dir, bool isForward)
    {
        if (isMovable && !IsColliding(dir, moveIncrament, isForward))
        {
            AnimationHandler.Instance.Push(isForward);
            targetPosition += transform.TransformDirection(dir * moveIncrament);
            isMovable = false;
            pushAudio.Play();
        }
    }

    public void Interact(/*CharacterInteraction interactor*/)
    {
        //player = interactor;
        //interactor.StartGrabbingBox(this);
    }

    private bool IsColliding(Vector3 pushingDirection, float distance, bool isForward)
    {
        bool playerGettingSquished = false;
        if (!isForward)
        {
            //Transform playerTransform = player.transform;
            //playerGettingSquished = Physics.Raycast(playerTransform.position, -playerTransform.forward,distance * 2f,
            //    layerToIgnore);
        }

        Transform boxtransform = transform;
        return Physics.BoxCast(boxtransform.position, collider.bounds.extents * 0.38f, pushingDirection, boxtransform.rotation, distance, layerToIgnore) || playerGettingSquished;
    }

    public Vector3 GetClosestPushingPosition(Vector3 playerPosition)
    {
        return shouldFindClosestsPoint ? TempWalkToClosest(playerPosition) : TempDontWalkAtAll(playerPosition);
    }
    
    
    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.blue;
        foreach (Vector3 position in pushingPositions)
        {
            Gizmos.DrawWireSphere(position + transform.position, 0.1f);
        }
    }

    [ContextMenu("Set Pushing Offset")]
    private void SetPushingOffsets()
    {
        float localScale = transform.localScale.x;
        float multiplier = 0.9f + (0.5f * localScale);
        
        pushingPositions[0] = Vector3.forward * multiplier;
        pushingPositions[1] = Vector3.back * multiplier;
        pushingPositions[2] = Vector3.right * multiplier;
        pushingPositions[3] = Vector3.left * multiplier;
    }

    private Vector3 TempWalkToClosest(Vector3 playerPosition)
    {
        float distance = 10000f;
        Vector3 closestPosition = Vector3.zero;
        foreach (Vector3 position in pushingPositions)
        {
            float distanceToPos = ((position + transform.position) - playerPosition).magnitude;
            if (!Physics.CheckSphere(position + transform.position, 0.5f, layerToIgnore))
            {
                if (distance > distanceToPos)
                {
                    closestPosition = position;
                    distance = distanceToPos;
                }
            }
        }
        
        return closestPosition == Vector3.zero ? Vector3.zero : closestPosition + transform.position;
    }

    private Vector3 TempDontWalkAtAll(Vector3 playerPosition)
    {
        float distance = 10000f;
        Vector3 closestPosition = Vector3.zero;
        foreach (Vector3 position in pushingPositions)
        {
            float distanceToPos = ((position + transform.position) - playerPosition).magnitude;
            {
                if (distance > distanceToPos)
                {
                    closestPosition = position;
                    distance = distanceToPos;
                }
            }
        }

        if (!Physics.CheckSphere(closestPosition + transform.position, 0.5f, layerToIgnore))
            return closestPosition + transform.position;
        return Vector3.zero;
    }
}