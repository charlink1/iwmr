using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {

    private static FloatingText popupText;
    private static GameObject canvas;

    public static void Initialize(FloatingText floatingText)
    {
        canvas = GameObject.FindWithTag("Canvas");
        if (!popupText)
          popupText = floatingText;        
    }

    public static void CreateFloatingText(string text, Transform location)
    {
        FloatingText instance = CreateText(location);
        instance.SetText(text);
    }

    public static void CreateFloatingText(string text, Transform location, Color color)
    {
        FloatingText instance = CreateText(location);
        instance.SetText(text, color);
    }

    private static FloatingText CreateText(Transform location)
    {
        FloatingText instance = Instantiate(popupText, canvas.transform, false);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x, location.position.y + 0.5f));
        instance.transform.position = screenPosition;
        return instance;
    }
}
