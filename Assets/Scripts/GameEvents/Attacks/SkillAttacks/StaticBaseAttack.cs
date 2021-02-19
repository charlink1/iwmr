using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StaticBaseAttack : BaseAttack {
    Animator characterAnimator;
    public StaticBaseAttack()
    {
        attackName = "Static Attack";
        attackDamage = 15;
        attackCost = 0;
    }

    public override IEnumerator ExecuteEffect(GameObject target, Vector3 startPosition, float characterSpeed, BaseBattleStateMachine characterStateMachine)
    {
        // Si el character animator es null
        if (characterAnimator == null)
            characterAnimator = characterStateMachine.gameObject.GetComponent<Animator>();

        if (target.GetComponent<CharacterStateMachine>() != null && characterAnimator != null && !characterAnimator.GetBool("attack"))
        {
            characterAnimator.SetBool("attack", true);
            // Hay que asegurarse de que el animator no está dentro de una transición
            while (characterAnimator.GetCurrentAnimatorClipInfo(0).Length == 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(WaitUntilFinished(characterAnimator));
            yield return new WaitForSeconds(WaitUntilFinished(characterStateMachine, target));

            //Aplicar efecto
            characterStateMachine.DoDamage();

            characterAnimator.SetBool("attack", false);
        }
    }

    protected float WaitUntilFinished(Animator animator)
    {
        if (animator != null)
        {
            //GameObject attackAnimEffect = Instantiate(animationPrefab, target.transform, false);

            //if (characterStateMachine is CharacterStateMachine)
            //    attackAnimEffect.GetComponent<SpriteRenderer>().flipX = true;

            //return attackAnimEffect.GetComponent<AttackAnimation>().GetAnimationLength();
          
            return animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / 2;
        }
        else
            return 0.5f;
    }
}

