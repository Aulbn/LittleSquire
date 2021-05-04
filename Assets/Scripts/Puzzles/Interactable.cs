using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteract
{
    public bool disableOnInteract;
    public string promptText;
    public UnityEvent onInteract;

    
    public void SetHover(bool isHovering)
    {
        if (isHovering)
        {
            UIManager.InteractionPrompt.ShowPrompt(promptText);

        }
        else
        {
            UIManager.InteractionPrompt.HidePrompt();
        }
    }



    void IInteract.Interact(/*CharacterInteraction interactor*/)
    {
        onInteract.Invoke();
        if (disableOnInteract)
            GetComponent<Collider>().enabled = false;
    }
}
