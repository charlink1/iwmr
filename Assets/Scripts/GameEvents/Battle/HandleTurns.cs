﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HandleTurn  {

	public string Attacker;
    public string Type;
	public GameObject AttacksGameObject;
	public GameObject AttackersTarget;


    //which attack is performed

    public BaseAttack choosenAttack;
}
