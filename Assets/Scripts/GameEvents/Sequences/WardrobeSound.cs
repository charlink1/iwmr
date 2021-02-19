using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeSound : MonoBehaviour {

    [SerializeField]
    AudioSource closingDoorSound;

	public void PlayCloseSound()
    {
        closingDoorSound.Play();
    }
}
