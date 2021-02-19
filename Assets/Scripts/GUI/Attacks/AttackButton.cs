using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour {

    public BaseAttack magicAttackToPerform;

    Button button;
    Text buttonText;
    Color defaultTextColor;
    Color inactiveTextColor;

    bool deactivated = false;

    void Start()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<Text>();
        inactiveTextColor = Color.gray;
        defaultTextColor = buttonText.color;
    }

   public bool IsDeactivated()
    {
        return deactivated;
    }

    public void CastMagicAttack()
    {
        GameObject.FindWithTag("BattleManager").GetComponent < BattleController >().MagicInput(magicAttackToPerform);
    }

    public void DeactivateButton()
    {
        if(button != null)
        {
            button.interactable = false;
            buttonText.color = inactiveTextColor;
            deactivated = true;
        }
       
    }

    public void ActivateButton()
    {
        button.interactable = true;
        buttonText.color = defaultTextColor;
        deactivated = false;
    }
}
