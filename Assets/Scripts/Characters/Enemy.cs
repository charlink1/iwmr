using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartLocalization;

public class Enemy : Character {

    public enum Rarity
    {
        COMMON,
        RARE, 
        UNIQUE

    }

    public enum EnemyType
    {
        FIRE,
        AIR,
        PHISIC

    }

    // public Rarity rarity;
    // public EnemyType enemyType;
    [SerializeField]
    private int m_Experience;

    public List<Item> lootList;
    public int lootProbability = 10;
    
    public int Experience
    {
        get
        {
            return m_Experience;
        }
    }

    void Start()
    {
        charStats.InitCharacterStats();
        charStats.name = LanguageManager.Instance.GetTextValue(charStats.name);
        if (currentEquipment.currentArmor != null)
        {
            EquipableItem armor = currentEquipment.currentArmor;
            armor.ApplyItemEquiped(this);


        }

        if (currentEquipment.currentWeapon != null)
        {
            EquipableItem currentWeapon = currentEquipment.currentWeapon;
            currentWeapon.ApplyItemEquiped(this);


        }
    }
}
