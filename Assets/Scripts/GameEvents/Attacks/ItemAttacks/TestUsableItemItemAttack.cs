using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUsableItemItemAttack : BaseItemAttack {
    [SerializeField]
    Color textColor = Color.green;

	public override IEnumerator ExecuteEffect(GameObject target, Vector3 startPosition, float characterSpeed, BaseBattleStateMachine characterStateMachine)
	{
		#pragma warning disable 0219       
        GameObject attackAnimEffect = null;
        if (animationPrefab != null)
        	attackAnimEffect = Instantiate(animationPrefab, target.transform, false);
		#pragma warning restore 0219 

        yield return new WaitForSeconds(WaitUntilFinished(characterStateMachine, target));

        BaseBattleStateMachine targetStateMachine = target.GetComponent<BaseBattleStateMachine>();
        if (targetStateMachine != null)
        {
            FloatingTextController.CreateFloatingText(item.value.ToString(), target.transform, textColor);
            item.UseItem(targetStateMachine.character);
            if (targetStateMachine is CharacterStateMachine)
                targetStateMachine.UpdateHeroPanel();
        }
        #if UNITY_EDITOR
                Debug.Log("Used in battle");
        #endif
        yield return null;


	}
}
