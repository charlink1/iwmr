using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordPuzzle : PuzzlePanel {

    [SerializeField]
    HiddenPanelTrigger hiddenPannelTrigger;

	void Update()
	{
		if (/*Game.puzzleBooksFinished && */!Game.puzzlePasswordFinished && Input.GetButtonDown(OSInputManager.GetPadMapping("Submit"))){

			if (CheckPassword())
			{
                #if UNITY_EDITOR
                    Debug.Log("Password puzzle done");
                #endif
                sequencer.StartCoroutine("StartSequence",sequencename);

				//Indicar que ya se ha obtenido ese numero
				Game.puzzlePasswordFinished = true;
				gameObject.SetActive(false);
			}
			else
				GameManager.instance.attacked = true;
			
		}
		if (Input.GetButtonDown(OSInputManager.GetPadMapping("Cancel"))){
            hiddenPannelTrigger.StartCoroutine("ReleaseMessages");
			gameObject.SetActive(false);
		}
	}

	bool CheckPassword(){
		for (int i = 0; i < textNumbers.Length; ++i) {
			if (!int.Parse (textNumbers [i].text).Equals (Game.code [i]))
				return false;
			
		}
		return true;
	}

	public IEnumerator InstantiateBoss(GameObject boss){
		yield return null;
		Instantiate (boss);
	}
		
}
