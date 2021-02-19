using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInteractiveElementTrigger : MonoBehaviour {


    [SerializeField]
    protected GameObject player;
    [SerializeField]
    protected MessageDialogController msgController;

    protected Animator animator;
    protected CharacterStats girlStats;

    protected bool playerInside = false;
    protected bool interacting = false;

    void Awake()
    {
        girlStats = player.GetComponent<PlayableCharacter>().charStats;
        animator = player.GetComponent<Animator>();
    }


    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        girlStats = player.GetComponent<PlayableCharacter>().charStats;
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        msgController = GameObject.FindWithTag("GUIController").GetComponent<MessageDialogController>();
    }

    protected void InteractShowingMessage(string key)
    {
        if (!interacting)
        {
            ShowMessage(key);
            interacting = true;
        }
        else
            interacting = false;
    }

    protected void ShowMessage(string key)
    {
        MessageDialog girlMsg2 = new MessageDialog(girlStats.avatar, girlStats.name, key);
        List<MessageDialog> msgList = new List<MessageDialog>();
        msgList.Add(girlMsg2);
        msgController.SetMessagesArray(msgList);
        msgController.ShowMessage();
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

}
