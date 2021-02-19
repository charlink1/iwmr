using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuController : Menu {

    [SerializeField]
    GameObject managementMenu;

    [SerializeField]
    protected Transform itemListObject;

   // protected GUIController guiController;
    

	
	/*protected virtual void Start ()
    {
        FindGUIController();
    }

*/  protected void FindGUIController()
    {
        guiController = GameObject.FindWithTag("GUIController").GetComponent<GUIController>();
    }


    protected virtual void OnEnable()
    {
		SelectFirstItem ();
	}

	protected virtual void SelectFirstItem ()
	{
		if (itemListObject.childCount > 0) {
			StartCoroutine ("SelectButton", itemListObject.GetChild (0).GetComponentInChildren<Button> ());
		}
	}


	protected void ShowManageMenu (GameObject panel)
	{
		guiController.HideGameObject (panel);
		guiController.ShowGameObject (managementMenu);
        guiController.PlayItemSound(SoundType.MENU_BACK);
	}

   

	
}
