using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomChestTrigger : MonoBehaviour {

    [SerializeField]
    ChestBehaviour chest;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ChestBottom"))
            chest.isAtBottom = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ChestBottom"))
            chest.isAtBottom = false;
    }
}
