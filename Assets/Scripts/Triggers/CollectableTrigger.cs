using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableTrigger : MonoBehaviour {

    [SerializeField]
    protected string axis;
    [SerializeField]
    protected int value;
    [SerializeField]
    protected Collectible collectible;

    protected Animator playerAnimator;

    protected virtual void Awake()
    {
        //Alguno se crea durante el gameplay
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
     
        if(other.CompareTag("Player") && Input.GetButton(OSInputManager.GetPadMapping("Submit")) && playerAnimator.GetFloat(axis) == value && !collectible.obtained)
            collectible.ObtainItem();
  
    }

}
