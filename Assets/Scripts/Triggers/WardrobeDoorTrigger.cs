using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeDoorTrigger : MonoBehaviour {

    public bool IsInside { get; set; }

	// Use this for initialization
	void Start () {
        IsInside = false;
	}
	
	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsInside = false;
        }
    }
}
