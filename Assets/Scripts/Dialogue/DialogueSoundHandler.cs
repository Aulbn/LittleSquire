using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSoundHandler : MonoBehaviour
{
    AudioSource SpeakSource = null;
    [SerializeField] private AudioClip[] ElenDegenClips;
    [SerializeField] private AudioClip[] CedricClips;
    [SerializeField] private float MinimunPitch = 1f, MaximumPitch = 1f;
    bool isWomanSpeaking = false;
    [HideInInspector]
    public bool isCutscene = false;

    private void Awake()
    {
        SpeakSource = gameObject.GetComponent<AudioSource>();
    }

    public void CutsceneWithMusicStart()
    {
        isCutscene = true;
        Debug.Log(isCutscene);
    }

    public void CutsceneWithMusicEnd()
    {
        isCutscene = false;
    }

    public void playCharacterSound(string characterName)
    {
        if (SpeakSource)
        {
            if (!isCutscene)
            {

                if (characterName == "ELEANOR")
                {
                    int c = Random.Range(0, ElenDegenClips.Length);
                    float p = Random.Range(MinimunPitch, MaximumPitch);
                    SpeakSource.pitch = p;
                    SpeakSource.PlayOneShot(ElenDegenClips[c]);
                }
                else
                {
                    int c = Random.Range(0, ElenDegenClips.Length);
                    float p = Random.Range(MinimunPitch, MaximumPitch);
                    SpeakSource.pitch = p;
                    SpeakSource.PlayOneShot(CedricClips[c]);
                }
            }
        }
        else
            Debug.Log("ya'll missing the Audio Source for the DialogueSoundHandler");
    }
}
