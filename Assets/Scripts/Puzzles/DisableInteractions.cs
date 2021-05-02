using UnityEngine;

public class DisableInteractions : MonoBehaviour
{
    [SerializeField] private GameObject[] gameobjectsArray;
    [Space(10)]
    [SerializeField] private GameObject finalGate = null;

    PuzzleGateController gateController = null;

    private void Awake()
    {
        gateController = finalGate.GetComponent<PuzzleGateController>();
    }

    public void CheckGateBool()
    {
        if (gateController.playGateSound.isPlaying)
        {
            DisableTheInteractions();
        }

    }

    public void DisableTheInteractions()
    {
        for (int i = 0; i < gameobjectsArray.Length; i++)
        {
            BoxCollider triggerArea = gameobjectsArray[i].gameObject.GetComponent<BoxCollider>();
            if (triggerArea.isTrigger)
            {
                triggerArea.enabled = false;
            }
        }

    }
}
