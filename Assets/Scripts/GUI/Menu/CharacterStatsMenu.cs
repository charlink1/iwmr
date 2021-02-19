using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class StatsFields
{
    public Image avatarImage;
    public Text NameText;
    public Text hpText;
    public Text mpText;
	public Text lvlText;
	public Text expText;

}

public class CharacterStatsMenu : MonoBehaviour {

    public List<StatsFields> charArray;
    public GameObject[] charsList;

    void OnEnable()
    {
        LoadChars();
    }
	
    public void LoadChars()
    {
        if (CharacterParty.charactersParty.Count > 0)
        {
            for (int i = 0; i < CharacterParty.charactersParty.Count; ++i)
            {
                if (!charsList[i].activeSelf)
                    charsList[i].SetActive(true);

                LoadCharStats(i);
                if (charArray[i].lvlText != null && charArray[i].expText != null)
                    LoadCharLevel(i);
            }
        }
    }

    private void LoadCharStats(int index)
    {
        CharacterStats tempCharacterStats = CharacterParty.charactersParty[index].charStats;
        StatsFields currentCharStats = charArray[index];
        currentCharStats.avatarImage.sprite = tempCharacterStats.avatar;
        currentCharStats.NameText.text = tempCharacterStats.name;
        currentCharStats.hpText.text = GetStatsString(tempCharacterStats.currentHealthPoints, tempCharacterStats.totalHealthPoints);
        currentCharStats.mpText.text = GetStatsString(tempCharacterStats.currentMagicPoints, tempCharacterStats.totalMagicPoints);
    }

	public void LoadCharLevel(int index){
		CharacterStats tempCharacterStats = CharacterParty.charactersParty[index].charStats;
		StatsFields currentCharStats = charArray[index];
		currentCharStats.lvlText.text = tempCharacterStats.level.ToString();
		currentCharStats.expText.text = GetStatsString (tempCharacterStats.currentExp, tempCharacterStats.totalExp);
	}

    string GetStatsString(int currentNumber, int totalNumber)
    {
        return currentNumber + " / " + totalNumber;
    }
	
}
