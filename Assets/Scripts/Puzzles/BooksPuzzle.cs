using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooksPuzzle : PuzzlePanel {
    [SerializeField]
    LibraryTrigger libraryTrigger;
	[SerializeField]
	string order;

	void Update()
	{
		if (!Game.puzzleBooksFinished && Input.GetButtonDown(OSInputManager.GetPadMapping("Submit"))){

			if (CheckSortOrder ()) {
                #if UNITY_EDITOR
                    Debug.Log ("Books puzzle done");
                #endif
                sequencer.StartCoroutine ("StartSequence", sequencename);
				Game.puzzleBooksFinished = true;
				gameObject.SetActive (false);
			} else
                sequencer.StartCoroutine("StartSequence", "failLibraryPuzzle");
            
		}
        if (Input.GetButtonDown(OSInputManager.GetPadMapping("Cancel")))
        {
            libraryTrigger.StartCoroutine("ReleaseMessages");
            gameObject.SetActive(false);
        }
	}

	bool CheckSortOrder(){
		string text = "";
		for (int i = 0; i < textNumbers.Length; ++i) {
			text += textNumbers [i].text;
		}
		if (text.Equals (order))
			return true;

		return false;
	}


		

}
