using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using SmartLocalization;

public enum SoundType { ITEM_SELECTED, DEAD_ENEMY, MENU_BACK, ITEM_OBTAINED, ITEM_USED, ITEM_WRONG};
public class GUIController : MonoBehaviour {

    [SerializeField]
    GameObject managementMenuPanel;
    [SerializeField]
    GameObject selectedItem;
    [SerializeField]
    Sequencer sequencer;
    [SerializeField]
    GameObject itemObtainedDialog;
    [SerializeField]
    GameObject clockDialog;
    [SerializeField]
    GameObject booksDialog;
    [SerializeField]
    GameObject questionDialog;
    [SerializeField]
    GameObject passwordDialog;
    [SerializeField]
    string menuScene;
    [SerializeField]
    AudioSource itemSelectedSound;
    [SerializeField]
    AudioSource menuBackSound;
    [SerializeField]
    AudioSource itemObtainedSound;
    [SerializeField]
    AudioSource itemUsedSound;
    [SerializeField]
    AudioSource itemWrongSound;
    [SerializeField]
    PlayerController2D playerController;

    public MenuEquipController equipMenuController;
    public string itemsListPath;
    public string equipmentListPath;

    public bool IsMenuActive { get; set; }
    public int characterIndex { get; set; }

    LanguageManager m_LanguageManager;
    MessageDialogController msgController;

    void Start()
    {
        characterIndex = 0;
        msgController = GetComponent<MessageDialogController>();
        if (!m_LanguageManager)
            m_LanguageManager = LanguageManager.Instance;
    }


	void Update () {
		if ((Input.GetButtonDown(OSInputManager.GetPadMapping("Menu")) || (Input.GetButtonDown(OSInputManager.GetPadMapping("Cancel"))) && IsMenuActive) && !sequencer.isOnSequence &&
            !clockDialog.activeSelf && !booksDialog.activeSelf &&!questionDialog.activeSelf &&!passwordDialog.activeSelf &&!msgController.IsDialogActive)
        {
            if (!IsMenuActive)
            {
                ShowGameObject(managementMenuPanel);
                playerController.StopPlayerMove();
                IsMenuActive = true;
                EventSystem.current.SetSelectedGameObject(selectedItem);
            }
            else
            {
                if(managementMenuPanel.activeSelf)
                    CloseMenu();
            }
        } 
	}

    public void UpdateCharacterIndex(int index)
    {

        characterIndex = index;
    }
 
	public IEnumerator ShowItemDialog(string itemName, bool isFemale){
		IsMenuActive = true;
        string obtainedString;

        if (isFemale)
            obtainedString = m_LanguageManager.GetTextValue("Item.LootFem");
        else
            obtainedString = m_LanguageManager.GetTextValue("Item.Loot");

        string text = itemName + obtainedString;
		itemObtainedDialog.SetActive (true);
		itemObtainedDialog.GetComponentInChildren<Text> ().text = text;
		yield return new WaitForSeconds (2);
		IsMenuActive = false;
		itemObtainedDialog.SetActive (false);

	}

    public IEnumerator AddItemNumber(GameObject itemPrefab)
    {
        yield return null;
        itemPrefab.GetComponent<QuestItem>().AddItem(1);
        sequencer.PerformNextSequenceStep();

    }

    public IEnumerator AddEquipableItem(GameObject itemPrefab)
    {
        yield return null;
        itemPrefab.GetComponent<EquipableItem>().AddItem(1);
        sequencer.PerformNextSequenceStep();

    }

    public IEnumerator ActivatePanel(GameObject itemPrefab)
    {
        yield return null;
        itemPrefab.SetActive(true);
        sequencer.PerformNextSequenceStep();

    }

    public void ShowGameObject(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void HideGameObject(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void CloseMenu()
    {
        HideGameObject(managementMenuPanel);
        IsMenuActive = false;   
    }

    public void SelectMenuButton(GameObject button)
    {
        StartCoroutine("SelectButton", button);
    }

    public IEnumerator SelectButton(GameObject button)
    {

        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void ExitGame()
    {
        Game.ResetGame();
        SceneManager.LoadScene(menuScene);
    }

    public void ShowSelectedMenu(GameObject panel)
    {
        ShowGameObject(panel);
        PlayItemSound(SoundType.ITEM_SELECTED);
    }

    public void PlayItemSound(SoundType type)
    {
        switch (type)
        {
            case SoundType.ITEM_SELECTED:
                itemSelectedSound.Play();
                break;
            case SoundType.MENU_BACK:
                menuBackSound.Play();
                break;
            case SoundType.ITEM_OBTAINED:
                itemObtainedSound.Play();
                break;
            case SoundType.ITEM_USED:
                itemUsedSound.Play();
                break;
            case SoundType.ITEM_WRONG:
                itemWrongSound.Play();
                break;
        }
    }
  
}
