using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class CoroutineData
{
    public MonoBehaviour script;
    public string coroutineName;
    public GameObject parameterGameObject; 
}

public enum TransitionType { TRIGGER, BOOL, INT, FLOAT }
[Serializable]
public class AnimationData{
   
    public TransitionType animationTransitionType;
	public Animator animator;
	public string paramName;
    public bool boolValue;
    public CoroutineData coroutine;
    public List<MessageDialog> messageDialogList;
    public Character character;
     
}

[Serializable]
public class SequenceSteps{
	public List<AnimationData> animatonData = new List<AnimationData> ();
}

public class Sequencer : MonoBehaviour {
	[Serializable]
	public class SequenceData
	{	
		public string sequenceName;
		public List<SequenceSteps> sequenceSteps = new List<SequenceSteps> ();
	}

	public List<SequenceData> sequences = new List<SequenceData> ();

	public bool isOnSequence = false;
    MessageDialogController dialogController;

	SequenceData currentSequence;
	List<SequenceSteps> currentSequenceSteps;
	
	int indexStep;

    public bool enableSequences;
    public bool testSequence = false;
    public string testSequenceName;

    public bool testNextStep = false;
    bool updateCanGetEncounter = false;

    void Update()
    {
        if (testSequence)
        {
            StartCoroutine(StartSequence(testSequenceName));
            testSequence = false;
        }
        if (testNextStep)
        {
            PerformNextSequenceStep();
            testNextStep = false;
        }
    }

	public IEnumerator StartSequence(string sequenceName){

        //Bloquear player controller e indicar al sistema de mensajes que puede desbloquear el siguiente paso
        yield return null;
        if (GameManager.instance.canGetEncounter)
        {
            updateCanGetEncounter = true;
            GameManager.instance.canGetEncounter = false;
        }
       
        GameManager.instance.canGetEncounter = false;
        indexStep = 0;
        isOnSequence = true;
        dialogController = GameObject.FindWithTag("GUIController").GetComponent<MessageDialogController>();
        //Buscar la secuencia que buscamos
        currentSequence = sequences.Find(p=>p.sequenceName.Equals(sequenceName));
		currentSequenceSteps = currentSequence.sequenceSteps;
        PerformNextSequenceStep();
       
    }

	public void PerformNextSequenceStep(){
        if (isOnSequence)
        {
            if (indexStep < currentSequenceSteps.Count)
            {
                List<AnimationData> animDataList = currentSequenceSteps[indexStep].animatonData;

                foreach (AnimationData animData in animDataList)
                {
                   
                    if (animData.animator != null)
                        if (animData.animationTransitionType.Equals(TransitionType.TRIGGER))
                            animData.animator.SetTrigger(animData.paramName);
                        else if (animData.animationTransitionType.Equals(TransitionType.BOOL))
                            animData.animator.SetBool(animData.paramName, animData.boolValue);

                    if (animData.coroutine.script != null)
                    {
                        animData.coroutine.script.StartCoroutine(animData.coroutine.coroutineName, animData.coroutine.parameterGameObject);
                    }
                    if (animData.messageDialogList != null && animData.messageDialogList.Count > 0)
                    {
                        ManageSequenceStepDialog(animData);
                    }
                }

                indexStep++;
            }else
                FinishSequence();

        }
    }

    private void ManageSequenceStepDialog(AnimationData animData)
    {
        List<MessageDialog> msgList = new List<MessageDialog>();
        foreach (MessageDialog msg in animData.messageDialogList)
        {
            if (animData.character != null)
            {
                Character animCharacter = animData.character;
                MessageDialog simpleMessage = new MessageDialog(animCharacter.charStats.avatar, animCharacter.charStats.name, msg.dialogText, msg.nextSequenceStep);
                msgList.Add(simpleMessage);
            }
            else if (msg.avatarImage != null)
            {
                msgList.Add(msg);
            }
            else
            {
                MessageDialog simpleMessage = new MessageDialog();
                simpleMessage.dialogText = msg.dialogText;
                simpleMessage.nextSequenceStep = msg.nextSequenceStep;
                msgList.Add(simpleMessage);
            }

        }
        dialogController.SetMessagesArray(msgList);
        dialogController.ShowMessage();
    }

    public void FinishSequence(){

        indexStep = 0;
       
		isOnSequence = false;
        if (updateCanGetEncounter)
            GameManager.instance.canGetEncounter = true;
    }
}
