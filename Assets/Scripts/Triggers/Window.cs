using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour {

	public Sprite openedWindow;
	SpriteRenderer spriteRenderer;

	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void OpenWindow(){
		spriteRenderer.sprite = openedWindow;
	}
}
