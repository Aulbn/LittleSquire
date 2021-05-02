using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public delegate void OnReloadScene();
    public static OnReloadScene OnRestartScene;


    public static bool isPaused;
	[SerializeField]private GameObject pauseMenuCanvas;
	[SerializeField]private GameObject settingsCanvas;
	[SerializeField]private GameObject quitPrompt;
	[SerializeField]private GameObject backToMainPrompt;
	[SerializeField] private Slider sensitivitySlider;
	[SerializeField] private Slider volumeSlider;
	[SerializeField] private Toggle sprintToggle;
	[SerializeField] private Image[] mainPauseArrowImages;
	[SerializeField] private Image[] optionsArrowImages;
	private float newSensitivity;
	private float volume;
	private Player playerControls;
	private PlayerControls playerInput;
	Vector2 mousePosition;
	private Image activeArrow;
	
	private void Start()
	{
		isPaused = false;
		Time.timeScale = 1f;
		gameObject.SetActive(false);
		AudioListener.pause = false;
		playerInput = playerControls.PlayerControls;
		playerInput.Menu.Point.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
	}

	private void OnEnable()
	{
		playerControls = FindObjectOfType<Player>();
		if (PlayerPrefs.HasKey("sensitivity"))
			OnSensitivityChanged(PlayerPrefs.GetFloat("sensitivity"));
		if (PlayerPrefs.HasKey("volume"))
			OnVolumeChanged(PlayerPrefs.GetFloat("volume"));
        if (PlayerPrefs.HasKey("ToggleSprint"))
            sprintToggle.isOn = PlayerPrefs.GetInt("ToggleSprint") == 1;

        playerControls.SetSensitivity(newSensitivity);
		volumeSlider.value = AudioListener.volume;
		sensitivitySlider.value = newSensitivity * 10f;


        settingsCanvas.SetActive(false);
		quitPrompt.SetActive(false);
		backToMainPrompt.SetActive(false);
	}

	public void OnActivate() 
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		isPaused = true;
		AudioListener.pause = true;
		gameObject.SetActive(true);
		pauseMenuCanvas.SetActive(true);
		settingsCanvas.SetActive(false);
		quitPrompt.SetActive(false);
		backToMainPrompt.SetActive(false);
		Time.timeScale = 0f;
	}

	public void OnDeactivate()
	{
		playerControls.SetSensitivity(newSensitivity);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		isPaused = false;
		AudioListener.pause = false;
		gameObject.SetActive(false);
		pauseMenuCanvas.SetActive(false);
		Time.timeScale = 1f;
    }

    public void OnSprintToggle(Toggle toggle)
    {
        PlayerPrefs.SetInt("ToggleSprint", toggle.isOn ? 1 : 0);
    }

	public void OnSettings()
	{
		settingsCanvas.SetActive(true);
		pauseMenuCanvas.SetActive(false);
	}

	public void OnSettingsDone()
	{
		pauseMenuCanvas.SetActive(true);
		settingsCanvas.SetActive(false);
	}
	
	public void OnBack()
	{
		OnDeactivate();
	}

	public void OnQuit()
	{
		quitPrompt.SetActive(true);
	}

	public void OnYesQuit()
	{
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#else
		Application.Quit();
#endif
	}

	public void OnNoQuit()
	{
		quitPrompt.SetActive(false);
	}

	public void OnBackToMain()
	{
		backToMainPrompt.SetActive(true);
	}

	public void OnYesToMain()
	{
		SceneManager.LoadScene(0);
	}

	public void OnNoToMain()
	{
		backToMainPrompt.SetActive(false);
	}
	
	public void OnVolumeChanged(float value)
	{
		AudioListener.volume = value;
		volume = value;
	}
	

	public void OnSensitivityChanged(float value)
	{
		newSensitivity = value * 0.1f;
	}

	public void OnRestart()
	{
        OnRestartScene?.Invoke();
    }

	private void OnDisable()
	{
		PlayerPrefs.SetFloat("volume", volume);
		PlayerPrefs.SetFloat("sensitivity", newSensitivity * 10f);
		PlayerPrefs.Save();
    }

	private void ChangeArrowPosition()
	{
		float minDistance = 100000f;
		if (settingsCanvas.activeSelf)
		{
			Image closestarrow = null;
			foreach (Image arrow in optionsArrowImages)
			{
				arrow.enabled = false;
				Vector3 position = arrow.transform.position;
				Vector2 arrowPos = new Vector2(position.x, position.y);
				float distance = (arrowPos - mousePosition).magnitude;
				if (distance <= minDistance)
				{
					minDistance = distance;
					closestarrow = arrow;
				}
			}

			if (closestarrow != null && (!backToMainPrompt.activeSelf || quitPrompt.activeSelf)) closestarrow.enabled = true;

			return;
		}

		Image closestArrow = null;
		foreach (Image arrow in mainPauseArrowImages)
		{
			arrow.enabled = false;
			Vector3 position = arrow.transform.position;
			Vector2 arrowPos = new Vector2(position.x, position.y);
			float distance = (arrowPos - mousePosition).magnitude;
			if (distance <= minDistance)
			{
				minDistance = distance;
				closestArrow = arrow;
			}
		}

		if (closestArrow != null && (!backToMainPrompt.activeSelf || quitPrompt.activeSelf)) closestArrow.enabled = true;
	}

	private void Update()
	{
		ChangeArrowPosition();
	}
}
