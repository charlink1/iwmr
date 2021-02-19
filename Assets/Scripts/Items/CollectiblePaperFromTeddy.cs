using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePaperFromTeddy : Collectible {

    void Start()
    {
        obtained = Game.paperFromTeddyObtained;
    }

    public override void ObtainItem()
    {
        if (!Game.paperFromTeddyObtained)
        {
            base.ObtainItem();
            Game.paperFromTeddyObtained = true;
        }
    }
}
