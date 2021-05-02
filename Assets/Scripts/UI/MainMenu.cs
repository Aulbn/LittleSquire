using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] GameObject mainMenuCanvas;
	[SerializeField] private GameObject optionsCanvas;
	[SerializeField] private Slider sensitivitySlider;
	[SerializeField] private Slider volumeSlider;
	[SerializeField] private Toggle sprintToggle;
	[SerializeField] private Image[] mainMenuArrows;
	[SerializeField] private Image[] optionsArrows;
	private PlayerControls playerControls;
	
	private float sensitivity = 10f;
	private float volume = 10f;

	private Vector2 mousePosition;
	public PlayableDirector timelineDirector;
	private bool isQuitting;
	

	private void Awake()
	{
		playerControls = new PlayerControls();
		playerControls.Menu.Point.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
	}

	private void Start()
	{
		mainMenuCanvas.SetActive(true);
		optionsCanvas.SetActive(false);
		sensitivity = PlayerPrefs.HasKey("sensitivity") ? PlayerPrefs.GetFloat("sensitivity") : 10f;
		volume = PlayerPrefs.HasKey("volume") ? PlayerPrefs.GetFloat("volume") : 1f;
		AudioListener.volume = volume;
		sensitivitySlider.value = sensitivity;
		volumeSlider.value = volume;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;
	}

	public void OnStart()
	{
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		timelineDirector.Play();
	}

	public void OnOptions()
	{
		mainMenuCanvas.SetActive(false);
		optionsCanvas.SetActive(true);
	}

	public void OnQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void OnOptionsDone()
	{
		optionsCanvas.SetActive(false);
		mainMenuCanvas.SetActive(true);
	}

	public void OnSensitivityChanged(float value)
	{
		sensitivity = value;
	}

	public void OnVolumeChanged(float value)
	{
		volume = value;
		AudioListener.volume = value;
	}

    public void OnSprintToggle(Toggle toggle)
    {
        PlayerPrefs.SetInt("ToggleSprint", toggle.isOn ? 1 : 0);
    }

    private void OnEnable()
	{
		playerControls.Enable();
        if (PlayerPrefs.HasKey("ToggleSprint"))
            sprintToggle.isOn = PlayerPrefs.GetInt("ToggleSprint") == 1;
        FindObjectOfType<Player>().canPause = false;
    }

    private void OnDisable()
	{
		playerControls.Disable();
		PlayerPrefs.SetFloat("volume", volume);
		PlayerPrefs.SetFloat("sensitivity", sensitivity);
		PlayerPrefs.Save();
        //FindObjectOfType<Player>().EnablePauseMenu();
        if (!isQuitting)
        {
			FindObjectOfType<Player>().canPause = true;
			UIManager.Instance.EnableHUD();
        }
	} 

	private void Update()
	{
		ChangeArrowPosition();
	}

	private void ChangeArrowPosition()
	{
		float minDistance = 100000f;
		if (optionsCanvas.activeSelf)
		{
			Image closestarrow = null;
			foreach (Image arrow in optionsArrows)
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

			if (closestarrow != null) closestarrow.enabled = true;

			return;
		}

		Image closestArrow = null;
		foreach (Image arrow in mainMenuArrows)
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

		if (closestArrow != null) closestArrow.enabled = true;
	}

	private void OnApplicationQuit()
	{
		isQuitting = true;
	}
}
