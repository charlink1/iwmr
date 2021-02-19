using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	//Class random monster
	[Serializable]
	public class RegionData{
		public string regionName;
		public int maxAmountEnemies = 3;
		public List<GameObject> possibleEnemy = new List<GameObject> (); 
	}

	public List<RegionData> regions = new List<RegionData>();
	public List<GameObject> enemiesToBattle = new List<GameObject>();

    BattleLoader battleLoader;
    public bool bossBattle = false;
    public bool finalBossBattle = false;

    public enum GameStates
	{
		WORLD_STATE,
		TOWN_STATE,
		BATTLE_STATE,
		IDLE

	}

	public GameStates gameState;

	//bools

	public bool isWalking = false;
	public bool canGetEncounter = true;
	public bool attacked = false;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
      

	}

	void Update(){
		switch (gameState) 
		{
		case GameStates.WORLD_STATE:
			if (isWalking) {
				RandomEncounter ();
			}
                if (attacked)
                {
                    gameState = GameStates.BATTLE_STATE;
                    for(int i = 0; i<ItemsList.items.Count; ++i)
                    {
                        ItemsList.items[i].instantiated = false;
                    }
                }
			break;

		case GameStates.TOWN_STATE:
			break;
		case GameStates.BATTLE_STATE:
			//Load battle Scene
			StartBattle ();
			gameState = GameStates.IDLE;
			//Go to Idle
			break;
		case GameStates.IDLE:
			break;
		}
	}

	void RandomEncounter(){
		if (isWalking && canGetEncounter) {
			if (UnityEngine.Random.Range (0, 1000) < 5) {
                #if UNITY_EDITOR
                    Debug.Log ("Me pegaan");
                #endif
                attacked = true;
			}
		}
	}

	public void StartBossBattle(GameObject boss, bool isBoss = false, bool isFinalBoss = false){
		enemiesToBattle.Add (boss);
        bossBattle = isBoss;
        finalBossBattle = isFinalBoss;
        InitBattle ();
	}

	void StartBattle(){
		//amount of enemies
		int enemyAmount = UnityEngine.Random.Range(1, regions[0].maxAmountEnemies+1);
		for (int i = 0; i < enemyAmount; ++i) {
			enemiesToBattle.Add(regions[0].possibleEnemy[UnityEngine.Random.Range(0, regions[0].possibleEnemy.Count)]);
		}

		InitBattle ();
    }

	void InitBattle(){
		isWalking = false;
		attacked = false;
		canGetEncounter = false;
		battleLoader = GameObject.FindWithTag("GameController").GetComponent<BattleLoader>();
		battleLoader.LoadBattleScene();
	}

    // Only debug test battle
   public void StartDebugBattle(GameObject[] enemyList)
    {
        int enemyAmount = enemyList.Length;
        for (int i = 0; i < enemyAmount; ++i)
        {
            enemiesToBattle.Add(enemyList[i]);
        }

        InitBattle();
    }
}
