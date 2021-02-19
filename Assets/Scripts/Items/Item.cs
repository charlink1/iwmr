using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SmartLocalization;


public enum StatName { Attack, Defense, Magic, Speed, CurrentHealthPoints, CurrentMagicPoints}
public class Item : MonoBehaviour {

    public enum ItemType { Usable, Equipment, Quest}

    [SerializeField]
    protected string itemNameKey;
    [SerializeField]
    protected string descriptionKey;
    [SerializeField]
    bool isFemale = false;

    public string itemName;
	public string description;

	public GameObject menuItemPrefab;
	public ItemType itemType;

    public Transform itemListGameObject { get; set; }
    public int quantity { get; set; }
    public bool instantiated { get; set; }

    protected GUIController guiController;

    bool nameInitialized = false;

    LanguageManager m_LanguageManager;
 
    public virtual void AddItem(int amount = 1, bool showDialog = true){
        nameInitialized = false;
        Init ();
        if (showDialog)
        {
            guiController = GameObject.FindWithTag("GUIController").GetComponent<GUIController>();
            guiController.StartCoroutine(guiController.ShowItemDialog(itemName, isFemale));
        }
		Item found = ItemsList.items.Find (p => p.itemName.Equals (itemName));
		if (found == null) {
			LoadMenuItem (amount);
            this.quantity = amount;
            ItemsList.items.Add (this);
			Instantiate (menuItemPrefab, itemListGameObject, false);
        } else {
			found.quantity += amount;
			UpdateMenuItem ();
		}
        if (guiController != null)
            guiController.PlayItemSound(SoundType.ITEM_OBTAINED);
        instantiated = true;
    }

    public void ReloadItem(int amount)
    {
         LoadMenuItem(amount);
        if (!instantiated)
        {
            Instantiate(menuItemPrefab, itemListGameObject, false);
            instantiated = true;
        }
    }

    public void AddItemInBattle(int amount = 1)
    {
        //mirar si el item está en la lista estática de items
        //si no está se añade y si está se incrementa la cantidad
        Item found = ItemsList.items.Find(p => p.itemName.Equals(itemName));
        if (found == null)
        {
            if(!nameInitialized)
                InitName();
            this.quantity = amount;
            ItemsList.items.Add(this);   
        }
        else
        {
            found.quantity += amount;
        }
    }

	public void Init ()
    {
        instantiated = false;
        if (!nameInitialized)
            InitName();
        if (itemListGameObject == null)
        {
            guiController = GameObject.FindWithTag("GUIController").GetComponent<GUIController>();
            itemListGameObject = GameObject.FindWithTag("Canvas").transform.Find(guiController.itemsListPath).transform;
        }
    }

    private void InitName()
    {
        if (m_LanguageManager == null)
            m_LanguageManager = LanguageManager.Instance;

        itemName = m_LanguageManager.GetTextValue(itemNameKey);
        description = m_LanguageManager.GetTextValue(descriptionKey);
        nameInitialized = true;
    }

    public virtual void LoadMenuItem (int amount)
	{
		GameMenuItem menuItem = menuItemPrefab.GetComponentInChildren<GameMenuItem> ();
       
        menuItem.itemDescription = description;
        menuItem.nameText.text = itemName;

        quantity = amount;
		menuItem.quantityText.text = quantity.ToString();

	}

	protected void UpdateMenuItem(){
		Init ();
		GameMenuItem menuItem = itemListGameObject.transform.Find (menuItemPrefab.name + "(Clone)").GetComponentInChildren<GameMenuItem>();
		if (menuItem != null) {
			menuItem.quantityText.text = quantity.ToString ();
		}
	}
}
