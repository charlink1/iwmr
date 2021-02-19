using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : Item {

	public Sprite itemImage;

	public override void AddItem (int amount = 1, bool showDialog = true)
	{
		base.AddItem (amount, showDialog);
		
	}

	public override void LoadMenuItem (int amount){
		base.LoadMenuItem (amount);
		itemType = ItemType.Quest;
	}



}
