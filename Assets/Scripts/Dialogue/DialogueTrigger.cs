using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] private bool isTimed;
	[SerializeField] private bool triggerOnce;
	[SerializeField] private bool isEndingCutscene;
	[SerializeField][Range(1f, 10f)] private float timeBetweenDialogues = 2.5f;
	[SerializeField] private bool skipQueue;
	public Dialogue[] dialogue;

	[Header("Extra Effects")] [SerializeField]
	private bool shouldShake;
	[SerializeField] private float shakeIntensity = 0.7f;
	[SerializeField] private float shakeTime = 0.5f;
    public bool IsTimed => isTimed;
    public float TimeBetweenDialogues => timeBetweenDialogues;
    public bool IsEndingCutscene => isEndingCutscene;
    public bool ShouldShake => shouldShake;
    public float ShakeIntensity => shakeIntensity;
    public float ShakeTime => shakeTime;
    public bool SkipQueue => skipQueue;


    private bool hasTriggered;

	[ContextMenu("TriggerDialogue")]



    public void TriggerDialogue()
	{
		if (triggerOnce && hasTriggered)
			return;

        UIManager.DialogueManager.StartDialogue(dialogue, this);
		hasTriggered = true;
	}

    public void TriggerPlayerBlockingDialogue()
    {
        Dialogue[] tempDialogue = new Dialogue[1];
        tempDialogue[0] = dialogue[3];
        UIManager.DialogueManager.StartDialogue(tempDialogue, this);
    }

	public void TriggerRandomDialogue()
	{
		Dialogue[] tempDialogue = new Dialogue[1];
		tempDialogue[0] = dialogue[Random.Range(0, dialogue.Length - 1)];
		UIManager.DialogueManager.StartDialogue(tempDialogue, this);
	}

	public void TriggerIfCanReach(/*AI.AISystem Ai*/)
	{
		//if (Ai.CanReachDestination(transform.position))
		//{
		//	TriggerDialogue();
		//}
	}
  }
