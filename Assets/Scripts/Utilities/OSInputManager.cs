using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



public class OSInputManager : MonoBehaviour {
	[Serializable]
	public class InputList
	{
		public string buttonName;
		public string WindowsPad;
		public string MacPad;
	}

    public List<InputList> buttonsOrAxis = new List<InputList>();

    private static Dictionary<string, string> dictionary = new Dictionary<string, string>();


	void Awake () {
		for (int i = 0; i < buttonsOrAxis.Count; ++i)
		{
            if (!dictionary.ContainsKey(buttonsOrAxis[i].buttonName))
            { 
			    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
						    dictionary.Add(buttonsOrAxis[i].buttonName, buttonsOrAxis[i].WindowsPad);
                #endif
                #if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
			        	    dictionary.Add(buttonsOrAxis[i].buttonName, buttonsOrAxis[i].MacPad);
                #endif
            }
        }
	}
		
	public static string GetPadMapping(string buttonName)
	{
		if (dictionary.ContainsKey(buttonName))
			return dictionary[buttonName];
		else
			Debug.Log("Button "+ buttonName +"  not defined");
		return null;
	}

	
}
	

