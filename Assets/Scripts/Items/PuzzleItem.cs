using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItem : QuestItem {

	[SerializeField]
	Sprite[] imageNumbers;
    GameController gameController;

	public override void AddItem (int amount = 1, bool showDialog = true)
	{
		base.AddItem (amount, showDialog);
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		Game.piecesOfNumber ++;
		if (Game.piecesOfNumber == 1) {
            GameManager.instance.canGetEncounter = true;
            GameObject.FindWithTag ("Sequencer").GetComponent<Sequencer> ().StartCoroutine("StartSequence", "boyAppears");
		} else if (Game.piecesOfNumber == 2) {
			GameObject.FindWithTag ("GameController").GetComponent<WindowsBehavior> ().ChangeWindowColor (0);
			//Activar animacion cajonera
		}
		else if (Game.piecesOfNumber == 3) {
			Game.puzzlesSolved++;
            gameController.CheckPuzzlesSolved();
		}

		itemImage = imageNumbers [Game.code [0]];
			
	}
}
