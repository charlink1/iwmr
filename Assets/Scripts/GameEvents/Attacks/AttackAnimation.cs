using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour {

    [SerializeField]
    AudioSource animationAudio;

    float animLength;

    public float GetAnimationLength()
    {
        return animLength;
    }
   void OnEnable()
    {
        AnimatorClipInfo[] clipInfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        if(animationAudio!= null)
            animationAudio.Play();
        animLength = clipInfo[0].clip.length;
        Destroy(gameObject, animLength);
    }
}
