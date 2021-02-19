using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float gravity = 20f;
    public int moveSpeed;
    CharacterController charController;
    public bool isGrounded;
  
	Vector3 currPosition, lastPosition;
    [SerializeField]
	GUIController guiController;


    bool flipped = false;


    void Awake () {
        charController = GetComponent<CharacterController>();
	  
    }

    void Update () {
		if (!guiController.IsMenuActive) {
			
			MoveCharacter ();
			currPosition = transform.position;
			isGrounded = charController.isGrounded;

			if (currPosition != lastPosition) {
				GameManager.instance.isWalking = true;
			} else {
				GameManager.instance.isWalking = false;
			}

			lastPosition = currPosition;

			if (GameManager.instance.canGetEncounter && GameManager.instance.isWalking) {

			}
		}

    }

    public void MoveCharacter(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Flip(horizontal);

        charController.SimpleMove(new Vector3(horizontal, -gravity, vertical) * moveSpeed);
       // rb.velocity = new Vector3(horizontal,0, vertical);
    }

    void Flip(float move)
    {
        if (move < 0f && !flipped) {
           // spriteRenderer.flipX = true;
            flipped = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (move > 0 && flipped){
         //   spriteRenderer.flipX = false;
            transform.localScale = new Vector3(1, 1, 1);
            flipped = false;
        }
    }

	void StopPlayerMove(){
		Debug.Log ("Test Event");
	}
}
