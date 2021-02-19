using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    [SerializeField]
    protected GUIController guiController;
    [SerializeField]
    protected BattleController battleController;

    private bool isLoading = true;

    public bool IsLoading
    {
        get
        {
            return isLoading;
        }

        set
        {
            isLoading = value;
        }
    }

    public BattleController GetBattleController(){
        return battleController;
    }
    public IEnumerator SelectButton(Button button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        if (!button.IsDestroyed() && button != null)
            EventSystem.current.SetSelectedGameObject(button.gameObject);
       
    }

    void OnDisable()
    {
        isLoading = true;
    }

    public void SetNavigation(List<GameObject> buttonsList)
    {
        if (buttonsList.Count > 1)
        {
            for (int i = 0; i < buttonsList.Count; ++i)
            {
                Button button = buttonsList[i].GetComponentInChildren<Button>();
                Navigation navigation = new Navigation();
                navigation.mode = Navigation.Mode.Explicit;
                if (i == 0) //First button
                    navigation.selectOnDown = buttonsList[1].GetComponentInChildren<Button>();
                else if (i == buttonsList.Count - 1) //Last button
                    navigation.selectOnUp = buttonsList[buttonsList.Count - 2].GetComponentInChildren<Button>();
                else
                {
                    navigation.selectOnDown = buttonsList[i + 1].GetComponentInChildren<Button>();
                    navigation.selectOnUp = buttonsList[i - 1].GetComponentInChildren<Button>();
                }
                button.navigation = navigation;
            }
        }
    }

    public void SetNavigation(List<Button> buttonsList)
    {
        if (buttonsList.Count > 1)
        {
            for (int i = 0; i < buttonsList.Count; ++i)
            {
                Button button = buttonsList[i];
                Navigation navigation = new Navigation();
                navigation.mode = Navigation.Mode.Explicit;
                if (i == 0) //First button
                    navigation.selectOnDown = buttonsList[1];
                else if (i == buttonsList.Count - 1) //Last button
                    navigation.selectOnUp = buttonsList[buttonsList.Count - 2];
                else
                {
                    navigation.selectOnDown = buttonsList[i + 1];
                    navigation.selectOnUp = buttonsList[i - 1];
                }
                button.navigation = navigation;

            }
        }
    }

    public void SetNavigation(Button[] buttonsList)
    {
        if (buttonsList.Length > 1)
        {
            for (int i = 0; i < buttonsList.Length; ++i)
            {
                Button button = buttonsList[i];
                Navigation navigation = new Navigation();
                navigation.mode = Navigation.Mode.Explicit;
                if (i == 0) //First button
                    navigation.selectOnDown = buttonsList[1];
                else if (i == buttonsList.Length - 1) //Last button
                    navigation.selectOnUp = buttonsList[buttonsList.Length - 2];
                else
                {
                    navigation.selectOnDown = buttonsList[i + 1];
                    navigation.selectOnUp = buttonsList[i - 1];
                }
                button.navigation = navigation;

            }
        }
    }

    public void PlayItemSelectedSound(bool battle = false)
    {
        if (!battle)
            guiController.PlayItemSound(SoundType.ITEM_SELECTED);
        else
            battleController.PlaySound(SoundType.ITEM_SELECTED);
    }
}

    

