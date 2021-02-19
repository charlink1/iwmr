using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsBehavior : MonoBehaviour {

	[SerializeField]
	Animator[] windowAnimators;

    [SerializeField]
    Animator[] insideWindowAnimators;


    [SerializeField]
	int defaultColor = 4;

	 void Start(){
		if (Game.bgWindowColor != -1)
			ChangeWindowColor (Game.bgWindowColor);
		else
			Game.bgWindowColor = defaultColor;
	}

	public void ChangeWindowColor(int colorNum){

        for (int i = 0; i < windowAnimators.Length; ++i)
        {
            windowAnimators[i].SetFloat("indexColor", colorNum);
            windowAnimators[i].SetTrigger("changeWindowColor");

        }
        Game.bgWindowColor = colorNum;
    }

    public void OpenWindows()
    {
        for (int i = 0; i < insideWindowAnimators.Length; ++i)
        {
            insideWindowAnimators[i].SetTrigger("openWindow");
            insideWindowAnimators[i].SetTrigger("openWindow");

        }
    }
}
