using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    //public static DialogueManager Instance;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    [SerializeField] private DialogueSoundHandler CharacterSoundHandler = null;

    private CanvasGroup canvasGroup;
    private Queue<Dialogue> dialogueQueue;
    private DialogueTrigger currentTrigger;
    private float timeShaking;
    private RectTransform rectTransform;
    private bool isShaking;
    private bool isCurrentlyPlayingDialogue;
    private Vector3 originalPosition;

    //private void Awake()
    //{
    //    if (Instance == null) Instance = this;
    //}

    private void Awake()
    {
        dialogueQueue = new Queue<Dialogue>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void StartDialogue(Dialogue[] dialogue, DialogueTrigger trigger)
    {
        if (isCurrentlyPlayingDialogue)
        {
            if (trigger.SkipQueue) return;
            foreach (Dialogue dialogueText in dialogue)
            {
                if (!dialogueQueue.Contains(dialogueText))
                {
                    dialogueQueue.Enqueue(dialogueText);
                }
            }
            return;
        }
        gameObject.SetActive(true);
        StopAllCoroutines();

        currentTrigger = trigger;
        if (!currentTrigger.IsTimed)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        continueButton.gameObject.SetActive(!trigger.IsTimed);
        canvasGroup.alpha = 1;

        dialogueQueue.Clear();
        foreach (Dialogue dialogueText in dialogue)
        {
            if (!dialogueQueue.Contains(dialogueText))
            {
                dialogueQueue.Enqueue(dialogueText);
            }
        }

        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        if (currentTrigger.IsEndingCutscene)
            HideButton();

        if (isShaking)
        {
            isShaking = false;
            rectTransform.anchoredPosition = originalPosition;
        }
        StopAllCoroutines();
        if (dialogueQueue.Count > 0)
        {
            isCurrentlyPlayingDialogue = true;
            StartCoroutine(TypeDialogue(dialogueQueue.Dequeue()));
        }
        else
        {
            isCurrentlyPlayingDialogue = false;
            EndDialogue();
        }
    }


    IEnumerator TypeDialogue(Dialogue dialogue)
    {
        float typeSpeed = 1 / dialogue.typeSpeed;
        float waitTime = dialogue.pauseSpeed;
        nameText.text = dialogue.speaker.nameText;
        nameText.color = dialogue.speaker.color;
        dialogueText.text = "";

        if (CharacterSoundHandler != null)
            CharacterSoundHandler.playCharacterSound(dialogue.speaker.nameText);
        int charCounter = 0;
        int maxChars = dialogue.dialogueText.ToCharArray().Length;
        foreach (char letter in dialogue.dialogueText.ToCharArray())
        {
            charCounter++;
            if (isShaking)
            {
                ShakeDialogueWindow();
            }
            if (letter == '#')
            {
                yield return new WaitForSeconds(waitTime);
                continue;
            }
            if (letter == '%')
            {
                if (currentTrigger.ShouldShake)
                    isShaking = true;
                yield return new WaitForSeconds(typeSpeed);
                continue;
            }

            if (letter == '*')
            {
                if (currentTrigger.IsEndingCutscene)
                {
                    EndCutscene.NextScene();
                }

                yield return new WaitForSeconds(typeSpeed);
                continue;
            }

            dialogueText.text += letter;
            if (currentTrigger.IsEndingCutscene && charCounter == maxChars - 2)
            {
                ShowButton();
            }

            yield return new WaitForSeconds(typeSpeed);
        }

        if (currentTrigger.IsTimed)
        {
            yield return new WaitForSeconds(currentTrigger.TimeBetweenDialogues);
            DisplayNextDialogue();
        }
    }

    private void ShowButton()
    {
        continueButton.gameObject.SetActive(true);
    }
    private void HideButton()
    {
        continueButton.gameObject.SetActive(false);
    }

    void ShakeDialogueWindow()
    {
        rectTransform.anchoredPosition = originalPosition + (Vector3)Random.insideUnitCircle * currentTrigger.ShakeIntensity;
        timeShaking += Time.deltaTime;
        if (!(timeShaking >= currentTrigger.ShakeTime)) return;
        rectTransform.anchoredPosition = originalPosition;
        isShaking = false;
    }

    private void EndDialogue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canvasGroup.alpha = 0;
        if (currentTrigger.IsEndingCutscene)
        {
            EndCutscene.NextScene();
        }
        currentTrigger = null;
        gameObject.SetActive(false);
    }
}

