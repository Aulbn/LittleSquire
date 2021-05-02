using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
	private PuzzleBoxController heldBox;
	private Vector3 pushDirection;
	private bool isHoldingBox;
	[HideInInspector]public bool canInteract;
	[HideInInspector]public bool kidCanInteract;
	private Camera camera;
	private Interact interactComp;
	private CharacterMovement characterMovement;
	private Player player;
	private CinemachineVirtualCamera boxCam;
	private Transform boxCamFollowTarget;

	public bool IsHoldingBox => isHoldingBox;

    public bool isOnPlatform = false;

	private void Start()
	{
		camera = Camera.main;
		interactComp = GetComponent<Interact>();
		characterMovement = GetComponent<CharacterMovement>();
		player = GetComponent<Player>();
		boxCamFollowTarget = transform.Find("VcamTarget"); //find VcamTarget transform
	}
	

	public void OnInteraction()
	{
		if (!canInteract && !kidCanInteract)
		{
			return;
		}

		if (canInteract)
		{
			if (isHoldingBox)
			{
				if (heldBox.isMovable)
					ReleaseBox();
				return;
			}
			IInteract interact = interactComp.GetCurrentInteractable();
			interact?.Interact(this);
		}

		if (kidCanInteract)
		{
            if (isOnPlatform)
            {
                DialogueTrigger dialogueTrigger = GameObject.FindGameObjectWithTag("Kid").GetComponent<DialogueTrigger>();
                if (dialogueTrigger)
                    dialogueTrigger.TriggerPlayerBlockingDialogue();
                else
                    Debug.Log("Did not find DialogueTrigger");
            }
            else
            {
                IInteract interact = interactComp.GetCurrentInteractable();
                interact?.Interact(this);
            }
		}
	}

	public void UpdateBoxInteraction(Vector2 inputDirection)
	{
		if (inputDirection.y > 0)
		{
			if (pushDirection.x < 0)
				heldBox.Move(Vector3.right, true);
			else if (pushDirection.x > 0)
                heldBox.Move(Vector3.left, true);
            else if (pushDirection.z < 0)
                heldBox.Move(Vector3.forward, true);
            else if (pushDirection.z > 0)
                heldBox.Move(Vector3.back, true);
        }
        else if (inputDirection.y < 0)
		{
			if (pushDirection.x > 0)
                heldBox.Move(Vector3.right, false);
            else if (pushDirection.x < 0)
                heldBox.Move(Vector3.left, false);
            else if (pushDirection.z > 0)
                heldBox.Move(Vector3.forward, false);
            else if (pushDirection.z < 0)
                heldBox.Move(Vector3.back, false);
        }
    }
	
	public void StartGrabbingBox(PuzzleBoxController grabbedBox)
	{
		heldBox = grabbedBox;
		Vector3 grabbingPosition = heldBox.GetClosestPushingPosition(transform.position);
		if (grabbingPosition == Vector3.zero)
			return;
		characterMovement.SetTargetPosition(grabbingPosition, heldBox);
	}
	
	
	public void GrabBox()
	{
		boxCam = heldBox.BoxCamera; // find camera in held box
		boxCam.gameObject.SetActive(false); // deactivate camera so blend doesn't glitch out
		transform.parent = heldBox.transform; 
		isHoldingBox = true;
		player.SetHoldingBox(true);
		boxCam.Priority = 11; 
		boxCam.Follow = boxCamFollowTarget.transform;
		boxCam.gameObject.SetActive(true); // set camera active after all the realignments has happened
		AnimationHandler.Instance.TogglePushing(isHoldingBox);
	}

	private void ReleaseBox()
	{
		isHoldingBox = false;
		transform.parent = null;
		heldBox = null;	
		player.SetHoldingBox(false);
		boxCam.Priority = 0;
		boxCam.Follow = null;
		AnimationHandler.Instance.TogglePushing(isHoldingBox);
	}

	public void SetPushingDirection(Vector3 direction)
	{
		pushDirection = direction;
	}
}