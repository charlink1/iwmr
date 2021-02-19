using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockoTrigger : MonoBehaviour {

    [SerializeField]
    GameObject sockoPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !GameManager.instance.attacked && Game.sockoAppears)
        {
            GameManager.instance.StartBossBattle(sockoPrefab, true);
        }
    }
}
