using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    GameObject paperFromTrashPrefab;
    [SerializeField]
    GameObject paperFromDesktopPrefab;
    [SerializeField]
    GameObject chest;
    [SerializeField]
    GameObject estanteria;
    [SerializeField]
    MonsterInWardrobe monsterInWardrobe;
    [SerializeField]
    Sequencer sequencer;

    BoyInitializer boyInitializer;
    WindowsBehavior windowsBehavior;
   


    void Awake()
    {
        boyInitializer = GetComponent<BoyInitializer>();
        windowsBehavior = GetComponent<WindowsBehavior>();
        if (!Game.paperFromTrashObtained)
            Instantiate(paperFromTrashPrefab);

        if (!Game.paperFromDesktopObtained)
            Instantiate(paperFromDesktopPrefab);

        if(Game.sockoAppears && !Game.sockoDefeated)
        {
            monsterInWardrobe.StartCoroutine("ActivateSocko");

        }else if (Game.sockoAppears && Game.sockoDefeated)
        {
            Game.sockoAppears = false;
            Game.puzzlesSolved++;

            CheckPuzzlesSolved();
        }

        if (Game.finalBossDefeated)
        {
            DisableColliders(chest);
            DisableColliders(estanteria);
            sequencer.StartCoroutine("StartSequence","finalSequence");
        }
    }

    void DisableColliders(GameObject obj)
    {
        Collider2D[] colliders = obj.GetComponents<Collider2D>();
        for(int i = 0; i< colliders.Length; ++i)
        {
            colliders[i].enabled = false;
        }
    }

    public IEnumerator InitBoy()
    {
        boyInitializer.InitBoy();
        yield return null;
    }

    public void CheckPuzzlesSolved()
    {
        if(Game.puzzlesSolved == 2)
        {
            //Socko aparece
            Game.sockoAppears = true;
            if (!Game.sockoDefeated)
            {
                monsterInWardrobe.StartCoroutine("ActivateSocko");
            }
           
            
        }
        if (Game.puzzlesSolved >0 && Game.puzzlesSolved < 6)
        {
            windowsBehavior.ChangeWindowColor(Game.puzzlesSolved);
        }
    }

    public IEnumerator StartFinalBossBattle(GameObject bossPrefab)
    {
        yield return null;
        sequencer.PerformNextSequenceStep();
        GameManager.instance.StartBossBattle(bossPrefab, false, true);

    }

    public IEnumerator DeactivateGameObject(GameObject obj)
    {
        yield return null;
        obj.SetActive(false);
    }
 
}
