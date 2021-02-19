using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePaperFromDesktop : Collectible {

    public override void ObtainItem()
    {
        base.ObtainItem();
        Game.paperFromDesktopObtained = true;
    }
}
