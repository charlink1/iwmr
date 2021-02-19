using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxiliarCombatPanel : MonoBehaviour {

    [SerializeField]
    GameObject commandsPanel;
    [SerializeField]
    BattleController battlecontroller;

	void Update () {
        if (Input.GetButtonDown(OSInputManager.GetPadMapping("Cancel"))){
            commandsPanel.SetActive(true);
            battlecontroller.ActivateCommandsMenu(battlecontroller.tempAttackButton);
            gameObject.SetActive(false);
        }
	}
}
