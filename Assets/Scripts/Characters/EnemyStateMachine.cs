using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using SmartLocalization;

public class EnemyStateMachine : BaseBattleStateMachine
{

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    float currentCooldown = 0f;
    [SerializeField]
    protected float maxCooldown = 100f;

	public Text nameText;
	private Vector3 startPosition;

    private bool actionStarted = false;

    float animSpeed = 10f;
    public GameObject selector;
    public GameObject menuPanel { get; set; }
    Animator anim;
    AnimationClip deathClip;

    bool alive = true;
    LanguageManager m_LanguageManagerInstance;

    void Start () {
		character = GetComponent<Enemy>();
        currentState = TurnState.PROCESSING;

		charStats = character.charStats;
		nameText.text = character.charStats.name;
		battleController = GameObject.FindWithTag ("BattleManager").GetComponent<BattleController> ();
		startPosition = transform.position;
        selector.SetActive(false);
        m_LanguageManagerInstance = LanguageManager.Instance;
        if (nameText != null && nameText.text.Contains("."))
            nameText.text = m_LanguageManagerInstance.GetTextValue(character.charStats.name);
        currentCooldown = Random.Range(0f, 2.5f) * 10;
        anim = gameObject.GetComponent<Animator>();
        deathClip = FindAnimationClip("die");
    }
		
    AnimationClip FindAnimationClip(string name)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        for (int i = 0; i< clips.Length; ++i)
        {
            if (clips[i].name.Equals(name))
                return clips[i];
        }
        return null;
    }
	void Update () {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpgradeProgressBar();
                break;

		    case (TurnState.CHOOSEACTION):
			    ChooseAction ();
			    currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                break;

            case (TurnState.ACTION):
                StartCoroutine("TimeForAction");
                break;

            case (TurnState.DEAD):
                if (!alive)
                {
                    return;
                }
                else
                {
                   StartCoroutine (KillEnemy());
                }
                break;

        }	
 
	}

    private IEnumerator KillEnemy()
    {
        this.gameObject.tag = "DeadEnemy";
        //not attackable by hero
        battleController.EnemiesInBattle.Remove(gameObject);
        //disable selector
        selector.SetActive(false);
        //remove all input enemy attacks
        if (battleController.EnemiesInBattle.Count > 0)
        {
            for (int i = 0; i < battleController.performList.Count; ++i)
            {
                if (i != 0)
                {
                    if (battleController.performList[i].AttacksGameObject == gameObject)
                    {
                        battleController.performList.Remove(battleController.performList[i]);
                    }

                    if (battleController.performList[i].AttackersTarget == gameObject)
                    {
                        battleController.performList[i].AttackersTarget = battleController.EnemiesInBattle[Random.Range(0, battleController.EnemiesInBattle.Count)];
                    }
                }

            }
        }

        //change color / Play animation
       
        anim.SetTrigger("die");
        battleController.PlaySound(SoundType.DEAD_ENEMY);
        //Add experience
        battleController.DefeatedEnemies.Add(gameObject);
        //set alive false
        alive = false;
        //reset enemy buttons
        battleController.UpdateEnemyButtons(menuPanel);
        //check if battle is won/lost

        float seconds = 0;
        if(deathClip != null)
            seconds = deathClip.length;
        yield return new WaitForSeconds(seconds + 1);
       
        battleController.battleStates = BattleController.PerformAction.CHECKALIVE;
    }

    void UpgradeProgressBar()
    {
        currentCooldown += charStats.totalSpeed * Time.deltaTime;

        if(currentCooldown >= maxCooldown)
        {
			currentState = TurnState.CHOOSEACTION;
        }
    }

	void ChooseAction(){
		HandleTurn myAttack = new HandleTurn ();
		myAttack.Attacker = character.name;
        myAttack.Type = "Enemy";
		myAttack.AttacksGameObject = gameObject;
        if (battleController.HeroesInBattle.Count > 0)
        {
            myAttack.AttackersTarget = battleController.HeroesInBattle[Random.Range(0, battleController.HeroesInBattle.Count)];
            int num = Random.Range(0, character.attacksList.Count);
            myAttack.choosenAttack = character.attacksList[num];
            #if UNITY_EDITOR
                Debug.Log(gameObject.name + " has choosen " + m_LanguageManagerInstance.GetTextValue(myAttack.choosenAttack.attackName));
            #endif
            battleController.CollectActions(myAttack);
        }
	}

    IEnumerator TimeForAction()
    {
        if(actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        yield return StartCoroutine(battleController.performList[0].choosenAttack.ExecuteEffect(characterToAttack, startPosition, animSpeed, this));

        //Borrar pj de la lista de acciones pendientes
        battleController.performList.RemoveAt(0);
        //Reset BSM -> Wait
        battleController.battleStates = BattleController.PerformAction.WAIT;
        actionStarted = false;
        //resetear estado del pj

        currentCooldown = 0f;
        currentState = TurnState.PROCESSING;
        yield return null;
    }

  
    public override void TakeDamage(int damage)
    {
        TakeDamage(damage, Vector3.zero);
      
    }

    public override void TakeDamage(int damage, Vector3 damagePosition)
    {
        base.TakeDamage(damage, damagePosition);

        if (charStats.currentHealthPoints <= 0)
        {
            charStats.currentHealthPoints = 0;
            currentState = TurnState.DEAD;
        }

    }
}
