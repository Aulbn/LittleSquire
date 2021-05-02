using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    public static DialogueManager DialogueManager => Instance.dialogueManager;
    [SerializeField] private InteractionPrompt interactionPrompt;
    public static InteractionPrompt InteractionPrompt => Instance.interactionPrompt;
    [SerializeField] private PauseMenu pauseMenu;
    public static PauseMenu PauseMenu => Instance.pauseMenu;

    [SerializeField] private GameObject[] children;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (SceneManager.GetActiveScene().buildIndex > 1)
            EnableHUD();
    }

    public void EnableHUD()
    {
        foreach (GameObject child in children)
        {
            child.gameObject.SetActive(true);
        }
    }
}
