using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        #if UNITY_EDITOR
            if (other.CompareTag("Player"))
            {
                Debug.Log("Inside Boy Trigger");
            }
        #endif
    }
}
