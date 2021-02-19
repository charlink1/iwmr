using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDialogBehavior : Menu {

    [SerializeField]
    Sequencer sequencer;
    [SerializeField]
    PlayerController2D controller;
    [SerializeField]
    Button initButton;

    [SerializeField]
    ChestBehaviour chest;

    void OnEnable()
    {
        GameManager.instance.canGetEncounter = false;
        controller.canMove = false;
        controller.canJump = false;
        chest.isDialogEnabled = true;
        StartCoroutine("SelectButton", initButton);
    }

    public void StartSequence(string sequence)
    {
        GameManager.instance.canGetEncounter = true;
        sequencer.StartCoroutine("StartSequence", sequence);
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        chest.buttonsDisabled = false;
        controller.canJump = true;
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        //  GameManager.instance.canGetEncounter = true;
        chest.StartCoroutine("DisabledDialog");
        controller.canMove = true;
    }

    IEnumerator PlayerWaitNextFrame()
    {
        yield return null;
        controller.canJump = true;
    }
}
