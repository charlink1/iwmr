using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockPuzzle : PuzzlePanel {

    GameController gameController;

    [SerializeField]
    ClockTrigger clockTrigger;
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (!Game.puzzleClockFinished && Input.GetButtonDown(OSInputManager.GetPadMapping("Submit"))){

            if (CheckTime())
            {
                //Iniciar secuencia de obtención del numero
                //Mensaje de que se ha abierto por detrás y hay algo dentro
                Debug.Log("Clock puzzle done");

			
				Game.puzzlesSolved++;
                gameController.CheckPuzzlesSolved();
                //Entregar item del número correspondiente
                sequencer.StartCoroutine("StartSequence",sequencename);

                //Indicar que ya se ha obtenido ese numero
                Game.puzzleClockFinished = true;
                gameObject.SetActive(false);
            }
			else 
				sequencer.StartCoroutine("StartSequence","failClockPuzzle");
			
        }
        if (Input.GetButtonDown(OSInputManager.GetPadMapping("Cancel"))){
            clockTrigger.StartCoroutine("ReleaseMessages");
            gameObject.SetActive(false);
        }
    }

    bool CheckTime()
    {
        string currentTime = DateTime.Now.ToString("HH : mm");
        string puzzleTime = textNumbers[0].text + textNumbers[1].text + " : " + textNumbers[2].text + textNumbers[3].text;

        if (currentTime.Equals(puzzleTime))
            return true;

        return false;
    }

}
