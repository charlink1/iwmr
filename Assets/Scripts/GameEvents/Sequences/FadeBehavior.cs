using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeBehavior : MonoBehaviour {

    [SerializeField]
    PlayerController2D playerController;

    [SerializeField]
    Sequencer sequencer;

    [SerializeField]
    string sequenceName;

    [SerializeField]
    GameObject finalMessagePanel;
    [SerializeField]
    string menuSceneName;
  
	public IEnumerator CheckFirstLoading()
    {
        if (!Game.firstSequenceDone)
        {
            yield return new WaitForSeconds(2);
            if(sequencer.enableSequences)
                sequencer.StartCoroutine("StartSequence",sequenceName);
            Game.firstSequenceDone = true;
        }
       else
       {
            playerController.canMove = true;
       }
    }

    public void ShowFinalMessage()
    {
        finalMessagePanel.SetActive(true);
    }


    public void ReturnToMenu(string menuScene)
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void DeactivateMove()
    {
        playerController.canMove = false;
    }
}
