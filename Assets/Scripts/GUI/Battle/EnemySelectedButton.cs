using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectedButton : CharacterSelected
{

    public void SelectedEnemy()
    {
        GameObject.FindWithTag("BattleManager").GetComponent<BattleController>().SelectTarget(characterPrefab);

    }


}
