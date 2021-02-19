using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPanelTrigger : MonoBehaviour {

    [SerializeField]
    Sequencer sequencer;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject guiControllerGO;
    [SerializeField]
    GameObject puzzlePanel;

    [SerializeField]
    string sequenceName;

    bool waitingUntilNewMessage = false;
    bool playerInside = false;

    PlayerController2D playerController;
    MessageDialogController msgController;
    GUIController guiController;    
    Animator playerAnimator;

    void Awake()
    {
        playerController = player.GetComponent<PlayerController2D>();
        playerAnimator = player.GetComponent<Animator>();
           
        msgController = guiControllerGO.GetComponent<MessageDialogController>();
        guiController = guiControllerGO.GetComponent<GUIController>();
    }

    void Update()
    {
        if (playerInside && (Input.GetButtonDown(OSInputManager.GetPadMapping("Submit"))) && playerAnimator.GetFloat("y") == 1 && !guiController.IsMenuActive)
        {
            StartCoroutine("InitPuzzle");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    public IEnumerator InitPuzzle()
    {
        yield return null;
        if (!Game.puzzlePasswordFinished && !puzzlePanel.activeSelf && !sequencer.isOnSequence && !waitingUntilNewMessage && !msgController.IsDialogActive)
        {
            playerController.StopPlayerMove();
            waitingUntilNewMessage = true;
            sequencer.StartCoroutine("StartSequence", sequenceName);
        }
    }
    public IEnumerator ReleaseMessages()
    {
        yield return null;
        waitingUntilNewMessage = false;
        yield return null;
        if(sequencer.isOnSequence)
            sequencer.PerformNextSequenceStep();
    }
}
