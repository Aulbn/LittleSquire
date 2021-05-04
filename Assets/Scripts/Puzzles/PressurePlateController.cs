using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.Playables;
[RequireComponent(typeof(PuzzleEventInvoker))]
[RequireComponent(typeof(BoxCollider))]
public class PressurePlateController : MonoBehaviour, IInteract
{

    private PuzzleEventInvoker eventInvoker;
    private List<GameObject> collidedActors = new List<GameObject>();
    private bool isBeeingStodOn;
    public AudioSource playPlateOn;
    public AudioSource playPlateOff;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().bounds.size);
    }

    void Start()
    {
        eventInvoker = GetComponent<PuzzleEventInvoker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.TryGetComponent(out Player player) ||
        //    other.TryGetComponent(out AI.AISystem ai) ||
        //    other.TryGetComponent(out PuzzleBoxController puzzleBox))
        //{
        //    collidedActors.Add(other.gameObject);
        //    if (!isBeeingStodOn)
        //        eventInvoker.InvokeActivateEvents();

        //    isBeeingStodOn = true;

        //    playPlateOn.Play();
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.TryGetComponent(out Player player) ||
        //    other.TryGetComponent(out AI.AISystem ai) ||
        //    other.TryGetComponent(out PuzzleBoxController puzzleBox))
        //{
        //    collidedActors.Remove(other.gameObject);

        //    // if no actors are colliding with the triggerplate, deactivate all connected prefabs
        //    if (collidedActors.Count == 0)
        //    {
        //        eventInvoker.InvokeDeactivateEvents();
        //        isBeeingStodOn = false;
        //        playPlateOff.Play();
        //    }
        //}
    }

    public void Interact(/*CharacterInteraction interactor*/)
    {
        //if (interactor.kidCanInteract)
        //{
        //    FindObjectOfType<AI.AISystem>().OnStay(transform.position);
        //}
    }
}
