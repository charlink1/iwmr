using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEvents : MonoBehaviour {
    [SerializeField]
    GameController gc;
    [SerializeField]
    GameObject finalBoss;
    [SerializeField]
    GameObject[] enemiesList;

    [SerializeField]
    Item testItem;

    [SerializeField]
    Item testItem2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(gc.StartFinalBossBattle(finalBoss));
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (testItem != null)
                testItem.AddItem(1, true);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (testItem != null)
                testItem2.AddItem(1, true);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (enemiesList != null && enemiesList.Length > 0)
                GameManager.instance.StartDebugBattle(enemiesList);
                    
        }

    }
}
