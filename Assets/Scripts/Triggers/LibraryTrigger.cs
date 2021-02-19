using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryTrigger : MonoBehaviour {

    [SerializeField]
    string sequenceName;
    [SerializeField]
    GameObject lirbaryPuzzlePanel;
    [SerializeField]
    Sequencer sequencer;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject GUIControllerGO;
    [SerializeField]
    Collider2D auxCollider;

    Animator animator;
    MessageDialogController msgController;
    CharacterStats girlStats;
    PlayerController2D playerController;
    GUIController guiController; 

    bool waitingUntilNewMessage = false;
    bool playerInside;
    bool libraryLocked;
    bool chestInside = false;


    void Awake()
    {
        playerController = player.GetComponent<PlayerController2D>();
        girlStats = player.GetComponent<PlayableCharacter>().charStats;
        animator = player.GetComponent<Animator>();
        msgController = GUIControllerGO.GetComponent<MessageDialogController>();
        guiController = GUIControllerGO.GetComponent<GUIController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Chest"))
        {
            chestInside = true;
            auxCollider.enabled = true;
        }

        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Chest"))
        {
            chestInside = false;
            auxCollider.enabled = false;
        }
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    void Update()
    {
        if(playerInside && Input.GetButtonDown(OSInputManager.GetPadMapping("Submit")) && animator.GetFloat("y") == 1f && !msgController.IsDialogActive && !guiController.IsMenuActive)
             StartCoroutine("ManageButtonDown");

    }


    IEnumerator ManageButtonDown()
    {
       
        //Si el cofre no está dentro mensaje de que hay libros desordenados pero no llega
        if ((!chestInside || !playerController.isOnChest) && !msgController.IsDialogActive && !waitingUntilNewMessage)
        {
            //waitingUntilNewMessage = true;
            if (!libraryLocked)
            {
                libraryLocked = true;
                MessageDialog msg = new MessageDialog(girlStats.avatar, girlStats.name, "Library.cantReach");
                List<MessageDialog> msgList = new List<MessageDialog>();
                msgList.Add(msg);
                msgController.SetMessagesArray(msgList);
                msgController.ShowMessage();
            }
            else
                libraryLocked = false;

        }
        else if (!Game.puzzleBooksFinished && playerController.isOnChest && !sequencer.isOnSequence && !waitingUntilNewMessage && !msgController.IsDialogActive)
        {
            //Si el cofre está dentro y ella está encima y no se ha hecho el puzle de la libreria iniciar puzle de la libreria mediante secuencia
            waitingUntilNewMessage = true;
            lirbaryPuzzlePanel.SetActive(true);
        }
        yield return null;
    }

    IEnumerator ReleaseMessage()
    {
        yield return null;
        waitingUntilNewMessage = false;
    }

    public IEnumerator ReleaseMessages()
    {
        yield return null;
        waitingUntilNewMessage = false;
        if (sequencer.isOnSequence)
        {
            yield return null;
            sequencer.PerformNextSequenceStep();
        }
    }
}
