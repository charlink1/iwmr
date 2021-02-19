using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderBehaviour : MonoBehaviour {

    [SerializeField]
    int orderInLayer = 2;

    SpriteRenderer spriteRender;
	SpriteRenderer playerRenderer;
	PlayerController2D playerController;

	void Awake(){
		spriteRender = GetComponentInParent<SpriteRenderer> ();
		GameObject player = GameObject.FindWithTag ("Player");
		playerRenderer = player.GetComponent<SpriteRenderer> ();
		playerController = player.GetComponent<PlayerController2D> ();
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			if ((!playerController.isOnChest /*|| !playerController.isJumping*/) && spriteRender.sortingOrder <= playerRenderer.sortingOrder && !playerController.isJumping) {
				spriteRender.sortingOrder = playerRenderer.sortingOrder + 1;
                #if UNITY_EDITOR
                    Debug.Log("Sprite order: " + spriteRender.sortingOrder);
                #endif
            }

			if ((playerController.isOnChest /*|| playerController.isJumping*/) && spriteRender.sortingOrder > playerRenderer.sortingOrder) {
				spriteRender.sortingOrder = playerRenderer.sortingOrder = orderInLayer;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Player")) 
			spriteRender.sortingOrder = playerRenderer.sortingOrder = orderInLayer;

        if (CompareTag("Chest") && transform.position.y < other.gameObject.transform.position.y)
            spriteRender.sortingOrder = playerRenderer.sortingOrder - 1;
    }

    void OnDisable()
    {
        if ((playerController.isOnChest /*|| playerController.isJumping*/) && spriteRender != null && spriteRender.sortingOrder > playerRenderer.sortingOrder)
        {
            spriteRender.sortingOrder = playerRenderer.sortingOrder = orderInLayer;
        }
    }
}
