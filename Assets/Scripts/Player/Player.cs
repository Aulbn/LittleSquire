using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	private PlayerControls playerControls;
	private CharacterMovement characterMovement;
	private CharacterInteraction characterInteraction;
	
	private bool isHoldingBox;
	private Vector2 movementInput;
	private Vector2 mouseDelta;
	private GameManager gameManager;
	private bool inputsActive = true;
	private CinemachineBrain brain;
	private float mouseSensitivity = 1f;
	private bool freezeOnCameraBlend = true;

    public bool canPause = true;

	public PlayerControls PlayerControls => playerControls;

	private void Awake()
	{
		characterInteraction = GetComponent<CharacterInteraction>();
		characterMovement = GetComponent<CharacterMovement>();
		gameManager = FindObjectOfType<GameManager>();
		playerControls = new PlayerControls();
		playerControls.Gameplay.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
		playerControls.Gameplay.Camera.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
		playerControls.Gameplay.Sprint.started += ctx => characterMovement.OnSprint(true);
		playerControls.Gameplay.Sprint.canceled += ctx => characterMovement.OnSprint(false);
		playerControls.Gameplay.Interact.performed += ctx => OnInteract();
	    playerControls.Gameplay.Pause.performed += ctx => OnPause();
	}
	

	private void Start()
	{
		if (gameManager)
			gameManager.GetPlayerPosition(transform);
        CinemachineCore.GetInputAxis = GetAxisCustom;
		brain = Camera.main.GetComponent<CinemachineBrain>();
	}

	private void Update()
	{
		if (PauseMenu.isPaused)
			return;
		if (!isHoldingBox)
			characterMovement.UpdateMovement(inputsActive ? movementInput : Vector2.zero);
		else if (isHoldingBox)
			characterInteraction.UpdateBoxInteraction(inputsActive ? movementInput.normalized : Vector2.zero);
	}
	

	private void OnInteract()
	{
		if (PauseMenu.isPaused || !inputsActive) 
			return;
		characterInteraction.OnInteraction();
		isHoldingBox = characterInteraction.IsHoldingBox;
	}
	
	private void OnEnable()
	{
		playerControls.Enable();
	}

	private void OnDisable()
	{
		playerControls.Disable();
	}

	private void OnPause()
	{
        if (!canPause) return;

        if (!UIManager.Instance)
            return;
        if (!PauseMenu.isPaused)
			UIManager.PauseMenu.OnActivate();
		else 
			UIManager.PauseMenu.OnDeactivate();
	}

	public bool IsHoldingBox()
	{
		return isHoldingBox;
	}

	[ContextMenu("SaveCheckPoint")]
	private void SaveCheckpoint()
	{
		if (gameManager)
			gameManager.SetPlayerPosition(transform);
	}

	[ContextMenu("GameOver")]
	private void GameOver()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void SetMovementInput(bool setBool)
	{
		inputsActive = setBool;
	}

	public void SetCharacterPosition(GameObject newPosition)
	{
		transform.position = newPosition.transform.position;
		transform.rotation = newPosition.transform.rotation;
	}

	private float GetAxisCustom(string name)
	{
		if ((brain.IsBlending && freezeOnCameraBlend) || PauseMenu.isPaused)
		{
			return 0;
		}
		switch (name)
		{
			case "Mouse X":
				return mouseDelta.x * mouseSensitivity;
			case "Mouse Y":
				return mouseDelta.y * mouseSensitivity;
			default:
				return 0;
		}
	}

	public void SetHoldingBox(bool setValue)
	{
		isHoldingBox = setValue;
	}

	public void SetSensitivity(float value)
	{
		mouseSensitivity = value;
	}

	public void RemoveParent()
	{
		transform.parent = null;
	}
	public void SetFreezeOnBlend(bool freeze)
	{
		freezeOnCameraBlend = freeze;
	}

	//public void EnablePauseMenu()
	//{
	//	playerControls.Gameplay.Pause.performed += ctx => OnPause();
	//}
}
