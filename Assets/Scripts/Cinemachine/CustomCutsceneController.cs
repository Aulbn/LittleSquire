using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomCutsceneController : MonoBehaviour
{
    private int counter = 0;
    private bool cutsceneActivated = false;
    public PlayableDirector timelineDirector;
    public DelayedEvent delayedEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlateSteppedOn()
    {
        counter++;
        if (counter == 2)
        {
            cutsceneActivated = true;
            timelineDirector.Play();
            delayedEvent.Invoke();
        }
    }

    public void LoadEndingCutscene()
    {
        SceneManager.LoadScene("EndingCutscene");
    }

    public void PlateSteppedOff()
    {
        if (cutsceneActivated)
            return;
        counter--;
    }
}
