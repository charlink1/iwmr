using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuPanel : MonoBehaviour {

    [SerializeField]
    protected GameObject firstButton;

    protected void OnEnable()
    {
        StartCoroutine("SelectButton", firstButton);
    }

    public IEnumerator SelectButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(button);
    }


    public void ResetButtonSelection(GameObject button)
    {
        StartCoroutine("SelectButton", button);
    }



}
