using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Equipment
{
	public EquipableItem currentWeapon;
	public EquipableItem currentArmor;
}

[Serializable]
public class CharacterStats {

    public string name;
    public Sprite avatar; 

    //Combat skills
    public int attack;
    public int magic;
    public int combatSpeed;
    public int defense;

	public int totalAttack { get; set; }
	public int totalDefense { get; set; }
	public int totalMagic { get; set; }
	public int totalSpeed { get; set; }

    //Global skills
    public int currentHealthPoints;
    public int currentMagicPoints;

    public int totalHealthPoints;
    public int totalMagicPoints;

    public int totalExp = 100;
    public int currentExp;

    public int level;
	int maxLevel = 100;
    public bool levelUp { get; set; }


	int indexAttack =2;
	int indexMagic = 1;
	int indexDefense = 1;
	int indexCombatSpeed = 1;
	int indexTotalHealthPoints = 50;
	int indexTotalMagicPoints = 10;

    public void AddExperience(int exp)
    {
		if (level < maxLevel) {
			currentExp += exp;
			if (currentExp >= totalExp) {
				LevelUp ();
			}
		}
    }


    public void LevelUp()
    {
        currentExp = Mathf.Abs(totalExp - currentExp);
        totalExp *= 2;

		//increase total stats
		totalAttack +=  indexAttack;
		totalDefense += indexDefense;
		totalMagic += indexMagic;
		totalSpeed += indexCombatSpeed;
					//Increase stats
		attack +=indexAttack;
		magic += indexMagic;
		combatSpeed += indexCombatSpeed;
		defense += indexDefense;
		totalHealthPoints += indexTotalHealthPoints;
		totalMagicPoints += indexTotalMagicPoints;
		currentHealthPoints = totalHealthPoints;
		currentMagicPoints = totalMagicPoints;
		//
        level ++;

		if (currentExp >= totalExp && level < maxLevel) {
			LevelUp ();
		}
        levelUp = true;
    }

	public void InitCharacterStats()
	{
		totalAttack = attack;
		totalDefense = defense;
		totalMagic = magic;
		totalSpeed = combatSpeed;
	}
}


