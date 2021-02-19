using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTrigger : StaticInteractiveElementTrigger {

    void Update()
    {
        if (playerInside && Input.GetButtonDown(OSInputManager.GetPadMapping("Submit")) && animator.GetFloat("x") == -1f && !msgController.IsDialogActive)
        {
            InteractShowingMessage("Book.Order");
        }
    }

    

}
