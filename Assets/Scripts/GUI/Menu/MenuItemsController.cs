using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemsController : MenuController {

    [SerializeField]
	GameObject charactersListPanel;

    [SerializeField]
    GameObject itemViewerPanel;

    [SerializeField]
    GameObject mainPanel;

    [SerializeField]
    GameObject inventoryPanel;

    [SerializeField]
    GameObject equipMenu;

    [SerializeField]
    Image questItemImage;

    List<GameObject> menuItemsList = new List<GameObject>();
	CharacterStatsMenu charactersMenu;
	UsableItem itemToUse;
   
    public List<GameObject>  GetMenuItemsList(){
		return menuItemsList;
	}

    protected void Start()
    {
      //  base.Start();
        charactersMenu = charactersListPanel.GetComponent<CharacterStatsMenu>();
        ReloadList();
    }

    private void ReloadList()
    {
        foreach (Item currentItem in ItemsList.items)
        {
            if (currentItem.itemListGameObject == null)
                currentItem.Init();

            currentItem.menuItemPrefab.GetComponentInChildren<GameMenuItem>().quantityText.text = currentItem.quantity.ToString();
            currentItem.ReloadItem(currentItem.quantity);
        }
        SelectFirstItem();
    }

    void Update(){
		if(Input.GetButtonDown(OSInputManager.GetPadMapping("Cancel"))){
            if (charactersListPanel != null && charactersListPanel.activeSelf)
            {
                charactersListPanel.SetActive(false);
                SelectFirstItem();
            }
            else if (itemViewerPanel != null && itemViewerPanel.activeSelf)
            {
                itemViewerPanel.SetActive(false);
                SelectFirstItem();
            }
            else
            {
                mainPanel.SetActive(false);
                ShowManageMenu(inventoryPanel);
            }
				
		}
	}

	public void ManageItem(Text itemTextName){
		Item item = ItemsList.items.Find(p=> p.itemName.Equals(itemTextName.text));

		if (item != null) {
			if(item.itemType == Item.ItemType.Usable) {
				if (itemViewerPanel!=null && itemViewerPanel.activeSelf)
					itemViewerPanel.SetActive (false);
			
				itemToUse = (UsableItem)item;
				charactersListPanel.SetActive (true);
				Button firstCharacterButton = GameObject.FindWithTag ("CharactersList").transform.GetChild (0).GetComponent<Button>();
				StartCoroutine ("SelectButton", firstCharacterButton);
			}
            if (item.itemType == Item.ItemType.Equipment)
            {
                guiController.HideGameObject(inventoryPanel);
                guiController.ShowGameObject(equipMenu);
            }

			if (item.itemType == Item.ItemType.Quest)
			{
		        QuestItem questItem = (QuestItem)item;
                if (questItemImage != null)
                {
                    ShowItemImage(questItem.itemImage);
                    guiController.PlayItemSound(SoundType.ITEM_SELECTED);
                }
			}
        }
	}
		
	void OnDisable(){
		if (itemViewerPanel!=null && itemViewerPanel.activeSelf)
		itemViewerPanel.SetActive (false);

		if (charactersListPanel!= null && charactersListPanel.activeSelf)
		charactersListPanel.SetActive (false);

	}


	void ShowItemImage(Sprite image){
		if (charactersListPanel!= null && charactersListPanel.activeSelf)
			charactersListPanel.SetActive (false);
	
		if (itemViewerPanel != null && !itemViewerPanel.activeSelf) {
			itemViewerPanel.SetActive (true);
			questItemImage.sprite = image;
			questItemImage.preserveAspect = true;
		}

	}

	public void ApplyChosenItem(GameObject charNameGameObject){
		if (itemToUse.quantity > 0) {
			Text charNameText = charNameGameObject.GetComponent<Text> ();
			PlayableCharacter character = CharacterParty.charactersParty.Find (p => p.charStats.name.Equals (charNameText.text));
			if (character != null) {
				itemToUse.UseItem (character);
				charactersMenu.LoadChars (); //Esto se puede optimizar
				UpdateMenuItemQuantity();
			}
		}
	}

	void UpdateMenuItemQuantity(){

        GameObject item = itemListObject.Find (itemToUse.menuItemPrefab.name + "(Clone)").gameObject;
		if (itemToUse.quantity > 0) {
			Text quantityText = item.transform.Find ("QuantityText").GetComponent<Text> ();
			quantityText.text = itemToUse.quantity.ToString ();
		} else {
			ItemsList.items.Remove (itemToUse);
			Destroy (item);
			charactersListPanel.SetActive (false);
            IsLoading = true;
            StartCoroutine("WaitBeforeSelecting");
        }
	}

    IEnumerator WaitBeforeSelecting()
    {
        yield return null;
        SelectFirstItem();
    }

	public void HideImage(){
	if (itemViewerPanel.activeSelf)
		itemViewerPanel.SetActive (false);
	}
}
