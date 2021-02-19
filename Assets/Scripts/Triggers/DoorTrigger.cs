using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

   
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    GameObject player;
    [SerializeField]
    MessageDialogController msgController;
    [SerializeField]
    Sequencer sequencer;
    [SerializeField]
    string messageOnIntro;
    [SerializeField]
    string messageOnTrigger;
    
    PlayerController2D playerController;
    CharacterStats playerStats;
   

    bool playerInside = false;
    bool doorLocked = false;

    void Awake()
    {
		playerStats = player.GetComponent<PlayableCharacter>().charStats;
		playerController = player.GetComponent<PlayerController2D> ();
       
    }

	public IEnumerator DoorLocked()
    {
        //PlaySound
        audioSource.Play();
        yield return new WaitForSeconds(1);
        MessageDialog msg = null;
        if (sequencer.isOnSequence)
            msg = new MessageDialog(playerStats.avatar, playerStats.name, messageOnIntro, true);
        else
            msg = new MessageDialog(playerStats.avatar, playerStats.name, messageOnTrigger, false);

        List < MessageDialog > msgList = new List<MessageDialog>();
        msgList.Add(msg);
        msgController.SetMessagesArray(msgList);
        msgController.ShowMessage();

    }

    void Update()
    {
        if (playerInside && Input.GetButtonDown(OSInputManager.GetPadMapping("Submit")) && !sequencer.isOnSequence && !msgController.IsDialogActive && playerController.GetAnimatorAxisValue("y") == 1)
        {
            if (!doorLocked)
            {
                doorLocked = true;
                StartCoroutine("DoorLocked");
            }
            else
                doorLocked = false;
        }
    }

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Player")) 
            playerInside = true;

	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))     
            playerInside = false;

    }

     IEnumerator ReleaseDoor()
    {
        yield return null;
        doorLocked = false;
    }

}
