using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableTriggerDown : CollectableTrigger {

    [SerializeField]
    GameObject playerObject;
    [SerializeField]
    SpriteRenderer renderer;

    int playerSortingOrder;
    int defaultSortingOrder;

   
    SpriteRenderer playerRenderer;

    protected override void Awake()
    {
        base.Awake();
        if (renderer != null)
            

        playerRenderer = playerObject.GetComponent<SpriteRenderer>();
        if (playerRenderer != null)
        {
            defaultSortingOrder = renderer.sortingOrder;
            playerSortingOrder = playerRenderer.sortingOrder;
        }
            
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerSortingOrder < defaultSortingOrder)
            {
                playerSortingOrder = playerRenderer.sortingOrder;
                playerRenderer.sortingOrder = defaultSortingOrder + 1;
            }
                
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRenderer.sortingOrder > defaultSortingOrder -1 )
                playerRenderer.sortingOrder = playerSortingOrder;
        }
    }


}
