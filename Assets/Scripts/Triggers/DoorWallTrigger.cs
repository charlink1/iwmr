using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWallTrigger : MonoBehaviour {

    [SerializeField]
    Collider2D wallCollider;

    [SerializeField]
    Sequencer sequencer;
    [SerializeField]
    Animator doorAnimator;
    [SerializeField]
    SpriteRenderer doorSprite;

    void Start()
    {
        if (Game.alreadyEntered)
        {
            gameObject.SetActive(false);
            wallCollider.enabled = true;
        }
    }
	
	void OnTriggerEnter2D(Collider2D other)
    {
        CloseDoor(other);
    }

    private void CloseDoor(Collider2D other)
    {
        if (other.CompareTag("Player") && !Game.alreadyEntered)
        {
            if (!wallCollider.enabled)
                wallCollider.enabled = true;

            //if (sequencer.isOnSequence)
            //{
                doorAnimator.SetBool("doorOpened", false);
                doorSprite.sortingOrder = 2;
           // }
            if (other.GetComponent<SpriteRenderer>().sortingOrder < 3)
                other.GetComponent<SpriteRenderer>().sortingOrder = 3;

            Game.alreadyEntered = true;
            gameObject.SetActive(false);
        }
    }

    //En caso de que no lo pille bien al entrar
    void OnTrigerExit2D(Collider2D other)
    {
        CloseDoor(other);
    }
}
