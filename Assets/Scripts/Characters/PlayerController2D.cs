using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2D : AutoController {

    [SerializeField]
    MessageDialogController msgController;
    [SerializeField]
    GUIController guiController;
    [SerializeField]
    float jumpSpeed = 75f;

    public GameObject targetToLook;
    public GameObject introTargetLook;

    Vector3 currPosition, lastPosition;
    Collider2D col;

    bool canBeAttacked;
    bool isGrounded = true;

    public bool isPushing { get; set; }
 	public bool isOnChest { get; set; }
	public bool isJumping { get; set; }
	public bool canJump { get; set; }
   
    protected override void Start () {
        base.Start();
   
		col = GetComponent<Collider2D> ();
       
        canJump = true;
        animator.SetFloat("y", -1);
		if (Game.firstSequenceDone)
			spriteRenderer.sortingOrder = 3;
		
		canMove = false;

        canBeAttacked = false;
        StartCoroutine("WaitBeforeGettingAttacked");
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //    StartCoroutine("LookTarget", testTarget);

        if (!isOnChest && canBeAttacked && !sequencer.isOnSequence && canMove && !msgController.IsDialogActive && !guiController.IsMenuActive /*&& isGrounded */&& !GameManager.instance.attacked)
        {
            currPosition = transform.position;
            if (currPosition != lastPosition)
            {
                GameManager.instance.isWalking = true;
            }
            else
            {
                GameManager.instance.isWalking = false;
            }

            lastPosition = currPosition;
        }

    }

 
    IEnumerator WaitBeforeGettingAttacked()
    {
#if UNITY_EDITOR_OSX || UNITY_STANDLAONE_OSX
        yield return new WaitForSeconds(7f);
        canBeAttacked = true;
#else 
        yield return new WaitForSeconds(5f);
        canBeAttacked = true;
#endif
    }

	void FixedUpdate ()
    {
        if (!sequencer.isOnSequence && canMove && !msgController.IsDialogActive && !guiController.IsMenuActive && isGrounded && !GameManager.instance.attacked)
        {
            MovePlayer();          		
        }
       
        MoveByAnimation();
    }

	public IEnumerator SetPlayerToMove(){
		LookTarget (introTargetLook);
		yield return null;
		canMove = true;
		if(sequencer.isOnSequence)
			sequencer.PerformNextSequenceStep ();
	}

    private void MovePlayer()
    {
        if (animator.isInitialized) { 
            float input_x = Input.GetAxisRaw("Horizontal");
            float input_y = Input.GetAxisRaw("Vertical");

            if (input_x == 0 && input_y == 0)
            {
                input_x = Input.GetAxis(OSInputManager.GetPadMapping("DPadHorizontal"));
                input_y = Input.GetAxis(OSInputManager.GetPadMapping("DPadVertical"));
            }

            if (input_x > 0)
                input_x = 1;
            else if (input_x < 0)
                input_x = -1;

            if (input_y > 0)
                input_y = 1;
            else if (input_y < 0)
                input_y = -1;

            ChechIfPlayerIsWalking (input_x, input_y);
			if (!isOnChest) 
				rb.velocity = new Vector2 (input_x, input_y).normalized * speed * Time.fixedDeltaTime;			
         }
    }

    protected override void ChechIfPlayerIsWalking(float input_x, float input_y)
    {
		bool hasInput = (Mathf.Abs (input_x) + Mathf.Abs (input_y)) > 0;
		if (!isOnChest)
			isWalking = hasInput;
		else
			isWalking = false;

        animator.SetBool("isWalking", isWalking);
		if ((isWalking ||(isOnChest && hasInput)) && !isPushing)
        {
            animator.SetFloat("x", input_x);
            animator.SetFloat("y", input_y);
        }

    }
		

    public void StopPlayerMove()
    {
        // Debug.Log("Test Event");
        ChechIfPlayerIsWalking(0, 0);
        rb.velocity = new Vector2(0, 0);
    }

	public IEnumerator Jump(Vector3 destination, bool up)
    {
		if (canJump) {
			canMove = false;
			float jumpForce = 0.3f;
			if (animator.GetFloat ("y") == 1)
				jumpForce = 0.4f;
			else if (animator.GetFloat ("y") == -1)
				jumpForce = 0.2f;
       
			StopPlayerMove ();
			col.isTrigger = true;
			Vector2 highPoint = new Vector2 ((destination.x - transform.position.x) / 2, transform.position.y + jumpForce);
			Vector2 direction = new Vector2 (highPoint.x, highPoint.y - transform.position.y).normalized;
            isJumping = true;
           
            animator.SetBool("isJumping", true);
            while (transform.position.y < highPoint.y) {
				rb.velocity = direction * Time.fixedDeltaTime * jumpSpeed;
				yield return null;
			}
            spriteRenderer.sortingOrder = 4;
            direction = new Vector2 (destination.x - transform.position.x, destination.y - transform.position.y);
			while (transform.position.y > destination.y + 0.2f) {
				rb.velocity = direction * Time.fixedDeltaTime * jumpSpeed;
				yield return null;
			}

			rb.velocity = Vector2.zero;
        
            isOnChest = up;
			canMove = true;
			col.isTrigger = false;
			isJumping = false;
            animator.SetBool("isJumping", false);
        }
        yield return null;
    }

    public IEnumerator WaitPlayerJump()
    {
        yield return null;
        canJump = true;
        yield return null;
        sequencer.PerformNextSequenceStep();
    }

    public IEnumerator StopPlayerCoroutine()
    {
        StopPlayerMove();
        yield return null;
        sequencer.PerformNextSequenceStep();
    }

	public int GetAnimatorAxisValue(string axis){
		return (int)animator.GetFloat (axis);
	}
   
}
