using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SmartLocalization;

public enum AttackEffect{Healing, Damage}

[Serializable]
public class BaseAttack : MonoBehaviour
{

	public AttackEffect attackEffect;
    public string attackName;
    public string attackDescription;
    public int attackDamage; //base damage
    public int attackCost; //if it uses MP
    [SerializeField]
    protected GameObject animationPrefab;

    LanguageManager m_LanguageManagerInstance;
    Character character;

   void OnEnable()
    {
        m_LanguageManagerInstance = LanguageManager.Instance;
        attackName = m_LanguageManagerInstance.GetTextValue(attackName);
        attackDescription = m_LanguageManagerInstance.GetTextValue(attackDescription);
    }

    public virtual IEnumerator ExecuteEffect(GameObject target, Vector3 startPosition, float characterSpeed, BaseBattleStateMachine characterStateMachine)
    {
       
            if (characterStateMachine.character != null)
                character = characterStateMachine.character;
            else
                character = characterStateMachine.GetComponent<Character>();
        
                        
        if (target.GetComponent<CharacterStateMachine>() == null || characterStateMachine is EnemyStateMachine)
        {
            Character targetChar = target.GetComponent<Character>();
            // Vector2 modifier = targetChar.minAttackerDistance;
            //if (characterStateMachine is EnemyStateMachine)
            //    modifier.x = -modifier.x;

            //  Vector3 enemyPosition = new Vector3(target.transform.position.x + modifier.x, target.transform.position.y + modifier.y, target.transform.position.z);

            Vector3 targetPosition = targetChar.targetAttack.transform.position;
            Vector3 enemyPosition = new Vector3(targetPosition.x + character.attackPositionOffset.x, targetPosition.y + character.attackPositionOffset.y, targetPosition.z);


            while (MoveTowardsEnemy(enemyPosition, characterStateMachine.gameObject, characterSpeed))
            {
                yield return null;
            }

            Vector3 damagePosition = Vector3.zero;
            GameObject obj = Helper.FindChildWithTag(characterStateMachine.transform, "DamagePosition");
            
            // En el caso de tener posición donde recibir el daño se utilizará esta para mostrar la animación y el texto del daño
            if (obj != null)
                damagePosition = obj.transform.position;

            
            //Esperar
            yield return new WaitForSeconds(WaitUntilFinished(characterStateMachine, target, damagePosition));
            //Aplicar efecto
            characterStateMachine.DoDamage(damagePosition);

            //Mover a posicion inicial
            // Vector3 firstPosition = startPosition;
            while (MoveTowardsEnemy(startPosition, characterStateMachine.gameObject, characterSpeed))
            {
                yield return null;
            }
        }
        else
        {
            if(target!= null && characterStateMachine is CharacterStateMachine)
                yield return new WaitForSeconds(WaitUntilFinished(characterStateMachine, target));

           else
                yield return new WaitForSeconds(0.5f);
            //Aplicar efecto
            characterStateMachine.DoDamage();
        }
    }

    public bool MoveTowardsEnemy(Vector3 target, GameObject attacker, float animSpeed)
    {
        return target != (attacker.transform.position = Vector3.MoveTowards(attacker.transform.position, target, animSpeed * Time.deltaTime));
    }

    //Retorna tiempo de espera hasta finalizar el ataque
    protected float WaitUntilFinished(BaseBattleStateMachine characterStateMachine, GameObject target, Vector3 damagePosition)
    {
        if (animationPrefab != null)
        {
            GameObject attackAnimEffect = Instantiate(animationPrefab, target.transform, false);
            // Si el enemigo es muy grande (se incluye la posición del daño) la animación no aparece en el centro. 
            if (damagePosition != Vector3.zero)
            {
                attackAnimEffect.transform.localPosition = damagePosition;
                attackAnimEffect.transform.localScale = attackAnimEffect.transform.localScale * 2;
            }
                

            if (characterStateMachine is CharacterStateMachine)
                attackAnimEffect.GetComponent<SpriteRenderer>().flipX = true;

            return attackAnimEffect.GetComponent<AttackAnimation>().GetAnimationLength();
           
        }
        else
            return 0.5f;
    }

    protected float WaitUntilFinished(BaseBattleStateMachine characterStateMachine, GameObject target)
    {
        return WaitUntilFinished(characterStateMachine, target, Vector3.zero);
    }

}
