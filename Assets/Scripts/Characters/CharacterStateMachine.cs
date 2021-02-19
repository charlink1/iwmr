using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterStateMachine : BaseBattleStateMachine
{

    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
     //   SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    float currentCooldown = 0f;
    [SerializeField]
    protected float maxCooldown = 100f;

    public Image progressBar;
	public Text nameText;
    public Animator textAnimator { get; set; }
	public Text healthText;
	public Text magicText;
    public GameObject selector;
    public Animator animator
    {
        get; set;
    }
   
    bool actionStarted = false;
    Vector3 startPosition;
    float animSpeed = 10f;

	bool alive = true;
	bool damagedTextColor = false;

    public PlayableCharacter GetCharacter()
    {
        return (PlayableCharacter)character;
    }

    void Start () {
        character = GetComponent<PlayableCharacter>();
        currentState = TurnState.PROCESSING;

		charStats = character.charStats;
		nameText.text = character.charStats.name;
        UpdateHeroPanel();
        battleController = GameObject.FindWithTag("BattleManager").GetComponent<BattleController>();
        selector.SetActive(false);
        startPosition = transform.position;
        currentCooldown = Random.Range(0f, 2.5f) * 10;
        animator = GetComponent<Animator>();
        textAnimator = nameText.GetComponent<Animator>();
    }

	string GetStatString(string current, string total){
		return current + " / " + total; 
	}
	
	void Update () {
        switch (currentState)
        {
            //Mientras espera a que la barra de espera llegue al final
            case (TurnState.PROCESSING):
                ProcessWhileWaitingTurn();
                break;

            //Añade el pj a la lista de pjs preparados
            case (TurnState.ADDTOLIST):
                battleController.HeroesToManage.Add(this.gameObject);
                currentState = TurnState.WAITING;
            break;

            //Listo y esperando para llevar a cabo la acción
            case (TurnState.WAITING):
                break;

            //case (TurnState.SELECTING):
            //    break;

            //Ejecuta la acción asignada
            case (TurnState.ACTION):               
                StartCoroutine("TimeForAction");
                break;

            //Pj KO
		    case (TurnState.DEAD):
                //Si el pj ya está muerto
			    if (!alive) {
                    //Controlar que ocurre si vuelve a la vida;
                    if (charStats.currentHealthPoints > 0)
                    {
                        ManagePlayerResurrect();
                    }
                    else
                        return;
                //En caso contrario... matarlo
                } else
                    StartCoroutine(SetCharacterKO());

                break;
        }	
	}

    private IEnumerator SetCharacterKO()
    {
        //change tag
        this.gameObject.tag = "DeadHero";
        nameText.color = Color.white;
        damagedTextColor = true;
        //not attackable
        battleController.HeroesInBattle.Remove(this.gameObject);
        //not manageable
        battleController.HeroesToManage.Remove(this.gameObject);
        //deactivate the selector
        selector.SetActive(false);
        //reset GUI
        battleController.actionPanel.SetActive(false);
        textAnimator.SetBool("colorChange", false);

        battleController.enemySelectPanel.SetActive(false);
        //remove item from performList
        if (battleController.HeroesInBattle.Count > 0)
        {
            for (int i = 0; i < battleController.performList.Count; ++i)
            {
                if (battleController.performList[i].AttacksGameObject == gameObject)
                {
                    battleController.performList.RemoveAt(i);
                }

                if (battleController.performList[i].AttackersTarget == gameObject)
                {
                    battleController.performList[i].AttackersTarget = battleController.HeroesInBattle[Random.Range(0, battleController.HeroesInBattle.Count)];
                }

                //Eliminar la acción del personaje muerto de la lista de performlist
                if(battleController.performList[i].Attacker == character.name)
                    battleController.performList.RemoveAt(i);
            }
        }
        //change color / play dead animation

        healthText.color = Color.red;

        //reset hero input      
        alive = false;
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(1);
        battleController.battleStates = BattleController.PerformAction.CHECKALIVE;
    }

    private void ManagePlayerResurrect()
    {
        this.gameObject.tag = "Player";
        currentCooldown = 0;
        battleController.HeroesInBattle.Add(this.gameObject);
        alive = true;
        animator.SetBool("dead", false);
        UpdateHeroPanel();
        currentState = TurnState.PROCESSING;
        battleController.battleStates = BattleController.PerformAction.CHECKALIVE;
    }

    private void ProcessWhileWaitingTurn()
    {
        int currentHealth = character.charStats.currentHealthPoints;
        if (battleController.EnemiesInBattle.Count == 0 && battleController.battleStates != BattleController.PerformAction.WIN)
            battleController.battleStates = BattleController.PerformAction.WIN;


        if (currentHealth > 0)
        {
            if (animator.GetBool("dead"))
                animator.SetBool("dead", false);

            //Cambiar el color del texto del nombre si el pj está dañado
            UpdateTextColor(currentHealth);
            UpgradeProgressBar();
        }

        else
            currentState = TurnState.DEAD;
    }

    private void UpdateTextColor(int currentHealth)
    {
        if (!damagedTextColor && currentHealth < (charStats.totalHealthPoints * 0.10))
        {
            healthText.color = Color.yellow;
            damagedTextColor = true;
        }
        else if (damagedTextColor && currentHealth >= (charStats.totalHealthPoints * 0.10))
        {
            healthText.color = Color.white;
            damagedTextColor = false;
        }
    }

    void UpgradeProgressBar()
    {
        currentCooldown += charStats.totalSpeed * Time.deltaTime;
        float calcCooldown = currentCooldown / maxCooldown;
        Vector3 localScale = progressBar.transform.localScale;
        progressBar.transform.localScale = new Vector3(Mathf.Clamp01(calcCooldown), localScale.y, localScale.z);

        if(currentCooldown >= maxCooldown)
        {
            currentState = TurnState.ADDTOLIST;
        }
    }

    IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

       yield return StartCoroutine(battleController.performList[0].choosenAttack.ExecuteEffect(characterToAttack, startPosition, animSpeed, this));

        //Borrar pj de la lista de acciones pendientes
        battleController.performList.RemoveAt(0);
        //Reset BSM -> Wait
        if (battleController.battleStates != BattleController.PerformAction.WIN && battleController.battleStates != BattleController.PerformAction.LOSE)
        {
            battleController.battleStates = BattleController.PerformAction.WAIT;

            //resetear estado del pj

            currentCooldown = 0f;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }

        actionStarted = false;
        yield return null;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (charStats.currentHealthPoints <= 0)
        {
            charStats.currentHealthPoints = 0;
            currentState = TurnState.DEAD;
        }
        UpdateHeroPanel();
    }

    public override void UpdateHeroPanel()
    {
        healthText.text = GetStatString(charStats.currentHealthPoints.ToString(), charStats.totalHealthPoints.ToString());
        magicText.text = GetStatString(charStats.currentMagicPoints.ToString(), charStats.totalMagicPoints.ToString());
    }
}
