using UnityEngine;

[RequireComponent(typeof(PuzzleEventInvoker))]
public class PuzzleLeverController : MonoBehaviour, IInteract
{
    private PuzzleEventInvoker eventInvoker;
    private PuzzleLeverAnimationHandler animationHandler;

    public AudioSource playLeverSound;

    public bool isPulled { get; protected set; }

    void Start()
    {
        eventInvoker = GetComponent<PuzzleEventInvoker>();
        animationHandler = GetComponentInChildren<PuzzleLeverAnimationHandler>();
    }

    public void StartPullAnimation(PuzzleLeverAnimationHandler.Puller puller)
    {
        animationHandler.StartPullAnimation(puller);
    }

    public void Pull()
    {
        if (!isPulled)
        {
            eventInvoker.InvokeActivateEvents();
            isPulled = true;
        }
        else
        {
            eventInvoker.InvokeDeactivateEvents();
            isPulled = false;
        }

        playLeverSound.Play();
    }

    public void Interact(/*CharacterInteraction interactor*/)
    {
        //if (interactor.canInteract)
        //{
        //    //play anim
        //    StartPullAnimation(PuzzleLeverAnimationHandler.Puller.Eleanor);
        //    Pull();
        //}
        //else if (interactor.kidCanInteract)
        //{
        //    CompanionInteract();
        //}
    }

    public void CompanionInteract()
    {
        Vector3 position = transform.position + ((isPulled ? -transform.forward : transform.forward) * 0.8f);
        //AI.AISystem ai = FindObjectOfType<AI.AISystem>();
        //ai.SetFocusPoint(transform.position + transform.up);
        //ai.OnPull(position, this);
    }
}
