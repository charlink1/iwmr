using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelected : MonoBehaviour {

    public GameObject characterPrefab;
    GameObject selector;

    void Start()
    {
        selector = characterPrefab.transform.FindChild("Selector").gameObject;
    }

    public void SelectedCharacter()
    {
        GameObject.FindWithTag("BattleManager").GetComponent<BattleController>().SelectTarget(characterPrefab);

    }

    public void Select(bool select)
    {
        selector.SetActive(select);
    }
}
