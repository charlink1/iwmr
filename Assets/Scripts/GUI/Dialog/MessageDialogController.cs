using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using SmartLocalization;
using System;
using UnityEngine.EventSystems;

[Serializable]
public class MessageDialog
{
   

    public string dialogText;

    public bool nextSequenceStep = false;
    public bool keepOtherDialogActive = false;

    public Sprite avatarImage { get; set; }
    public string avatarName {get; set;}

    public MessageDialog() {}

    public MessageDialog(Sprite sprite, string characterName, string quote, bool callNextSequenceStep = false)
    {
        avatarImage = sprite;
        avatarName = characterName;
        dialogText = quote;
        nextSequenceStep = callNextSequenceStep;
    }
    
}

public class MessageDialogController: MonoBehaviour{

    [SerializeField]
    GameObject fullDialogMessagePanel;
    [SerializeField]
    GameObject simpleDialogMessagePanel;

    [SerializeField]
    Text textName;
    [SerializeField]
    Text textMessage;
    [SerializeField]
    Image spriteImage;

    [SerializeField]
    Text simpleTextMessage;

    [SerializeField]
    Sequencer sequencer;

    LanguageManager languageManager;
    List<MessageDialog> messagesArray = new List<MessageDialog>();

    int currentMessageIndex = 0;
    bool previouslyAttackable = false;

    public bool IsDialogActive { get; set; }   
    public bool releaseConversation{ get; set; }


    public void SetMessagesArray(List<MessageDialog> list)
    {
        messagesArray = list;
    }

    void Start()
    {
        languageManager = LanguageManager.Instance;
        IsDialogActive = false;
        releaseConversation = true;
        #if UNITY_EDITOR
            Debug.Log(languageManager.CurrentlyLoadedCulture.englishName);
        #endif
   
    }

    void Update()
    {
        if (IsDialogActive && Input.GetButtonDown(OSInputManager.GetPadMapping("Submit")))
            ShowNextMessage();
    }

    IEnumerator  ShowMessageCoroutine()
    {
        releaseConversation = false;
        previouslyAttackable = GameManager.instance.canGetEncounter;
        if (!sequencer.isOnSequence && previouslyAttackable)
            GameManager.instance.canGetEncounter = false;

        yield return null;
        if (!IsDialogActive)
            IsDialogActive = true;

        MessageDialog msg;
        Text textField;
        CreateText(out msg, out textField);

        if (textField != null)
            textField.text = languageManager.GetTextValue(msg.dialogText);
    }

    private void CreateText(out MessageDialog msg, out Text textField)
    {
        msg = messagesArray[currentMessageIndex];
        textField = null;
        if (msg.avatarImage != null)
        {
            textField = textMessage;
            if (!msg.keepOtherDialogActive)
                simpleDialogMessagePanel.SetActive(false);

            if (!fullDialogMessagePanel.activeSelf)
                fullDialogMessagePanel.SetActive(true);

            textName.text = msg.avatarName;
            spriteImage.sprite = msg.avatarImage;
        }
        else
        {
            textField = simpleTextMessage;
            if (!msg.keepOtherDialogActive)
                fullDialogMessagePanel.SetActive(false);

            if (!simpleDialogMessagePanel.activeSelf)
                simpleDialogMessagePanel.SetActive(true);
        }
    }

    public void ShowMessage()
    {
        StartCoroutine("ShowMessageCoroutine");
    }

    public void ShowNextMessage()
    {
        bool changeStep = messagesArray[currentMessageIndex].nextSequenceStep;
  
        currentMessageIndex++;
        if (currentMessageIndex < messagesArray.Count)
            ShowMessage();

        else
           StartCoroutine(FinishMessage());

        if (changeStep)
            sequencer.PerformNextSequenceStep();

    }
	
    IEnumerator FinishMessage()
    {
        currentMessageIndex = 0;
       // yield return null;
        if (IsDialogActive)
        {
            IsDialogActive = false;
            if (fullDialogMessagePanel.activeSelf)
                fullDialogMessagePanel.SetActive(false);
            if (simpleDialogMessagePanel.activeSelf)
                simpleDialogMessagePanel.SetActive(false);
        }
        messagesArray.Clear();
        if(!sequencer.isOnSequence && previouslyAttackable)
            GameManager.instance.canGetEncounter = true;
        yield return null;
        releaseConversation = true;
    }
}
