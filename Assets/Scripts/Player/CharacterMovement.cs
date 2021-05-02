using System;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement speed")]
    [SerializeField]
    private float walkSpeed = 5f;

    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float walkToInteractionSpeed = 2.5f;

    [Header("Movement smoothing")]
    [SerializeField]
    private float turnSmoothTime = 0.2f;

    [SerializeField] private float speedSmoothTime = 0.1f;
    [Header("Physics")] [SerializeField] private float gravityScale = 1f;
    [SerializeField] private LayerMask groundLayer;

    //Refactor to a audio manager class
    //private AudioSource audioSource;
    //[Header("Audio settings")]
    //[SerializeField] private AudioClip walkingSound = null;
    //[SerializeField] private AudioClip runningSound = null;

    private float gravity = 9.8f;
    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private float turnTime;
    private float currentSpeed;
    private Transform cameraTransform;
    private Transform cachedTransform;
    private CharacterController characterController;
    private bool isOnGround;
    private Vector3 groundCheckPosition;
    private float groundCheckRadius;
    private int groundCheckLayer;
    private Vector3 velocity;
    private bool isSprinting;
    private Vector3 targetPosition;
    private bool isMovingToGrab;
    private CharacterInteraction characterInteraction;
    private IInteract currentInteraction;
    
    public float CurrentSpeed => currentSpeed;
    public float WalkSpeed => walkSpeed;
    public float SprintSpeed => sprintSpeed;
    public bool IsOnGround => isOnGround;

    private void Awake()
    {
        if (Camera.main != null) cameraTransform = Camera.main.transform;
        cachedTransform = transform;
        characterController = GetComponent<CharacterController>();
        turnTime = turnSmoothTime;
        groundCheckRadius = characterController.radius;

        //audioSource = GetComponent<AudioSource>();
        //if (walkingSound)
        //{
        //    audioSource.clip = walkingSound;
        //}

        characterInteraction = GetComponent<CharacterInteraction>();
    }

    private void Update()
    {
        if (isMovingToGrab)
        {
            MoveTowardsTargetPosition();
        }
    }

    public void UpdateMovement(Vector2 input)
    {
        Vector2 inputDirection = input.normalized;
        groundCheckPosition = transform.TransformPoint(characterController.center) -
                               Vector3.up * (characterController.height * 0.5f - characterController.radius);
        groundCheckPosition.y -= 0.1f;
        isOnGround = Physics.CheckSphere(groundCheckPosition, groundCheckRadius, groundLayer);
        if (inputDirection != Vector2.zero && !isMovingToGrab)
        {
            HandleCharacterRotation(inputDirection);
        }
        float targetSpeed = (isSprinting ? sprintSpeed : walkSpeed) * input.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);


        if (!isMovingToGrab)
        { 
            HandleCharacterMovement(inputDirection);
        }
        HandleSounds();
    }
    public void OnSprint(bool buttonDown)
    {
        bool isToggle = PlayerPrefs.GetInt("ToggleSprint") == 1;

        if (isToggle)
        {
            if (buttonDown)
                isSprinting = !isSprinting;
        }
        else
        {
            isSprinting = buttonDown;
        }
    }

    private void HandleCharacterRotation(Vector2 inputDirection)
    {
        float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg +
                               (isMovingToGrab ? 0 : cameraTransform.eulerAngles.y);
        cachedTransform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(cachedTransform.eulerAngles.y,
            targetRotation,
            ref turnSmoothVelocity, turnTime);
    }
    
    private void HandleCharacterMovement(Vector2 inputDirection)
    {
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        
        Vector3 movementDir =
            ((cameraForward.normalized * inputDirection.y) + (cameraRight.normalized * inputDirection.x)).normalized;
        velocity = movementDir * (currentSpeed * Time.deltaTime);
        velocity.y = -gravity * gravityScale * Time.deltaTime;
        if (inputDirection.magnitude <= 0.01f && isSprinting && PlayerPrefs.GetInt("ToggleSprint") == 1)
            isSprinting = false;
        characterController.Move(velocity);
    }
    
    private void MoveTowardsTargetPosition()
    {
        float targetSpeed = walkToInteractionSpeed;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        Vector3 position = cachedTransform.position;
        targetPosition.y = position.y;
        Vector3 movementDir = (targetPosition - position).normalized;
        velocity = movementDir * (currentSpeed * Time.deltaTime);
        velocity.y -= gravity * gravityScale * Time.deltaTime;
        HandleCharacterRotation(new Vector2(movementDir.x, movementDir.z));
        if (isSprinting && PlayerPrefs.GetInt("ToggleSprint") == 1)
            isSprinting = false;
        
        characterController.Move(velocity);
        if ((targetPosition - cachedTransform.position).magnitude <= 0.05f)
        {
            cachedTransform.position = targetPosition;
            if (currentInteraction != null)
                RotateTowardsInteraction();
        }
    }

    public void SetTargetPosition(Vector3 targetPos, IInteract targetInteraction = null)
    {
        currentInteraction = targetInteraction;
        targetPosition = targetPos;
        isMovingToGrab = true;
    }

    public void MoveUpwardsWhitPlatform(MovablePlattform platform)
    {
        Debug.Log("Moveupwards");
    }

    private void RotateTowardsInteraction()
    {
        PuzzleBoxController puzzleBox = currentInteraction as PuzzleBoxController;
        if (puzzleBox != null)
        {
            LookAtTarget(puzzleBox.transform.position);
            characterInteraction.GrabBox();
            isMovingToGrab = false;
            return;
        }
        PuzzleLeverController lever = currentInteraction as PuzzleLeverController;
        if (lever != null)
        {
            Debug.Log("Found the lever!");
            LookAtTarget(lever.transform.position);
        }
    }

    private void HandleSounds()
    {
        Vector3 lastPosition = cachedTransform.position;
        //if (lastPosition != cachedTransform.position)
        //{
        //    if (!audioSource.isPlaying)
        //    {
        //        audioSource.Play();
        //    }
        //}
        //else if (audioSource.isPlaying)
        //{
        //    audioSource.Stop();
        //}
    }

    private void LookAtTarget(Vector3 target)
    {
        cachedTransform.LookAt(target);
        Quaternion currentRot = cachedTransform.rotation;
        currentRot.x = 0f;
        currentRot.z = 0f;
        cachedTransform.rotation = currentRot;
    }
}