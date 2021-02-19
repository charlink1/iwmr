using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePaperFromTrash : Collectible {

    public override void ObtainItem()
    {
        base.ObtainItem();
        Game.paperFromTrashObtained = true;
    }
}
