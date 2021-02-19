using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using SmartLocalization;


public enum BattleAction { Attack, UseItem, Wait}

public class BattleController : MonoBehaviour {
    
   //Lista de pjs participantes en la batalla
    List<Character> charatersList = new List<Character>();
   
    [SerializeField]
    string m_ItemsTextKey;

    [SerializeField]
    string m_MagicTextKey;

    [SerializeField]
    string m_AttackTextKey;
    
    [SerializeField]
    Sprite finalBattleSprite;

    [SerializeField]
    SpriteRenderer[] backgroundRendererSprites;

    [SerializeField]
    GameObject battleMessagePanel;

    [SerializeField]
    FloatingText floatingTextPrefab;

    [SerializeField]
    Transform characterBattleStatsPanel;

    [SerializeField]
    GameObject characterBattleStatsPrefab;

    [SerializeField]
    Transform enemyBattlePanel;

    [SerializeField]
    GameObject enemyPanelPrefab;

    //public GameObject TestEnemyPrefab;

    [SerializeField]
    string progressBarName;
    [SerializeField]
    string nameTextName;
    [SerializeField]
    string HPTextName;
    [SerializeField]
    string MPTextName;

    //public string enemyListName;
    [SerializeField]
    string enemyNameTextName;

    [SerializeField]
    Transform[] m_CharacterPositions;

   
   
	Animator fadeAnim;
    Text battleMessage;

    public enum PerformAction{
		WAIT,
		TAKEACTION,
		PERFORMACTION,
        CHECKALIVE,
        WIN, 
        LOSE
	}

    public enum HeroGUI {
        ACTIVATE, 
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

	public PerformAction battleStates;
    public HeroGUI HeroInput;

    public List<HandleTurn> performList = new List<HandleTurn>();
	public List<GameObject> HeroesInBattle = new List<GameObject> ();
	public List<GameObject> EnemiesInBattle = new List<GameObject> ();
    public List<GameObject> DefeatedEnemies = new List<GameObject>();
    public List<GameObject> HeroesToManage = new List<GameObject>();

    public GameObject actionPanel;
    public GameObject enemySelectPanel;

    [SerializeField]
    public GameObject magicPanel;
    [SerializeField]
    public GameObject itemPanel;

    //attack of heroes
    [SerializeField]
    Transform actionSpacer;
    [SerializeField]
    Transform magicSpacer;
    [SerializeField]
    Transform itemsSpacer;

    [SerializeField]
    GameObject actionButton;
    [SerializeField]
    GameObject magicAttacksButton;
    [SerializeField]
    GameObject itemsButton;
    [SerializeField]
    bool afterCombat = false;

    [SerializeField]
    AudioSource[] battleBgMusic; //0 randomBattle, 1 MidBoss, 2 FinalBoss, 3Game Over
    [SerializeField]
    AudioSource[] enemyDeadSound; //0 randomBattle, 1 MidBoss, 2 FinalBoss

    [SerializeField]
    AudioSource commandSelectedSound;
    [SerializeField]
    AudioSource itemObtainedSound;
    [SerializeField]
    AudioSource itemUsedSound;
    [SerializeField]
    AudioSource lvlUpSound;
    [SerializeField]
    AudioSource playerCommandsPanelSound;

    AudioSource currentAudioSourceMusic;
    LanguageManager m_LanguageManagerInstance;
   
    List<GameObject> attackButtonsList = new List<GameObject>();
    List<UsableItem> usableItems = new List<UsableItem>();
    List<Button> magicButtonsList = new List<Button>();
    List<Button> itemButtonsList = new List<Button>();

    //enemy buttons
    List<GameObject> enemyBtns = new List<GameObject>();
    GameManager gameManager;
    List<GameObject> enemiesList;

    private HandleTurn heroChoice;

    public Button tempAttackButton { get; set; }

    void Start ()
    {
        gameManager = GameManager.instance;
        PlayBattleMusic();
        Game.isFading = true;
        fadeAnim = GameObject.FindWithTag("FadeImage").GetComponent<Animator>();
        LoadCharacters();
        LoadEnemies();
        battleStates = PerformAction.WAIT;
        HeroInput = HeroGUI.ACTIVATE;
        //  Debug.Log(SceneManager.GetActiveScene().name);
        actionPanel.SetActive(false);
        magicPanel.SetActive(false);
        enemySelectPanel.SetActive(false);
        FloatingTextController.Initialize(floatingTextPrefab);
        if (m_LanguageManagerInstance == null)
            m_LanguageManagerInstance = LanguageManager.Instance;
        battleMessage = battleMessagePanel.GetComponentInChildren<Text>();
    }

    private void PlayBattleMusic()
    {
        if (gameManager.bossBattle)
        {
            //musiqueta de mid Boss;
            currentAudioSourceMusic = battleBgMusic[1];
            gameManager.bossBattle = false;
        }
        else if (gameManager.finalBossBattle)
        {
            //musiqueta de Boss;
            currentAudioSourceMusic = battleBgMusic[2];
            //gameManager.finalBossBattle = false;

            for (int i = 0; i < backgroundRendererSprites.Length; ++i)
            {
                backgroundRendererSprites[i].sprite = finalBattleSprite;
                Vector3 bgPosition = backgroundRendererSprites[i].transform.position;
                bgPosition.x = 0.01f;
                backgroundRendererSprites[i].transform.position = bgPosition;
            }
            backgroundRendererSprites[1].enabled = false;
            
        }
        else
        {
            currentAudioSourceMusic = battleBgMusic[0];
        }
        currentAudioSourceMusic.Play();
    }

    void LoadSceneAfterBattle()
    {
        GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
        gameManager.canGetEncounter = true;
        fadeAnim.SetTrigger("fadeOn");
        if (gameManager.enemiesToBattle.Count > 0)
            gameManager.enemiesToBattle.Clear();

        Game.timePlayed += Time.timeSinceLevelLoad;
        Game.returningFromBattle = true;
    }
	
	void Update ()
    {
    
        HandleMainStateMachine();

        switch (HeroInput)
        {
            case HeroGUI.ACTIVATE:
                if(HeroesToManage.Count > 0)
                {
                    Animator anim = HeroesToManage[0].GetComponent<CharacterStateMachine>().textAnimator;
                    if (!anim.GetBool("colorChange"))
                        anim.SetBool("colorChange", true);

                    heroChoice = new HandleTurn();
                    actionPanel.SetActive(true);
                    playerCommandsPanelSound.Play();
                    CreateAttackButtons();
                    HeroInput = HeroGUI.WAITING;
                }
                break;

            case HeroGUI.WAITING:
                break;

            case HeroGUI.INPUT1:
                break;

            case HeroGUI.INPUT2:
                break;

            case HeroGUI.DONE:
                HeroInputDone();
                break;
        }
    }

    IEnumerator CheckToReload()
    {
        yield return new WaitForSeconds(0.5f);
        if (actionPanel.activeSelf && EventSystem.current.currentSelectedGameObject == null)
        {
            ClearAttackPanel();
            CreateAttackButtons();
        }
    }

    private void HandleMainStateMachine()
    {
        switch (battleStates)
        {
            case (PerformAction.WAIT):
                if (performList.Count > 0)
                {
                    battleStates = PerformAction.TAKEACTION;
                }
                break;
            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(performList[0].Attacker);
                if (performList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();

					for (int i = 0; i < HeroesInBattle.Count; ++i) {
						if (performList [0].AttackersTarget == HeroesInBattle [i]) {
							ESM.characterToAttack = performList [0].AttackersTarget;
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
							break;
						} else {
							performList [0].AttackersTarget = HeroesInBattle [Random.Range (0, HeroesInBattle.Count)];
							ESM.characterToAttack = performList [0].AttackersTarget;
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
						}
					}
                    
                }

                if (performList[0].Type == "Hero")
                {
                    CharacterStateMachine HSM = performer.GetComponent<CharacterStateMachine>();
                    HSM.characterToAttack = performList[0].AttackersTarget;
                    HSM.currentState = CharacterStateMachine.TurnState.ACTION;
                }

                battleStates = PerformAction.PERFORMACTION;
                break;

            case (PerformAction.PERFORMACTION):

                break;
            case (PerformAction.CHECKALIVE):
                if (HeroesInBattle.Count < 1)
                {
                    ClearAttackPanel();
                    battleStates = PerformAction.LOSE;
                }

                else if (EnemiesInBattle.Count < 1)
                    battleStates = PerformAction.WIN;
                else
                {
                    ClearAttackPanel();
                    HeroInput = HeroGUI.ACTIVATE;
                    battleStates= PerformAction.WAIT;
                }

            break;
            case (PerformAction.LOSE):
                #if UNITY_EDITOR
                    Debug.Log("You've lost");
                #endif
                currentAudioSourceMusic.Stop();
                battleBgMusic[3].Play();
                fadeAnim.SetTrigger("fadeGameOver");

                break;
            case (PerformAction.WIN):
                #if UNITY_EDITOR
                    Debug.Log("You won");
                #endif

                if (!afterCombat)
                {
                    afterCombat = true;
                    for (int i = 0; i < HeroesInBattle.Count; ++i)
                    {
                        HeroesInBattle[i].GetComponent<CharacterStateMachine>().currentState = CharacterStateMachine.TurnState.WAITING;
                    }
                    StartCoroutine("FinishBattle");
                }
                    
                break;
        }
    }

    IEnumerator FinishBattle()
    {
        yield return new WaitForSeconds(1);
        //Add experience
      
        int amountExperience = 0;
        bool hasLoot = false;
        Item item = null;
        foreach (GameObject enemyGO in DefeatedEnemies)
        {
            Enemy enemy = enemyGO.GetComponent<Enemy>();
            amountExperience += enemy.Experience;
            if (!hasLoot && Random.Range(1, 100) <= enemy.lootProbability && enemy.lootList.Count > 0)
            {
                hasLoot = true;
                item = enemy.lootList[Random.Range(0, enemy.lootList.Count)];    
                item.AddItemInBattle(1);
            }

        }

        foreach(GameObject heroGO in HeroesInBattle)
        {
            PlayableCharacter character = heroGO.GetComponent<PlayableCharacter>();
            if (character.charStats.currentHealthPoints > 0)
            {
                character.charStats.AddExperience(amountExperience / HeroesInBattle.Count);
                if (character.charStats.levelUp)
                {
                    character.charStats.levelUp = false;
                    string lvlUpText = character.charStats.name + " "+ m_LanguageManagerInstance.GetTextValue("Character.Levelup");
                    lvlUpSound.Play();
                    yield return StartCoroutine("ShowCombatText", lvlUpText);
                }

            }
        }
        //Add items if needed

        if (hasLoot && item != null)
        {
            string text = item.itemName + " " + m_LanguageManagerInstance.GetTextValue("Item.Loot");
            itemObtainedSound.Play();
            yield return StartCoroutine("ShowCombatText", text);
        }

        DefeatedEnemies.Clear();
        yield return new WaitForSeconds(1);
        LoadSceneAfterBattle();
        
    }

    IEnumerator ShowCombatText(string text)
    {
        battleMessagePanel.gameObject.SetActive(true);
        battleMessage.text = text;
        yield return new WaitForSeconds(2);
        battleMessage.text = null;
        battleMessagePanel.gameObject.SetActive(false);
    }

    public void CollectActions(HandleTurn input){
		performList.Add (input);
	}
    
    void LoadCharacters()
    {
        if (CharacterParty.charactersParty != null) {
            if (CharacterParty.charactersParty.Count == 1)
            {
                LoadCharacter(0, m_CharacterPositions[1]);
            }else if(CharacterParty.charactersParty.Count == 2)
            {
                Transform[] tempTransform = { m_CharacterPositions[0], m_CharacterPositions[2] };
                for(int i = 0; i< CharacterParty.charactersParty.Count; ++i)
                {
                    LoadCharacter(i, tempTransform[i]);
                }
            }
            else
            {
                for (int i = 0; i < CharacterParty.charactersParty.Count; ++i)
                {
                    LoadCharacter(i, m_CharacterPositions[i]);
                }
            }
        }
    }

    void LoadEnemies()
    {
        enemiesList = gameManager.enemiesToBattle;
        if(enemiesList != null && enemiesList.Count > 0)
        {
            if(gameManager.finalBossBattle)
                LoadEnemy(0, m_CharacterPositions[6]);
            else if (enemiesList.Count == 1)
            {
                LoadEnemy(0, m_CharacterPositions[4]);
            }
            else if(enemiesList.Count == 2)
            {
                int counter = 2;
                Transform[] tempTransform = { m_CharacterPositions[3], m_CharacterPositions[5] };
                for (int i = 0; i < enemiesList.Count; ++i)
                {
                    GameObject enemy = LoadEnemy(i, tempTransform[i]);
                    enemy.GetComponent<SpriteRenderer>().sortingOrder = counter;
                    counter--;
                }
            }
            else
            {
                int counter = 1;
                for(int i = 0; i<enemiesList.Count; ++i)
                {
                    GameObject enemy = LoadEnemy(i, m_CharacterPositions[i+3]);
                    enemy.GetComponent<SpriteRenderer>().sortingOrder = counter;
                    counter++;
                }
            }

        }
    }

    private void LoadCharacter(int characterIndex, Transform objectTransform)
    {
        PlayableCharacter playCharacter = CharacterParty.charactersParty[characterIndex];
        GameObject characterGO = Instantiate(playCharacter.prefab);
        characterGO.transform.position= objectTransform.position;
        characterGO.transform.rotation = objectTransform.rotation;
        PlayableCharacter character = characterGO.GetComponent<PlayableCharacter>();
        character.charStats = playCharacter.charStats;
        character.currentEquipment = playCharacter.currentEquipment;
        character.magicAttacks = playCharacter.magicAttacks;
        character.attacksList = playCharacter.attacksList;

        GameObject charStatsPanel = Instantiate(characterBattleStatsPrefab, characterBattleStatsPanel, false);
        CharacterStateMachine stateMachine = characterGO.GetComponent<CharacterStateMachine>();

        CharacterSelected button = charStatsPanel.GetComponentInChildren<CharacterSelected>();
        button.characterPrefab = characterGO;

        Transform stateMachineTransform = charStatsPanel.transform.Find(progressBarName);
		stateMachine.progressBar = stateMachineTransform.GetComponent<Image>();
		stateMachine.nameText = charStatsPanel.transform.Find(nameTextName).GetComponent<Text>();
		stateMachine.healthText = charStatsPanel.transform.Find(HPTextName).GetComponent<Text>();
		stateMachine.magicText = charStatsPanel.transform.Find(MPTextName).GetComponent<Text>();

        charatersList.Add(playCharacter);
		HeroesInBattle.Add (characterGO);
    }

    private GameObject LoadEnemy(int characterIndex, Transform objectTransform)
    {
        if(enemiesList == null)
            enemiesList = gameManager.enemiesToBattle;

        Enemy currentEnemy = enemiesList[characterIndex].GetComponent<Enemy>();
        GameObject enemyGO = Instantiate (currentEnemy.prefab);
		enemyGO.transform.position= objectTransform.position;
		enemyGO.transform.rotation = objectTransform.rotation;
		Enemy character = enemyGO.GetComponent<Enemy>();

        if (m_LanguageManagerInstance == null)
            m_LanguageManagerInstance = LanguageManager.Instance;
        enemyGO.name = m_LanguageManagerInstance.GetTextValue(character.charStats.name) + " "+characterIndex +1;
		character.charStats = enemyGO.GetComponent<Enemy>().charStats;
		character.currentEquipment = enemyGO.GetComponent<Enemy>().currentEquipment;

		GameObject enemyPanel = Instantiate(enemyPanelPrefab, enemyBattlePanel,false);
		EnemyStateMachine stateMachine = enemyGO.GetComponent<EnemyStateMachine>();
      
        EnemySelectedButton button = enemyPanel.GetComponentInChildren<EnemySelectedButton>();
        button.characterPrefab = enemyGO;
        stateMachine.menuPanel = enemyPanel;
        enemyBtns.Add(enemyPanel);
	
		stateMachine.nameText = enemyPanel.transform.Find(enemyNameTextName).GetComponent<Text>();
		EnemiesInBattle.Add (enemyGO);
        return enemyGO;

    }

    public void UpdateEnemyButtons(GameObject panel)
    {
        Destroy(panel);
        enemyBtns.Remove(panel);
    }

    public void Input1()//Attack button
    {
        //PlaySound(SoundType.ITEM_USED);

        heroChoice.Attacker = HeroesToManage[0].name; //o el nombre del pj
        heroChoice.AttacksGameObject = HeroesToManage[0];
        heroChoice.Type = "Hero";
        heroChoice.choosenAttack = HeroesToManage[0].GetComponent<CharacterStateMachine>().GetCharacter().attacksList[0];
        actionPanel.SetActive(false);
        enemySelectPanel.SetActive(true);
    }

    public void SelectTarget(GameObject choosenEnemy)
    {
        PlaySound(SoundType.ITEM_USED);

        heroChoice.AttackersTarget = choosenEnemy;
        HeroInput = HeroGUI.DONE;
    }

    void HeroInputDone()
    {
        performList.Add(heroChoice);
        ClearAttackPanel();

        Animator anim = HeroesToManage[0].GetComponent<CharacterStateMachine>().textAnimator;
        if (anim.GetBool("colorChange"))
            anim.SetBool("colorChange", false);

        HeroesToManage.RemoveAt(0);
        HeroInput = HeroGUI.ACTIVATE;
    }

    void ClearAttackPanel()
    {
        EventSystem.current.SetSelectedGameObject(null);
        enemySelectPanel.SetActive(false);
        actionPanel.SetActive(false);
        magicPanel.SetActive(false);

        //clean attack panel
        foreach (GameObject atkbtn in attackButtonsList)
        {
            Destroy(atkbtn);
        }

		foreach (Button mgbtn in magicButtonsList) {
			Destroy (mgbtn.transform.parent.gameObject);
		}

		foreach (Button itmgtn in itemButtonsList) {
			Destroy (itmgtn.transform.parent.gameObject);
		}


        attackButtonsList.Clear();
        magicButtonsList.Clear();
		itemButtonsList.Clear ();
		usableItems.Clear ();
    }

    //Action buttons
    void CreateAttackButtons()
    {
        Button attackButton = CreateButton(m_LanguageManagerInstance.GetTextValue(m_AttackTextKey));
        attackButton.onClick.AddListener(() => Input1());
        tempAttackButton = attackButton;

        CreateMagicButtons();
        CreateIemsButtons();
        ActivateCommandsMenu(attackButton);
    }

    private void CreateMagicButtons()
    {
        PlayableCharacter currentChar = HeroesToManage[0].GetComponent<CharacterStateMachine>().GetCharacter();
        List<BaseAttack> magicAttacksList = currentChar.magicAttacks;
        int CharMana = currentChar.charStats.currentMagicPoints;
        if (magicAttacksList.Count > 0)
        {
            Button magicButton = CreateButton(m_LanguageManagerInstance.GetTextValue(m_MagicTextKey));
            magicButton.onClick.AddListener(() => SwitchToMagicAttacks());
            foreach (BaseAttack magicAttack in magicAttacksList)
            {
                Button magButton = CreateButton(magicAttacksButton, m_LanguageManagerInstance.GetTextValue(magicAttack.attackName), magicSpacer, false);

                AttackButton attButton = magButton.transform.parent.GetComponent<AttackButton>();
                attButton.magicAttackToPerform = magicAttack;
                magicButtonsList.Add(magButton);

                //Controla si se puede realizar la magia
                if (magicAttack.attackCost > CharMana)
                {
                    attButton.DeactivateButton();
                }
                else if(attButton.IsDeactivated())
                {
                    attButton.ActivateButton();
                }

            }
        }
    }

    private void CreateIemsButtons()
    {
       
        for(int i = 0; i< ItemsList.items.Count; ++i)
        {
            if (ItemsList.items[i] is UsableItem)
                usableItems.Add((UsableItem)ItemsList.items[i]);
        }

        if(usableItems.Count > 0)
        {
            Button itemButton = CreateButton(m_LanguageManagerInstance.GetTextValue(m_ItemsTextKey));
            itemButton.onClick.AddListener(() => SwitchToItemsInventory());
            foreach (UsableItem item in usableItems)
            {
                Button usableItemButton = CreateButton(itemsButton, item.itemName, itemsSpacer, false);
                ItemAttackButton itemAttackButton = usableItemButton.transform.parent.GetComponent<ItemAttackButton>();
                itemAttackButton.itemToUse = item.GetComponent<BaseItemAttack>();
				itemAttackButton.itemToUse.item = item;
                itemButtonsList.Add(usableItemButton);
            }
        }
    }

    public void ActivateCommandsMenu(Button attackButton)
    {
        Menu menu = actionPanel.GetComponent<Menu>();
        menu.StartCoroutine("SelectButton", attackButton);
        menu.SetNavigation(attackButtonsList);
    
    }

    Button CreateButton(string buttonText)
    {
        GameObject buttonGameObject = Instantiate(actionButton, actionSpacer, false);
        Button button = buttonGameObject.transform.FindChild("Button").GetComponent<Button>();
        Text attackButtonText = buttonGameObject.transform.FindChild("Text").GetComponent<Text>();
        attackButtonText.text = buttonText;
        attackButtonsList.Add(buttonGameObject);
        return button;
    }

    Button CreateButton(GameObject buttonPrefab, string buttonText, Transform spacer, bool addToActionButtonsList = true)
    {
        GameObject buttonGameObject = Instantiate(buttonPrefab, spacer, false);
        Button button = buttonGameObject.transform.FindChild("Button").GetComponent<Button>();
        Text attackButtonText = buttonGameObject.transform.FindChild("Text").GetComponent<Text>();
        attackButtonText.text = buttonText;
        if(addToActionButtonsList)
            attackButtonsList.Add(buttonGameObject);
        return button;
    }

    public void MagicInput(BaseAttack choosenMagic)//Input 3
    {   
        if (choosenMagic.attackCost <= HeroesToManage[0].GetComponentInChildren<CharacterStateMachine>().character.charStats.currentMagicPoints)
        {
            //PlaySound(SoundType.ITEM_SELECTED);

            heroChoice.Attacker = HeroesToManage[0].name; //o el nombre del pj
            heroChoice.AttacksGameObject = HeroesToManage[0];
            heroChoice.Type = "Hero";
            heroChoice.choosenAttack = choosenMagic;
            magicPanel.SetActive(false);
            enemySelectPanel.SetActive(true);
        }
        else
        {
            PlaySound(SoundType.MENU_BACK);
        }
    }

    public void ItemInput(BaseItemAttack choosenItem)//Input 3
    {
        //  if(choosenItem.item.quantity > 0)
        //PlaySound(SoundType.ITEM_SELECTED);

        heroChoice.Attacker = HeroesToManage[0].name; //o el nombre del pj
        heroChoice.AttacksGameObject = HeroesToManage[0];
        heroChoice.Type = "Hero";

        heroChoice.choosenAttack = choosenItem;
        itemPanel.SetActive(false);
        enemySelectPanel.SetActive(true);

		if (choosenItem.attackEffect == AttackEffect.Healing)
			SelectHero ();
    }

    public void SwitchToMagicAttacks() //swithing to magicAttacks
    {
        //PlaySound(SoundType.ITEM_SELECTED);

        actionPanel.SetActive(false);
        magicPanel.SetActive(true);

        Menu magicMenu = magicPanel.GetComponent<Menu>();
        magicMenu.StartCoroutine("SelectButton", magicButtonsList[0]);
        magicMenu.SetNavigation(magicButtonsList);
    }

    public void SwitchToItemsInventory()
    {
        PlaySound(SoundType.ITEM_SELECTED);

        actionPanel.SetActive(false);
        itemPanel.SetActive(true);

		Menu itemMenu = itemPanel.GetComponent<Menu>();
		itemMenu.StartCoroutine("SelectButton", itemButtonsList[0]);
		itemMenu.SetNavigation(itemButtonsList);
    }

	public void SelectHero(){
		characterBattleStatsPanel.GetComponent<Menu> ().StartCoroutine("SelectButton",characterBattleStatsPanel.GetComponentInChildren<Button> ());
	}

    public void PlaySound(SoundType type)
    {
        switch (type)
        {
            case SoundType.ITEM_SELECTED:
                commandSelectedSound.Play();
                break;
            case SoundType.DEAD_ENEMY:
                enemyDeadSound[0].Play();
                break;
            case SoundType.ITEM_USED:
                itemUsedSound.Play();
                break;

        }
    }

    
}
