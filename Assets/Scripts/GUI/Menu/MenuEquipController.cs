using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEquipController : MenuController {

	[SerializeField]
    Color defaultTextColor = Color.white;
    [SerializeField]
    Image avatarImage;

    [SerializeField]
    Text charWeaponText;
    [SerializeField]
    Text charArmorText;

    [SerializeField]
    Text currentAttackText;
    [SerializeField]
    Text updatedAttackText;
    [SerializeField]
    Text currentDefenseText;
    [SerializeField]
    Text updatedDefenseText;
    [SerializeField]
    Text currentSpeedText;
    [SerializeField]
    Text updatedSpeedText;
    [SerializeField]
    Text currentMagicText;
    [SerializeField]
    Text updatedMagicText;

    [SerializeField]
    Button firstSelectedButton;
    [SerializeField]
    Button equipPanelSelectedButton;
    [SerializeField]
    GameObject equipPanel;
    [SerializeField]
    Text descriptionText;

    EquipableItem selectedItem;

    PlayableCharacter selectedCharacter { get; set; }
    public int characterIndex { get; set; }

    protected void Start () {
		//base.Start ();
        characterIndex = guiController.characterIndex;
        ReloadEquipItemsList();
    }

    void Update()
    {
        if (Input.GetButtonDown(OSInputManager.GetPadMapping("Cancel")))
        {
			if (selectedItem != null) {
				selectedItem = null;
				StartCoroutine ("SelectButton", firstSelectedButton);

			} else {
				selectedCharacter = null;
                guiController.characterIndex = 0;
				ShowManageMenu (equipPanel);
			}
        }

		if(Input.GetButtonDown(OSInputManager.GetPadMapping("LButton"))){
			List<PlayableCharacter> tempList = CharacterParty.charactersParty;
			if (tempList != null && tempList.Count > 0) {
				characterIndex--;
				if (characterIndex < 0)
					characterIndex = tempList.Count - 1;
			
				LoadSelectedCharacterData ();
			}
		}

		if(Input.GetButtonDown(OSInputManager.GetPadMapping("RButton"))) {
            List<PlayableCharacter> tempList = CharacterParty.charactersParty;
            if (tempList != null && tempList.Count > 0)
            {
                characterIndex++;
                if (characterIndex > tempList.Count - 1)
                    characterIndex = 0;

                LoadSelectedCharacterData();
            }
        }
			
    }

    void ReloadEquipItemsList()
    {
        foreach (EquipableItem item in ItemsList.equipableItems)
        {
            if(item.equipmentMenuList == null)
            {
                if (guiController == null)
                    FindGUIController();
                item.equipmentMenuList = GameObject.FindWithTag("Canvas").transform.Find(guiController.equipmentListPath).transform;
                //Instantiate(item.menuEquipPrefab, item.equipmentMenuList, false);
                if(!item.equiped)
                    item.ReloadMenuEquipItem();
            }
        }
    }

	void LoadSelectedCharacterData ()
	{
		selectedCharacter = CharacterParty.charactersParty [characterIndex];
        if (CharacterParty.charactersParty.Count > 1)
            guiController.PlayItemSound(SoundType.ITEM_SELECTED);

		LoadCharacterData (selectedCharacter);
	}

    protected override void OnEnable(){
        base.OnEnable();
        if (itemListObject.childCount == 0)
            StartCoroutine("SelectButton", firstSelectedButton);

		if (CharacterParty.charactersParty != null && CharacterParty.charactersParty.Count > 0) {
			if (selectedCharacter == null) {
                if (guiController == null)
                    guiController = GameObject.FindWithTag("GUIController").GetComponent<GUIController>();
				characterIndex = characterIndex = guiController.characterIndex; 
				selectedCharacter = CharacterParty.charactersParty [characterIndex];
			}
			else {
				characterIndex = CharacterParty.charactersParty.FindIndex (p => p.Equals (selectedCharacter));
			}
		}

		if(selectedCharacter != null)
			LoadCharacterData (selectedCharacter);
	}

	void LoadCharacterData(PlayableCharacter character){
		CharacterStats characterStats = character.charStats;
		//Character base
		avatarImage.sprite = characterStats.avatar;
		EquipableItem weapon = character.currentEquipment.currentWeapon;
		if (weapon != null)
			charWeaponText.text = weapon.itemName;
		else
			charWeaponText.text = "";
		
		EquipableItem armor = character.currentEquipment.currentArmor;
		if (armor != null)
			charArmorText.text = armor.itemName;
		else
			charArmorText.text = "";
		//Character stats
		currentAttackText.text = characterStats.totalAttack.ToString();
		currentDefenseText.text = characterStats.totalDefense.ToString();
		currentSpeedText.text = characterStats.totalSpeed.ToString();
		currentMagicText.text = characterStats.totalMagic.ToString();

        PrintUpdatedStats(characterStats);


    }

    public void Reload()
    {
        LoadCharacterData(selectedCharacter);
    }

    void PrintUpdatedStats(CharacterStats charStats)
    {
        updatedAttackText.text = charStats.totalAttack.ToString();
        updatedDefenseText.text = charStats.totalDefense.ToString();
        updatedSpeedText.text = charStats.totalSpeed.ToString();
        updatedMagicText.text = charStats.totalMagic.ToString();

        //En caso de que sea menor se muestra en rojo, en caso de ser mayor se muestra en amarillo (Esto secundario)

        CharacterStats currentStats = selectedCharacter.charStats;
        CheckStat(charStats.totalAttack, currentStats.totalAttack, updatedAttackText);
        CheckStat(charStats.totalDefense, currentStats.totalDefense, updatedDefenseText);
        CheckStat(charStats.totalSpeed, currentStats.totalSpeed, updatedSpeedText);
        CheckStat(charStats.totalMagic, currentStats.totalMagic, updatedMagicText);


    }

    void CheckStat(int temp, int current, Text field)
    {
         if (temp < current)
            field.color = Color.red;
         else if (temp > current)
            field.color = Color.yellow;
         else
            field.color = defaultTextColor;

    }


    public void UpdateStats()
    {
        CharacterStats statsCopy = null;

        if (selectedCharacter.currentEquipment.currentArmor != null && selectedItem != null && selectedItem.equipType.Equals(EquipType.Armor))
        {
            EquipableItem currentArmor = selectedCharacter.currentEquipment.currentArmor;
            StatName statToDecrease = currentArmor.characterStat;

            statsCopy = ApplyValue(statToDecrease, selectedCharacter.charStats, -currentArmor.value);

        }
       else if (selectedCharacter.currentEquipment.currentWeapon != null && selectedItem != null && selectedItem.equipType.Equals(EquipType.Weapon))
        {
            EquipableItem currentWeapon = selectedCharacter.currentEquipment.currentWeapon;
            StatName statToDecrease = currentWeapon.characterStat;

            statsCopy = ApplyValue(statToDecrease, selectedCharacter.charStats, -currentWeapon.value);
           
        }
        else
        {
            statsCopy = selectedCharacter.charStats;
        }
      
        PrintUpdatedStats(statsCopy);
    }
    public void ShowUpdatedStats(EquipableItem item)
    {
        //Mirar si el pj tiene un objeto equipado en la casilla correspondiente
        //En caso afirmativo se deja como si no tuviera nada (usando variable a parte)
        CharacterStats statsCopy =null;
       
        if (selectedCharacter.currentEquipment.currentArmor != null && item.equipType.Equals(EquipType.Armor))
        {
            EquipableItem currentArmor = selectedCharacter.currentEquipment.currentArmor;
            StatName statToDecrease = currentArmor.characterStat;

            statsCopy = ApplyValue(statToDecrease, selectedCharacter.charStats, -currentArmor.value);

        }
        if (selectedCharacter.currentEquipment.currentWeapon != null && item.equipType.Equals(EquipType.Weapon))
        {
            EquipableItem currentWeapon = selectedCharacter.currentEquipment.currentWeapon;
            StatName statToDecrease = currentWeapon.characterStat;

            statsCopy = ApplyValue(statToDecrease, selectedCharacter.charStats, -currentWeapon.value);
        }

        //Se añade lo que se aplicaría y se muestra en el resultado actualizado
        if(statsCopy == null)
        {
            statsCopy = InitStatsCopy(selectedCharacter.charStats);
        }
        statsCopy = ApplyValue(item.characterStat, statsCopy, item.value);
        PrintUpdatedStats(statsCopy);
    }

    CharacterStats InitStatsCopy(CharacterStats charStat)
    {
        CharacterStats stats = new CharacterStats();

        stats.totalAttack = charStat.totalAttack;
        stats.totalDefense = charStat.totalDefense;
        stats.totalMagic = charStat.totalMagic;
        stats.totalSpeed = charStat.totalSpeed;
        return stats;
    }
    CharacterStats ApplyValue(StatName statName, CharacterStats charStat, int num)
    {
        CharacterStats stats = InitStatsCopy(charStat);

        if (statName == StatName.Attack) {
            stats.totalAttack += num;
        }
      
		if (statName == StatName.Defense) {
			stats.totalDefense += num;
		}
		if (statName == StatName.Magic) {
			stats.totalMagic += num;
		}
		if (statName == StatName.Speed) {
			stats.totalSpeed += num;
		}

        return stats;
    }

    public void ManageItem(Text itemTextName)
    {
        EquipableItem item = (EquipableItem) ItemsList.items.Find(p => p.itemName.Equals(itemTextName.text));
        IsLoading = true;
        switch (item.equipType)
        {
            case EquipType.Weapon:
                UnequipItem(selectedCharacter.currentEquipment.currentWeapon);
                break;
            case EquipType.Armor:
                UnequipItem(selectedCharacter.currentEquipment.currentArmor); ;
                break;
           
        }

        item.EquipItem(selectedCharacter, item.equipType);
        LoadCharacterData(selectedCharacter);

        StartCoroutine("SetSelectedButton");
    }

    private void UnequipItem(EquipableItem equipableItem)
    {
        if (equipableItem != null)
            equipableItem.UnequipItem(selectedCharacter);
    }

    IEnumerator SetSelectedButton()
    {
        yield return null;
        IsLoading = true;
        if (itemListObject.childCount == 0)
            StartCoroutine("SelectButton", firstSelectedButton);
        else
            StartCoroutine("SelectButton", itemListObject.GetChild(0).GetComponentInChildren<Button>());
    }

    public void UnequipItem()
    {
        if (!IsLoading)
            IsLoading = true;

        if (selectedItem != null)
        {
            selectedItem.UnequipItem(selectedCharacter);
            LoadCharacterData(selectedCharacter);
            StartCoroutine("SelectButton", firstSelectedButton);
            guiController.PlayItemSound(SoundType.MENU_BACK);
			selectedItem = null;
        }
    }

    public void SetSelectedWeapon()
    {
		if (charWeaponText.text != null  && !charWeaponText.text.Equals("")) {
			selectedItem = selectedCharacter.currentEquipment.currentWeapon;
			StartCoroutine ("SelectButton", equipPanelSelectedButton);
		}
    }

    public void SetSelectedArmor()
    {
		if (charArmorText.text != null && !charArmorText.text.Equals("")) {
			selectedItem = selectedCharacter.currentEquipment.currentArmor;
			StartCoroutine ("SelectButton", equipPanelSelectedButton);
		}
    }


	public void ShowCurrentArmorDescription(){
		EquipableItem item = selectedCharacter.currentEquipment.currentArmor;
		if( item != null)
			ShowItemDescription(item.description);
	}

	public void ShowCurrentWeaponDescription(){
		EquipableItem item = selectedCharacter.currentEquipment.currentWeapon;
		if(item != null)
			ShowItemDescription(item.description);
	}


	public void ShowItemDescription(string description)
	{
		descriptionText.text = description;
	}
}
