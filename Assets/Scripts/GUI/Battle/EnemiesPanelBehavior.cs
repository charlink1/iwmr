using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesPanelBehavior : Menu {

    Button[] buttonsList;

    void OnEnable()
    {
        buttonsList = GetComponentsInChildren<Button>();
        //   SetNavigation(buttonsList);
        IsLoading = true;

        if (buttonsList.Length > 0)
            StartCoroutine("SelectButton", buttonsList[0]);
    }

   
}
