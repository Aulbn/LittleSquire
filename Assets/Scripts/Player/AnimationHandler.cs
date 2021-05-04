using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public static AnimationHandler Instance;
    //private CharacterMovement movement;
    public Animator anim { get; private set; }
    //public Transform spineJoint;
    //public float spineMaxRotation = 90;

    private float currentSpeed;
    private bool hasStepped = false;
    private float footCurve;
    private AudioManager footstepAudio;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        anim = GetComponent<Animator>();
        //movement = GetComponentInParent<CharacterMovement>();
    }

    private void Start()
    {
        footstepAudio = GetComponentInParent<AudioManager>();

    }

    void Update()
    {
        FootStepUpdate();

        //currentSpeed = movement.CurrentSpeed <= movement.WalkSpeed ?
        //    (movement.CurrentSpeed / movement.WalkSpeed) / 2 :
        //    (movement.CurrentSpeed / movement.SprintSpeed);

        //anim.SetFloat("Velocity", currentSpeed);
        //anim.SetBool("IsGrounded", movement.IsOnGround);
    }

    public void SetLocomotion(float velocity, float walkSpeed, float runSpeed)
    {
        anim.SetFloat("Velocity", velocity / walkSpeed);
    }

    private void FootStepUpdate()
    {
        if (currentSpeed < 0.1f) return;

        footCurve = anim.GetFloat("FootCurve");
        if (hasStepped && footCurve < 0)
        {
            hasStepped = false;
        } else if (!hasStepped && footCurve > 0)
        {
            hasStepped = true;
            footstepAudio.Play();
        }
    }

    public void TogglePushing(bool value)
    {
        SetBool("IsPushing", value);
    }

    public void Push(bool isPushingForward)
    {
        SetBool("IsPushingForward", isPushingForward);
        SetTrigger("Push");
    }

    public void SetBool(string id, bool value)
    {
        anim.SetBool(id, value);
    }

    public void SetTrigger(string id)
    {
        anim.SetTrigger(id);
    }
}
