using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour {

    void OnDestroy()
    {
        Game.finalBossDefeated = true;
    }
}
