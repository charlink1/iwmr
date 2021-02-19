using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartLocalization;

public class ChestTrigger : MonoBehaviour {

    [SerializeField]
    string axis;
    [SerializeField]
    float value;
	[SerializeField]
	float valueOnChest;
    [SerializeField]
    string sequenceName;
    Sequencer sequencer;
    FixedJoint2D joint;
    [SerializeField]
    Sprite[] submitButtonImages;//0 joystick xbox, 1 teclado
    [SerializeField]
    Text submitButtonText;     
    [SerializeField]
    Sprite[] cancelButtonImages; //0 joystick xbox, 1 teclado

    [SerializeField]
    GameObject submitButtonImage; 
    [SerializeField]
    GameObject cancelButtonImage; 

    [SerializeField]
    Transform jumpPosition;
    [SerializeField]
    Transform returnToFloorTransform;
   
	[SerializeField]
	Collider2D rabbitCollider;

    [SerializeField]
    GameObject questionPanel;
    [SerializeField]
    bool disableWhenOnChest = false;
    [SerializeField]
    string m_JumpText;
    [SerializeField]
    string m_PushText;

    float defaultYPosition;
    [SerializeField]
    float onUpPosition;
    [SerializeField]
    Collider2D col;
    [SerializeField]
    ChestBehaviour chest;

    Collider2D[] siblings;
    Animator animator;
    Rigidbody2D rigidBody;
    GUIController guiController;
    PlayerController2D playerController;
    LanguageManager m_LanguageManagerInstance;

    bool playerInside;

    void Start ()
    {
        sequencer = GameObject.FindWithTag("Sequencer").GetComponent<Sequencer>();
        GameObject player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController2D>();    
        animator = player.GetComponent<Animator>();     
        joint = GetComponentInParent<FixedJoint2D>();
        rigidBody = GetComponentInParent<Rigidbody2D>();
        guiController = GameObject.FindWithTag("GUIController").GetComponent<GUIController>();
        siblings = transform.parent.GetComponentsInChildren<Collider2D>();

        defaultYPosition = col.offset.y;
        playerInside = false;
        //seleccionar imagen en funcion de teclado o pad
        CheckJoystickConnected();

        m_LanguageManagerInstance = LanguageManager.Instance;
    }

    private void CheckJoystickConnected()
    {
       // Debug.Log(Input.GetJoystickNames()[0]);
        if (Input.GetJoystickNames().Length > 0 && !Input.GetJoystickNames()[0].Equals(""))
        {
            submitButtonImage.GetComponent<Image>().sprite = submitButtonImages[0];
            cancelButtonImage.GetComponent<Image>().sprite = cancelButtonImages[0];
        }
        else
        {
            submitButtonImage.GetComponent<Image>().sprite = submitButtonImages[1];
            cancelButtonImage.GetComponent<Image>().sprite = cancelButtonImages[1];
        }
    }

    void UpdateButtons ()
	{
        if (!chest.buttonsDisabled)
        {
            if (!playerController.isWalking && !chest.isAtBottom)
            {//Saltar
                if (!submitButtonImage.activeSelf)
                    submitButtonImage.SetActive(true);
                submitButtonText.text = m_LanguageManagerInstance.GetTextValue(m_JumpText);
            }
            else if (playerController.isWalking)
            {//Empujar
                if (!submitButtonImage.activeSelf)
                    submitButtonImage.SetActive(true);
                submitButtonText.text = m_LanguageManagerInstance.GetTextValue(m_PushText); ;
            }
            else
            {
                DisableButtons();
            }
          
        }
        else
        {
            DisableButtons();
        }
    }

    private void DisableButtons()
    {
        if (submitButtonImage.activeSelf || cancelButtonImage.activeSelf)
        {
            submitButtonImage.SetActive(false);
            cancelButtonImage.SetActive(false);
        }
    }

    void CancelPush ()
	{
		rabbitCollider.enabled = false;
		joint.enabled = false;
		rigidBody.velocity = Vector2.zero;
		rigidBody.bodyType = RigidbodyType2D.Kinematic;
		submitButtonImage.SetActive (false);
		cancelButtonImage.SetActive(false);
		playerController.isPushing = false;
        playerController.canJump = true;
        animator.SetBool ("isPushing", false);
	}

	void CheckToPush ()
	{
        //Para empujar el cofre debe de estar vacío y el player debe de estar andando hacia el cofre
		if (playerController.isWalking) {
			if (!Game.chestEmpty) {
				//Hay que vaciar el cofre primero
				playerController.StopPlayerMove ();
                chest.buttonsDisabled = true;
				sequencer.StartCoroutine ("StartSequence", sequenceName);
			}
			else if(!playerController.isJumping && !playerController.isOnChest) {
				rabbitCollider.enabled = true;
				joint.enabled = true;
				rigidBody.bodyType = RigidbodyType2D.Dynamic;
				submitButtonImage.SetActive (false);
				cancelButtonImage.SetActive (true);
				playerController.isPushing = true;
				animator.SetBool ("isPushing", true);
			}
		}
		else
			if (!playerController.isOnChest &&!playerController.isPushing && !chest.isAtBottom) {
				playerController.StartCoroutine (playerController.Jump (jumpPosition.position, true));
                 CheckForUpdateOffset(onUpPosition);
        }
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerController.isJumping)
                playerInside = true;
            if (animator.GetFloat(axis) == value && !submitButtonImage.activeSelf && !playerController.isPushing)
            {
                submitButtonImage.SetActive(true);
                submitButtonText.text = m_LanguageManagerInstance.GetTextValue(m_JumpText);
                cancelButtonImage.SetActive(false);
            }
        }
    }

    void Update()
    {

       if(Game.finalBossDefeated && (cancelButtonImage.activeSelf || submitButtonImage.activeSelf))
        {
            submitButtonImage.SetActive(false);
            cancelButtonImage.SetActive(false);
        }

		if (playerInside) {
          
            if (!playerController.isOnChest)
            {
                if (Input.GetButtonDown(OSInputManager.GetPadMapping("Cancel")) && playerController.isPushing && !guiController.IsMenuActive)
                {
                    CancelPush();
                }

                if (animator.GetFloat(axis) == value)
                {
                    if (!playerController.isPushing)
                        UpdateButtons();

                    if (Input.GetButtonDown(OSInputManager.GetPadMapping("Submit")) && !playerController.isPushing && !sequencer.isOnSequence && !chest.isDialogEnabled && !guiController.IsMenuActive)
                    {
                        CheckToPush();
                    }

                }
                if(animator.GetFloat(axis) != value || guiController.IsMenuActive || sequencer.isOnSequence)
                {
                    submitButtonImage.SetActive(false);
                    cancelButtonImage.SetActive(false);
                }
            }
            else if (playerController.isOnChest)
            {
                if (disableWhenOnChest && gameObject.activeSelf &&!playerController.isJumping)
                    gameObject.SetActive(false);

                if (animator.GetFloat(axis) == valueOnChest)
                    ManageOnChest();
                else
                {
                    submitButtonImage.SetActive(false);
                    cancelButtonImage.SetActive(false);
                }
            }

        }
    }

	void ManageOnChest ()
	{
		if (!submitButtonImage.activeSelf)
			submitButtonImage.SetActive (true);
		submitButtonText.text = m_LanguageManagerInstance.GetTextValue(m_JumpText);
		if (Input.GetButtonDown (OSInputManager.GetPadMapping ("Submit")) && !playerController.isPushing && !sequencer.isOnSequence && !questionPanel.activeSelf) {
			playerController.StartCoroutine (playerController.Jump (returnToFloorTransform.position, false));
            EnableSiblings();
           
            CheckForUpdateOffset(defaultYPosition);
        }
  
    }

    void CheckForUpdateOffset(float position)
    {
        if (col.offset.y != position)
        {
            Vector2 offset = new Vector2(col.offset.x, position);
            col.offset = offset;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            submitButtonImage.SetActive(false);
            cancelButtonImage.SetActive(false);
            playerInside = false;
           
        }
    }

  void EnableSiblings()
    {
        for(int i = 0; i< siblings.Length; ++i)
        {
            if (!siblings[i].gameObject.activeSelf)
                siblings[i].gameObject.SetActive(true);
        }
    }

    void OnDisable()
    {
        if (playerInside)
        {
            playerInside = false;
            if(submitButtonImage != null)
                submitButtonImage.SetActive(false);
            if(cancelButtonImage != null)
                cancelButtonImage.SetActive(false);
        }
     }

}
