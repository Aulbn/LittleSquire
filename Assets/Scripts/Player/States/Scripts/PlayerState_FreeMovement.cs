using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "States/Player/FreeMovement")]
public class PlayerState_FreeMovement : State
{
    public float walkSpeed, runSpeed;
    public float gravity = 5;
    private float turnTime = 0.1f;
    private float accelerationTime = 0.1f;

    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private float currentSpeed;
    private Vector2 input;

    PlayerController player;


    public override void Initialize(Controller owner)
    {
        player = (PlayerController)owner;
        //cam = player.cam;
        //anim = player.anim;
    }

    public override void Enter()
    {
        player.Controls.Gameplay.Move.performed += ctx => OnMovement(ctx.ReadValue<Vector2>());
        player.Controls.Gameplay.Enable();
    }

    public override void Update()
    {
        RotationUpdate();
        MovementUpdate();
    }

    public override void Exit()
    {
        player.Controls.Gameplay.Move.performed -= ctx => OnMovement(ctx.ReadValue<Vector2>());
        //player.Controls.Gameplay.Disable();
    }

    private void MovementUpdate()
    {
        if (player.cc.isGrounded && player.HasControl)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, walkSpeed * input.magnitude, ref speedSmoothVelocity, accelerationTime);
            Velocity = Velocity.y * transform.up + transform.forward * currentSpeed;
            //anim.SetFloat("Horizontal", Velocity.x / movementSpeed);
            //anim.SetFloat("Vertical", Velocity.z / movementSpeed);
            //player.anim.anim.SetFloat("Horizontal", Velocity.x / walkSpeed);
            //player.anim.anim.SetFloat("Vertical", Velocity.z / walkSpeed);
        }
        else
            Velocity += Vector3.down * gravity * Time.deltaTime; //Gravity

        player.cc.Move(Velocity * Time.deltaTime); //Movement
        player.anim.anim.SetBool("IsGrounded", player.cc.isGrounded);
        player.anim.SetLocomotion(new Vector3(Velocity.x, 0, Velocity.z).magnitude, walkSpeed, runSpeed);
    }
    private void RotationUpdate()
    {
        if (!player.HasControl) return;

        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRot = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + player.mainCamera.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref turnSmoothVelocity, turnTime);
        }
    }

    public void OnMovement(Vector2 value)
    {
        input = value.normalized;
        //Debug.Log(input);
    }

}
