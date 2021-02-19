using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobbieTrigger : StaticInteractiveElementTrigger {

    void Update()
    {

        if (playerInside && Input.GetButtonDown(OSInputManager.GetPadMapping("Submit")) && !msgController.IsDialogActive &&
            ((player.transform.position.x < transform.position.x && animator.GetFloat("x") == 1)||
            (player.transform.position.x > transform.position.x && animator.GetFloat("x") == -1)))
        {
            InteractShowingMessage("Curiosities.Robbie");
        }

    }
}
