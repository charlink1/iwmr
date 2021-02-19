using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper {

	public static GameObject FindChildWithTag(Transform transform, string tagName)
	{
		for(int i = 0; i< transform.childCount; ++i)
		{
			if (transform.GetChild(i).CompareTag(tagName))
				return transform.GetChild(i).gameObject;
		}
		return null;
	}
}
