using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class BattleLoader : MonoBehaviour {

    Animator fadigAnimator;
    Animator lightningAnimator;
  
	GameObject fadingImageGameObject;
    PlayerController2D playerController;
    [SerializeField]
    Transform chestTransform;

    void Awake()
    {
        if(!Game.initialized)
            Game.Init();
    }

    IEnumerator Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController2D>();
        lightningAnimator = GameObject.FindWithTag("LightningImage").GetComponent<Animator>();
        yield return new WaitForEndOfFrame();
        Game.returningFromBattle = false;

		fadingImageGameObject = GameObject.FindWithTag ("FadeImage");
        if (Game.isFading)
        {
			fadigAnimator = fadingImageGameObject.GetComponent<Animator>();
            fadigAnimator.enabled = true;
            fadigAnimator.SetTrigger("fadeOff");
            Game.isFading = false;
		}else
			fadingImageGameObject.SetActive (false);
    }

	//void Update () {
 //       if (Input.GetKeyDown(KeyCode.B))
 //           LoadBattleScene();
 //   }

   public void LoadBattleScene()
    {
        playerController.canMove = false;
        playerController.StopPlayerMove();
        Game.chestPosition = chestTransform.position;
        foreach (PlayableCharacter character in CharacterParty.charactersParty)
        {
            GameObjectData data = new GameObjectData();           
            character.orderInLayer = character.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            Transform objTransform = null;
            data.objName = character.CharGameObject.name;
            objTransform = character.CharGameObject.transform;
            data.position = objTransform.position;
            data.rotation = objTransform.rotation;
            data.localScale = objTransform.localScale;
            Game.AddObjectStatus(data);
        }

        lightningAnimator.enabled = true;
        Game.timePlayed += Time.timeSinceLevelLoad;
    }

	IEnumerator InitBossBattle(GameObject boss)
	{
		yield return null;
		GameManager.instance.StartBossBattle (boss);
	}
    
    
}
