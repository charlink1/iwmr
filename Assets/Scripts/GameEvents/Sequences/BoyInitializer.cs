using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyInitializer : MonoBehaviour {


    public GameObject boyGameObject;

	
	void Awake () {
        if (Game.boyAppeared)
            boyGameObject.SetActive(true);
	}
	
	public void InitBoy()
    {
        boyGameObject.SetActive(true);
        Animator animator = boyGameObject.GetComponent<Animator>();
        animator.SetTrigger("underBed");
        #if UNITY_EDITOR
            Debug.Log("Boy Active: " + Game.boyAppeared);
        #endif
        Game.boyAppeared = true;
        
       
    }

    
}
