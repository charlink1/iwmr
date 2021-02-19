using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBattleStateMachine : MonoBehaviour {

    public CharacterStats charStats
    {
        get; set;
    }
    public Character character;
   
    public GameObject characterToAttack;
    protected BattleController battleController;


    public virtual void TakeDamage(int damage)
    {
        TakeDamage(damage, Vector3.zero);
    }

    public virtual void TakeDamage(int damage, Vector3 damagePosition)
    {
        int totalDamage = Mathf.Clamp(damage - charStats.totalDefense, 0, damage);
        Transform trans = transform;

        // Si se especifica una posición en concreto, que el texto aparezca ahí (o un poco más arriba)
        if (damagePosition != Vector3.zero)
            trans.position = damagePosition;

        FloatingTextController.CreateFloatingText(totalDamage.ToString(), trans);
        charStats.currentHealthPoints -= totalDamage;
    }

    public virtual void UpdateHeroPanel() {}

    public void DoDamage(bool magic = false)
    {
        int damage= 0;
        if(!magic)
            damage = character.charStats.totalAttack + battleController.performList[0].choosenAttack.attackDamage;
        else
            damage = character.charStats.totalMagic + battleController.performList[0].choosenAttack.attackDamage;

        characterToAttack.GetComponent<BaseBattleStateMachine>().TakeDamage(damage);
    }

    public void DoDamage(Vector3 damagePosition, bool magic = false)
    {
        int damage = 0;
        if (!magic)
            damage = character.charStats.totalAttack + battleController.performList[0].choosenAttack.attackDamage;
        else
            damage = character.charStats.totalMagic + battleController.performList[0].choosenAttack.attackDamage;

        characterToAttack.GetComponent<BaseBattleStateMachine>().TakeDamage(damage);
    }

}
