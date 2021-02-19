using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryBehaviour : MonoBehaviour {

   

	void Awake () {
        if (Game.puzzleBooksFinished)
        {
            GetComponent<Animator>().enabled = false;
            transform.position = Game.libraryFinalPosition;
        }	
	}
	
	public void SetLibraryPosition(){
		Game.libraryFinalPosition = transform.position;
	}
}
