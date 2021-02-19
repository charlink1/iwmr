using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    [SerializeField]
    Animator animator;

    private Text damageTex;
   
    void OnEnable()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject,clipInfo[0].clip.length);
        damageTex = animator.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        damageTex.GetComponent<Text>().text = text; 
    }

    public void SetText(string text, Color color)
    {
        Text textField = damageTex.GetComponent<Text>();
        textField.color = color;
        textField.text = text;
    }

    
}
