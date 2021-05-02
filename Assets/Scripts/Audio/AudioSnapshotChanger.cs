using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSnapshotChanger : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioMixerSnapshot snapshot;
    public float transitionTime;
    public void ChangeSnapshot()
    {  
            snapshot.TransitionTo(transitionTime);          
    }
        
}
