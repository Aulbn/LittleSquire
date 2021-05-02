using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class PuzzleEventInvoker : MonoBehaviour
{
    public UnityEvent puzzleActivateEvents;
    public UnityEvent puzzleDeactivateEvents;
    public bool invokeWithCutsceneOnce = false;
    public PlayableDirector timelineDirector;

    private bool timelinePlayed = false; 

    public void InvokeActivateEvents()
    {
        puzzleActivateEvents.Invoke();
        if(invokeWithCutsceneOnce && !timelinePlayed)
        {
            timelinePlayed = true;
            timelineDirector.Play();
        }
    }

    public void InvokeDeactivateEvents()
    {
        puzzleDeactivateEvents.Invoke();
    }
}
