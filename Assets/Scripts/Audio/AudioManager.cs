using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioMixerGroup output;
    [Range(0,1)]public float volume = 1;
    public bool is3D = true;
    private int lastCLipIndex;

    private AudioSource source;

    public void Play()
    {
        if (source == null) return;


        source.spatialBlend = is3D ? 1 : 0;
        source.outputAudioMixerGroup = output;
        if (audioClips.Length > 1)
        {
            lastCLipIndex = -1;
        }
        int nextClipIndex = Random.Range(0, audioClips.Length);
        if (nextClipIndex == lastCLipIndex)
        {
            if (nextClipIndex == audioClips.Length)
                nextClipIndex--;
            else 
                nextClipIndex++;
        }
        source.PlayOneShot(audioClips[nextClipIndex], volume);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (source == null)
            source = gameObject.AddComponent<AudioSource>();
    }
}
