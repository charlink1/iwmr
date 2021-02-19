using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockoBehaviour : MonoBehaviour {

	void OnDestroy()
    {
        Game.sockoDefeated = true;
    }
}
