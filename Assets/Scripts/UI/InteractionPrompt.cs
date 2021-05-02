using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    public TextMeshProUGUI interactionText;

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ShowPrompt(string text)
    {
        interactionText.text = text;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
