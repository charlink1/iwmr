using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMagicAttack : BaseAttack {

    public override IEnumerator ExecuteEffect(GameObject target, Vector3 startPosition, float characterSpeed, BaseBattleStateMachine characterStateMachine)
    {
       yield return CastMagic(target, characterStateMachine);
            characterStateMachine.character.charStats.currentMagicPoints -= attackCost;

            if (characterStateMachine is CharacterStateMachine)
                characterStateMachine.UpdateHeroPanel();

            yield return null;
      //  }

    }

    protected IEnumerator CastMagic(GameObject target, BaseBattleStateMachine stateMachine)
    {
        if(stateMachine is CharacterStateMachine)
        {
            ((CharacterStateMachine)stateMachine).animator.SetBool("usingMagic", true);
        }

        yield return new WaitForSeconds(WaitUntilFinished(stateMachine, target));

        //Aplicar efecto
        stateMachine.DoDamage(true);

        if (stateMachine is CharacterStateMachine)
        {
            ((CharacterStateMachine)stateMachine).animator.SetBool("usingMagic", false);
        }
    }

   
}
