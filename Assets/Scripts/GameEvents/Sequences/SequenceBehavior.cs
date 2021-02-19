using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceBehavior : MonoBehaviour {

    [SerializeField]
    Sequencer sequencer;
    [SerializeField]
    AudioSource audioSource;

    void CallNextStep()
    {
        if (sequencer.isOnSequence)
            sequencer.PerformNextSequenceStep();
    }

    public void StartAudio()
    {
        audioSource.time = 1;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
