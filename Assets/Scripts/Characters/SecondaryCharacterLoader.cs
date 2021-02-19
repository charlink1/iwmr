using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryCharacterLoader : MonoBehaviour {

    PlayableCharacter character;

    public bool boyAppeared;

	void Start () {
        character = GetComponent<PlayableCharacter>();
        if (Game.boyAppeared && !character.enabled)
            character.enabled = true;	
	}
	
    void Update()
    {
        if (!Game.boyAppeared && boyAppeared)
            Game.boyAppeared = true;
    }
	
}
