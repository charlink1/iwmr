using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison1Spell : BaseMagicAttack
{

	public Poison1Spell()
    {
        attackName = "Poison";
        attackDescription = "Poison spell";
        attackDamage = 5;
        attackCost = 5;
    }
}
