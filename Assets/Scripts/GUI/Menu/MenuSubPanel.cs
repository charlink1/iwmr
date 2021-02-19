using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSubPanel : MainMenuPanel {

    [SerializeField]
    MainMenuController menuController;

    [SerializeField]
    bool activateMainPanel;

    [SerializeField]
    AudioSource backAudio;
    [SerializeField]
    GameObject prevButton;

    GameObject currentPanel;


    void Awake()
    {
        currentPanel = this.gameObject;
    }


    void Update()
    {
        if (Input.GetButtonDown(OSInputManager.GetPadMapping("Cancel")))
        {
            StartCoroutine(Cancel());
        }
    }

    public void CancelButton()
    {
        StartCoroutine(Cancel());
    }

    public IEnumerator Cancel()
    {

        if (backAudio != null)
        {
            backAudio.Play();
            yield return new WaitForSeconds(backAudio.clip.length);
        }
        menuController.Cancel(currentPanel, activateMainPanel, prevButton);
    }
}
