using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnBridgeCheck : MonoBehaviour
{
    private GameObject playerObject = null;
    private CharacterInteraction InteractionController = null;

    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        InteractionController = playerObject.GetComponent<CharacterInteraction>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionController.isOnPlatform = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionController.isOnPlatform = false;
        }
    }
}