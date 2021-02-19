using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuItem : MonoBehaviour {

	public Text quantityText;
	public Text nameText;

	MenuItemsController menuItemsController;
    Text descriptionText;

    public string itemDescription { get; set; }

    void Start()
    {
        menuItemsController = GameObject.FindWithTag("ItemListGameObject").GetComponent<MenuItemsController>();
        GetDescription();
    }

    protected void GetDescription()
    {
        descriptionText = GameObject.FindWithTag("DescriptionField").GetComponent<Text>();
        Item item = ItemsList.items.Find(p => p.itemName.Equals(nameText.text));
        if(item!=null)
            itemDescription = item.description;
    }


    public void ShowDescription()
    {
        if(itemDescription != null)
        {
            Item item = ItemsList.items.Find(p => p.itemName.Equals(nameText.text));
            itemDescription = item.description;
        }
        descriptionText.text = itemDescription;

    }

	public void ManageClick(){
		menuItemsController.ManageItem (nameText);
	}

	public void CleanDescription(){
		descriptionText.text = "";
	}


	public void HideImage(){
		menuItemsController.HideImage ();
	}
}
