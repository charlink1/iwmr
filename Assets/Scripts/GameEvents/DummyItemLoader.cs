using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyItemLoader : MonoBehaviour {

	public Item itemPrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I))
			itemPrefab.AddItem (1);
	}
}
