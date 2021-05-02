using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Dialogue
{
   public DialogueSpeaker speaker;
   [Range(1f, 100f)]public float typeSpeed = 20f;
   [Tooltip("Time in seconds to wait for every # symbol in dialogue")][Range(0.1f, 2f)]public float pauseSpeed = 0.25f;
   [TextArea(3, 10)] public string dialogueText;
}