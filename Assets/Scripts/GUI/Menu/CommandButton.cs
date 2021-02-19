using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButton : DefaultMenuButton {

    
    BattleController battleController;

    protected override void Start () {
        base.Start();
        if (parent == null)
            parent = transform.parent.parent.GetComponent<Menu>();
        battleController = parent.GetBattleController();

    }

   

    
}
