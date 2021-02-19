using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandsPanel : Menu {

    public Button firstButton;

    protected  void OnEnable()
    {
        IsLoading = true;
        StartCoroutine("SelectButton", firstButton);
    }

}
