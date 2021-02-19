using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour {

	
	void Start () {

        //Hide cursor
#if !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
#endif

        //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
	}
	
	
	
}
