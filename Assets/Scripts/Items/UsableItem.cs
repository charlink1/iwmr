using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : Item {

    public StatName characterStat;
    public int value;


	public override void AddItem (int amount = 1, bool showDialog = true)
	{
		base.AddItem (amount, showDialog);
	}

	public void UseItem(Character character){

		CharacterStats charStats = character.charStats;
		if (characterStat.Equals (StatName.CurrentHealthPoints)) {
			if (charStats.currentHealthPoints < charStats.totalHealthPoints) {
				int tempValue = charStats.currentHealthPoints + value;
				charStats.currentHealthPoints = Mathf.Clamp (tempValue, 0, charStats.totalHealthPoints);
				quantity--;
                if(guiController != null)
                    guiController.PlayItemSound(SoundType.ITEM_USED);
            }
            else
            {
                if(guiController != null)
                    guiController.PlayItemSound(SoundType.ITEM_WRONG);
            }
			
		} else if (characterStat.Equals (StatName.CurrentMagicPoints)) {
			if (charStats.currentMagicPoints < charStats.totalMagicPoints) {
				int tempValue = charStats.currentMagicPoints + value;
				charStats.currentMagicPoints = Mathf.Clamp (tempValue, 0, charStats.totalMagicPoints);
				quantity--;
                if (guiController != null)
                    guiController.PlayItemSound(SoundType.ITEM_USED);
            }
            else
            {
                guiController.PlayItemSound(SoundType.ITEM_WRONG);
            }
           

        }
		if (quantity <= 0)
			ItemsList.items.Remove (this);
	}

	public override void LoadMenuItem (int amount){
		base.LoadMenuItem (amount);
		itemType = ItemType.Usable;
	}




}
