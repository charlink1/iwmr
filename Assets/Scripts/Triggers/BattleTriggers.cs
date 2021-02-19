using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggers : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Enemy>() != null)
        {
            if (gameObject.transform.position.y > col.gameObject.transform.position.y)
                spriteRenderer.sortingOrder = col.GetComponent<SpriteRenderer>().sortingOrder - 1;
            else
                spriteRenderer.sortingOrder = col.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }

    //void OnTriggerStay2D(Collider2D col)
    //{
    //    if (col.GetComponent<Enemy>() != null && gameObject.transform.position.y > col.gameObject.transform.position.y && (col.GetComponent<SpriteRenderer>().sortingOrder != spriteRenderer.sortingOrder + 1) )
    //        col.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
    //}

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<Enemy>() != null /* && gameObject.transform.position.y < col.gameObject.transform.position.y*/)
            spriteRenderer.sortingOrder = col.GetComponent<SpriteRenderer>().sortingOrder;
    }
}
