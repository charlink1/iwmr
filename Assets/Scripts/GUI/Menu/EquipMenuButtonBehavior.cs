using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipMenuButtonBehavior : MonoBehaviour {


    [SerializeField]
    Transform itemsListTransform;


	public void CheckNextElementDown(){
		Button button = gameObject.GetComponent<Button> ();
		Navigation nav = new Navigation ();
		nav.mode = Navigation.Mode.Explicit;
		nav.selectOnUp = button.navigation.selectOnUp;
		if (itemsListTransform.childCount > 0) {
			Button buttonBelow = itemsListTransform.GetChild (0).GetComponentInChildren<Button> ();
			nav.selectOnDown = buttonBelow;
			button.navigation = nav;
		} else {
			nav.selectOnDown = null;
			button.navigation = nav;
		}
	}
}
