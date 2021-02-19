using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAttackButton : MonoBehaviour {

    public BaseItemAttack itemToUse;

    public void CastItemUse()
    {
        GameObject.FindWithTag("BattleManager").GetComponent<BattleController>().ItemInput(itemToUse);
    }
}
