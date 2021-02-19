using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemAttack : BaseAttack {

   public UsableItem item
    {
        get; set;
    }
    void Start()
    {
        item = GetComponent<UsableItem>();
        attackName = item.itemName;
    }
    public override IEnumerator ExecuteEffect(GameObject target, Vector3 startPosition, float characterSpeed, BaseBattleStateMachine characterStateMachine)
    {
        #if UNITY_EDITOR
                Debug.Log("Used in battle");
        #endif
        yield return null;
       
    }
}
