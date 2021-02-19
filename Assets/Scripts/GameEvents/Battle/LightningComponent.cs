using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningComponent : MonoBehaviour {


    public Animator anim { get; set; }
    AudioSource lightningAudio;

    Animator camAnim;

    void Start()
    {
        anim = GetComponent<Animator>();
        camAnim = Camera.main.GetComponent<Animator>();
        lightningAudio = GetComponent<AudioSource>();
    }

    public void StartBlur()
    {
        camAnim.SetTrigger("blurOn");
        
    }

    public void PlaySound(int play)
    {
        if (play==1)
            lightningAudio.Play();
        else
            lightningAudio.Stop();
    }
}
