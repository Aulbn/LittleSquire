using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    [SerializeField] private LayerMask playerInteractionLayerMask;
    [SerializeField] private LayerMask kidInteractionLayerMask;
    [SerializeField] private LayerMask ignoreLayerMask;

    //[SerializeField] private GameObject interactPrompt;
    //[SerializeField] private Text interactText;

    //private Player player;
    private Camera camera;
    private Vector3 interactablePosition;
    //private CharacterInteraction characterInteraction;
    private IInteract currentInteractable = null;
    private void Start()
    {
        //player = GetComponent<Player>();
        camera = Camera.main;
        //characterInteraction = GetComponent<CharacterInteraction>();
    }

    private void LateUpdate()
    {
        //if (player.IsHoldingBox())
        //{
        //    //characterInteraction.canInteract = true;
        //    ShowPrompt(transform.position + (transform.forward * 2f), "Release  \nE");
        //    return;
        //}

        //if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitInfo, 3f, playerInteractionLayerMask))
        //{

        //    PuzzleBoxController box = hitInfo.collider.gameObject.GetComponent<PuzzleBoxController>();
        //    if (box != null)
        //    {
        //        characterInteraction.canInteract = true;
        //        ShowPrompt(box.transform.position, "Interact \nE");
        //        currentInteractable = box;
        //        characterInteraction.SetPushingDirection(hitInfo.normal);
        //        return;
        //    }

        //    PuzzleLeverController lever = hitInfo.collider.gameObject.GetComponent<PuzzleLeverController>();
        //    if (lever != null)
        //    {
        //        characterInteraction.canInteract = true;
        //        ShowPrompt(lever.transform.position, "Interact \nE");
        //        currentInteractable = lever;
        //    }
        //}
        ////else if (Physics.SphereCast(camera.transform.position, 0.1f, camera.transform.forward, out RaycastHit cameraHitInfo,
        ////    100f, kidInteractionLayerMask))
        //    else if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit cameraHitInfo,
        //    100f, kidInteractionLayerMask))
        //{
        //    Vector3 direction = camera.transform.position - cameraHitInfo.point;
        //    if (Physics.Raycast(cameraHitInfo.point + direction.normalized, direction.normalized, out RaycastHit info, direction.magnitude, ignoreLayerMask))
        //    {
        //        characterInteraction.kidCanInteract = false;
        //        UIManager.InteractionPrompt.HidePrompt();
        //        currentInteractable = null;
        //        return;
        //    }
        //    PuzzleLeverController lever = cameraHitInfo.collider.gameObject.GetComponent<PuzzleLeverController>();
        //    if (lever != null)
        //    {
        //        characterInteraction.kidCanInteract = true;
        //        ShowPrompt(lever.transform.position, "Cedric \nE");
        //        currentInteractable = lever;
        //    }

        //    CrawlController crawl = cameraHitInfo.collider.gameObject.GetComponent<CrawlController>();
        //    if (crawl != null)
        //    {
        //        characterInteraction.kidCanInteract = true;
        //        ShowPrompt(crawl.transform.position, "Cedric \nE");
        //    }
        //    Interactable interactable = cameraHitInfo.collider.gameObject.GetComponent<Interactable>();
        //    if (interactable != null)
        //    {
        //        string temp = interactable.promptText + "\nE";
        //        characterInteraction.kidCanInteract = true;
        //        ShowPrompt(interactable.transform.position, temp);
        //        currentInteractable = interactable;
        //    }
        //}
        //else
        //{
        //    characterInteraction.kidCanInteract = false;
        //    characterInteraction.canInteract = false;
        //    UIManager.InteractionPrompt.HidePrompt();
        //    currentInteractable = null;
        //}
    }

    private void ShowPrompt(Vector3 position, string promptText)
    {
        Vector3 worldToScreenPosition = camera.WorldToScreenPoint(position + Vector3.up);
        UIManager.InteractionPrompt.SetPosition(worldToScreenPosition);
        UIManager.InteractionPrompt.ShowPrompt(promptText);
    }

    public IInteract GetCurrentInteractable()
    {
        return currentInteractable;
    }
}
