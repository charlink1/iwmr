using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTrigger : MonoBehaviour {

    [SerializeField]
    string boyName;
    [SerializeField]
    GameObject clockPuzzlePanel;
    [SerializeField]
    GameObject player;
    [SerializeField]
	string sequenceName;
    [SerializeField]
    Sequencer sequencer;
    [SerializeField]
    MessageDialogController msgController;

    CharacterStats girlStats;
    Animator animator;

    bool playerInside;
	bool waitingUntilNewMessage = false;

    void Awake(){
      
        girlStats = player.GetComponent<PlayableCharacter> ().charStats;	
        animator = player.GetComponent<Animator>();
        
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

    void Update(){
      
        if (playerInside) {
           
            if (Input.GetButtonDown(OSInputManager.GetPadMapping("Submit")) && animator.GetFloat("y") == 1f && !msgController.IsDialogActive) {
				//Caso 1: Niño aparece y no ha ocurrido el evento de "mirar el reloj tontamente"
				if (Game.boyAppeared && !Game.conversationAboutClockDone) {
					Game.conversationAboutClockDone = true;
					CharacterStats boyStats = CharacterParty.charactersParty.Find (p => p.charStats.name.Equals (boyName)).charStats;

					List<MessageDialog> msgList = new List<MessageDialog> ();
                    MessageDialog boyMsg1 = new MessageDialog(boyStats.avatar, boyStats.name, "ClockConversation.Part1");
                    MessageDialog girlMsg1 = new MessageDialog (girlStats.avatar, girlStats.name, "ClockConversation.Part2");
                    MessageDialog boyMsg2 = new MessageDialog(boyStats.avatar, boyStats.name, "ClockConversation.Part3");

                    msgList.Add(boyMsg1);
                    msgList.Add (girlMsg1);
                    msgList.Add(boyMsg2);


                    if (!Game.puzzleClockFinished) {
						MessageDialog girlMsg2 = new MessageDialog (girlStats.avatar, girlStats.name, "ClockConversation.Part4");
						msgList.Add (girlMsg2);
					}
					
					msgController.SetMessagesArray (msgList);
                    msgController.ShowMessage();
                    
                }//Caso 2: Puzle del reloj
				else if(!Game.puzzleClockFinished && !clockPuzzlePanel.activeSelf && !sequencer.isOnSequence &&!waitingUntilNewMessage &&!msgController.IsDialogActive && msgController.releaseConversation){
					waitingUntilNewMessage = true;
					sequencer.StartCoroutine("StartSequence", sequenceName);
				}

			}
		}
	}

	public IEnumerator ReleaseMessages(){
		yield return new WaitForSeconds(1); ;
		waitingUntilNewMessage = false;
        if (sequencer.isOnSequence)
        {
            yield return null;
            sequencer.PerformNextSequenceStep();
        }
	}
}
