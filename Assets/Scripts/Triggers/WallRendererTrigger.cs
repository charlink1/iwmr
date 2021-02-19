using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRendererTrigger : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    SpriteRenderer playerSpriteRenderer;
    int defaultOrder;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultOrder = spriteRenderer.sortingOrder;
        playerSpriteRenderer = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
    }
	
	void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder + 1;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sortingOrder = defaultOrder;
        }
    }
}
