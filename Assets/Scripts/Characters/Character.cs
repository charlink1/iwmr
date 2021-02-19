using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public CharacterStats charStats;
    public Equipment currentEquipment;
    public GameObject prefab;

    public Vector2 attackPositionOffset = new Vector2 (0, 0);
    public GameObject targetAttack;

    public List<BaseAttack> attacksList = new List<BaseAttack> ();
}
