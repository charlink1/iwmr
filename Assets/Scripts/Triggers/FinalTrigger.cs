using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTrigger : MonoBehaviour {
  
	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            other.GetComponent<SpriteRenderer>().enabled = false;
            Collider2D[] colliders = other.gameObject.GetComponents<Collider2D>();
            for(int i = 0; i< colliders.Length; ++i)
            {
                colliders[i].enabled = false;
            }
         
        }
    }
}
