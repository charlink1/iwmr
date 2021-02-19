using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GameMenuController : MonoBehaviour {

    [SerializeField]
    Button firstButton;
    [SerializeField]
    Text clockField;

    [SerializeField]
    Text timerField;

    void Update () {
        clockField.text = DateTime.Now.ToString("HH : mm : ss ");
        timerField.text = CalculateTimer();
    }

    string CalculateTimer()
    {
        float minutes, seconds, hours;
        float timer = Time.timeSinceLevelLoad + Game.timePlayed;
        hours = (int)(timer / 3600f);
        minutes = (int)(timer / 60f);
        seconds = (int)(timer % 60f);
        return hours.ToString("00") + " : " + minutes.ToString("00") + " : " + seconds.ToString("00");
        
    }

    void OnEnable()
    {
        SelectFirstItem( firstButton);
    }

    void SelectFirstItem(Button button)
    {
        StartCoroutine("SelectButton", button);
    }

    IEnumerator SelectButton(Button button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        if (!button.IsDestroyed() && button != null)
            EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
}
