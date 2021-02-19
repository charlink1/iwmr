using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayableCharacter : Character {

    public static PlayableCharacter instance;

    public bool battlePrefab;

    public GameObject CharGameObject { get; set; }

    public List<BaseAttack> magicAttacks = new List<BaseAttack>();
    SpriteRenderer spriteRenderer;
    public int orderInLayer { get; set; }
    

    void Start () {
        instance =  GetInstance();
       
        PlayableCharacter mainCharacter = gameObject.GetComponent<PlayableCharacter>();
        CharacterParty.JoinParty(mainCharacter);
       
        spriteRenderer = GetComponent<SpriteRenderer>();
        charStats.InitCharacterStats ();
       
        if (currentEquipment.currentArmor != null)
        {
            EquipableItem armor = currentEquipment.currentArmor;
            armor.ApplyItemEquiped(this);
           
            if (!Game.returningFromBattle && !battlePrefab)
            {
                armor.AddItem(1, false);
                Destroy(armor.equipmentMenuList.Find(armor.menuEquipPrefab.name + "(Clone)").gameObject);
            }
        }

        if (currentEquipment.currentWeapon != null)
        {
            EquipableItem currentWeapon = currentEquipment.currentWeapon;
            currentWeapon.ApplyItemEquiped(this);
            if (!Game.returningFromBattle  && !battlePrefab)
            {
                currentWeapon.AddItem(1, false);
                Destroy(currentWeapon.equipmentMenuList.Find(currentWeapon.menuEquipPrefab.name + "(Clone)").gameObject);
            }
          
        }
        orderInLayer = spriteRenderer.sortingOrder;
        if (Game.initialized)
            LoadCharacter();

        CharGameObject = gameObject;
       
		//Debug.Log ("total attack: " + charStats.totalAttack);

    }


    public PlayableCharacter GetInstance()
    {
        if (instance == null)
            return this;
        else
            return instance;
    }

    public void LoadCharacter()
    {
        PlayableCharacter player = CharacterParty.GetCharacterData(charStats.name);
        charStats = player.charStats;
        spriteRenderer.sortingOrder = player.orderInLayer;

        if (!battlePrefab)
        {
            CharacterParty.UpdateCharacterGameObject(charStats.name, gameObject);
            GameObjectData trans = Game.LoadSceneObjectTransformByName(name);
            currentEquipment = player.currentEquipment;

            if (trans != null)
            {
                transform.position = trans.position;
                transform.rotation = trans.rotation;
                transform.localScale = trans.localScale;
            }
         
           CharacterParty.UpdateCharacter(this);
        }

    }

    
}
