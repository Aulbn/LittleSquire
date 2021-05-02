using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EndCutscene : MonoBehaviour
{
    [SerializeField] private bool isUsingTimeline;
    [SerializeField] private GameObject[] scenes;
    [SerializeField] private PlayableDirector[] timelineScenes;
    [SerializeField] private DialogueTrigger dialogue;
    
    private PlayerControls playerControls;
    private int currentSceneIndex = 0;
    private int nextSceneIndex = 1;
    private static EndCutscene instance;
    private SceneLoadTrigger sceneTrigger;

    private void Awake()
    {
        instance = this;
        sceneTrigger = GetComponent<SceneLoadTrigger>();
        if (isUsingTimeline)
        { 
            timelineScenes[currentSceneIndex].Play();
        }
        else
        {
            foreach (GameObject scene in scenes)
            {
                scene.SetActive(false);
            }
        
            scenes[0].SetActive(true);
        }
    }

    private void Start()
    {
        dialogue.TriggerDialogue();
    }

    public void NextImage()
    {
        if (scenes.Length > nextSceneIndex)
        {
            scenes[currentSceneIndex].SetActive(false);
            scenes[nextSceneIndex].SetActive(true);
            currentSceneIndex++;
            nextSceneIndex++;
        }
    }

    public void NextTimelineScene()
    {
        if (timelineScenes.Length > nextSceneIndex)
        {
            timelineScenes[currentSceneIndex].Stop();
            timelineScenes[nextSceneIndex].Play();
            currentSceneIndex++;
            nextSceneIndex++;
        }
    }

    private void ChangeScene()
    {
        if (isUsingTimeline)
            NextTimelineScene();
        else 
            NextImage();
    }
    
    public static void NextScene()
    {
        instance.ChangeScene();
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }
}
