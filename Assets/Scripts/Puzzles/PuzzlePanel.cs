using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzlePanel : MonoBehaviour {

    [SerializeField]
	protected Text[] textNumbers;
    [SerializeField]
    protected string sequencename;

    protected PlayerController2D playerController;
	protected Sequencer sequencer;
	protected MessageDialogController messageDialogController;
	
	protected WindowsBehavior windowsBehavior;

	protected void Awake()
	{
		playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController2D>();
		sequencer = GameObject.FindWithTag("Sequencer").GetComponent<Sequencer>();
		messageDialogController = GameObject.FindWithTag ("GUIController").GetComponent<MessageDialogController> ();
		playerController.canMove = false;
		windowsBehavior = GameObject.FindWithTag ("GameController").GetComponent<WindowsBehavior> ();

	}


	public IEnumerator SelectButton(GameObject go)
	{

		EventSystem.current.SetSelectedGameObject(null);
		yield return null;
		if ( go != null)
			EventSystem.current.SetSelectedGameObject(go);
		
	}

	protected void OnEnable()
	{
		playerController.canMove = false;
		StartCoroutine ("SelectButton", textNumbers [0].gameObject);
	}

	protected void OnDisable()
	{
		playerController.canMove = true;
	}

	protected IEnumerator ShowFailMessage(){
		yield return null;
		CharacterStats charStats = playerController.gameObject.GetComponent<PlayableCharacter> ().charStats;
		MessageDialog msg1 = new MessageDialog(charStats.avatar, charStats.name, "Puzzle.Fail", true);
		List<MessageDialog> messageList = new List<MessageDialog> ();
		messageList.Add (msg1);
		messageDialogController.SetMessagesArray (messageList);
		messageDialogController.ShowMessage ();

		yield return null;
		gameObject.SetActive(false);

	}
}
