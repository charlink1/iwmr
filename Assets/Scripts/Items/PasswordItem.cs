using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordItem : QuestItem {


    [SerializeField]
    Sprite[] imageNumbers;

    [SerializeField]
    int code;

    public override void AddItem(int amount = 1, bool showDialog = true)
    {
        base.AddItem(amount, showDialog);     
        itemImage = imageNumbers[Game.code[code]];
    }

    public override void LoadMenuItem(int amount)
    {
        base.LoadMenuItem(amount);
        itemImage = imageNumbers[Game.code[code]];
    }
}
