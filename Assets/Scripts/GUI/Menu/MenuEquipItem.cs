using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEquipItem : GameMenuItem {

    MenuEquipController menuController;
    GUIController guiController;
    EquipableItem item;
  
    void Awake()
    {
        guiController = GameObject.FindWithTag("GUIController").GetComponent<GUIController>();
    }

    void Start()
    {
        menuController = guiController.equipMenuController;
        item = (EquipableItem)ItemsList.items.Find(p => p.itemName.Equals(nameText.text));
        GetDescription();
    }

    public void ShowUpdatedStats()
    {
        if(item == null)
            item = (EquipableItem)ItemsList.items.Find(p => p.itemName.Equals(nameText.text));
        menuController.ShowUpdatedStats(item);
    }

    public void ShowDefaultStats()
    {
        menuController.UpdateStats();
    }
    new  public void ManageClick()
    {
        menuController.ManageItem(nameText);
    }

}
