using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartLocalization;

public class TextInitializer : MonoBehaviour {
   
    [SerializeField]
    string textKey;

    LanguageManager languageManager;
	void Start () {
        languageManager = LanguageManager.Instance;   
        GetComponent<Text>().text = languageManager.GetTextValue(textKey);
	}
	
	
}
