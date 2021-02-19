using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipType {Weapon, Armor};

public class EquipableItem : Item {
	
	public GameObject menuEquipPrefab;
   
	public StatName characterStat;
	public int value;

	public EquipType equipType;
	public bool equiped = false;

    public MenuEquipController menuEquipController { get; set; }
    public Transform equipmentMenuList { get; set; }

    void Awake()
    {
        if (equipmentMenuList == null)
            equipmentMenuList = GameObject.FindWithTag("Canvas").transform.Find(guiController.equipmentListPath).transform;
    }

	public override void AddItem (int amount = 1, bool showDialog = true)
	{
		
		base.AddItem (amount, showDialog);

		ItemsList.equipableItems.Add (this);
		if(equipmentMenuList == null)
			equipmentMenuList = GameObject.FindWithTag ("Canvas").transform.Find (guiController.equipmentListPath).transform;
		
		if(menuEquipPrefab != null)
			Instantiate (menuEquipPrefab, equipmentMenuList, false);
	}

	
	public override void LoadMenuItem (int amount){
		base.LoadMenuItem (amount);
		itemType = ItemType.Equipment;

        MenuEquipItem menuEquipItem = menuEquipPrefab.GetComponentInChildren<MenuEquipItem>();
        
        menuEquipItem.itemDescription = description;
        menuEquipItem.nameText.text = itemName;

    }

	public void EquipItem(PlayableCharacter character, EquipType type){
		if (type.Equals (equipType)) {
			ApplyItemEquiped(character);
            if(guiController == null)
                guiController = GameObject.FindWithTag("GUIController").GetComponent<GUIController>();

            guiController.PlayItemSound(SoundType.ITEM_USED);

			if (equipType.Equals (EquipType.Armor))
				character.currentEquipment.currentArmor = this;
			else
				character.currentEquipment.currentWeapon = this;

			Destroy(equipmentMenuList.Find (menuEquipPrefab.name+"(Clone)").gameObject);
			
		}
	}

	public void UnequipItem(PlayableCharacter character){
		CharacterStats charStat = character.charStats;
		ApplyValue (charStat, -value);
        GameObject menuItem = ReloadMenuEquipItem();
        menuItem.transform.SetSiblingIndex(1);

        if (equipType.Equals(EquipType.Armor))
            character.currentEquipment.currentArmor = null;
        else
            character.currentEquipment.currentWeapon = null;
        this.equiped = false;

    }

    public GameObject ReloadMenuEquipItem()
    {
        MenuEquipItem menuItem = menuEquipPrefab.GetComponentInChildren<MenuEquipItem>();
        menuItem.itemDescription = description;
        menuItem.nameText.text = itemName;

        return Instantiate(menuEquipPrefab, equipmentMenuList, false);
    }

	public void ApplyItemEquiped(Character character){
		CharacterStats charStat = character.charStats;
		ApplyValue (charStat, value);
        this.equiped = true;
	}

	void ApplyValue(CharacterStats charStat, int num){
		if (characterStat == StatName.Attack) {
			charStat.totalAttack += num;
		}
		if (characterStat == StatName.Defense) {
			charStat.totalDefense += num;
		}
		if (characterStat == StatName.Magic) {
			charStat.totalMagic += num;
		}
		if (characterStat == StatName.Speed) {
			charStat.totalSpeed += num;
		}
	}
}
