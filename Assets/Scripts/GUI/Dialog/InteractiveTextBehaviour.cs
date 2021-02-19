using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveTextBehaviour : MonoBehaviour {

    bool axisUp;
    bool isSelected;

    Text textField;

    float value; 
    float timer = 0;

    bool usingDpad = false;

    void Start()
    {
        textField = GetComponent<Text>();
        textField.text = "0";
        value = int.Parse(textField.text);

    }

    void OnEnable()
    {
        if (textField != null)
        {
            textField.text = "0";
            value = int.Parse(textField.text);
        }
    }

    void OnDisable()
    {
        if (isSelected)
            SetSelected(false);
    }

    void Update () {
        if (isSelected)
        {
            float inputY = 0;
            if(!usingDpad)
                inputY = Input.GetAxisRaw("Vertical");

            if(inputY == 0f)
            {
                inputY = Input.GetAxis(OSInputManager.GetPadMapping("DPadVertical"));
                if (!usingDpad)
                    usingDpad = true;
                if (inputY == 0)
                    usingDpad = false;
            }
           // Debug.Log(inputY);
            if ((int)inputY == 1)
            {
                //       value += 1 * Time.deltaTime;              
                textField.text = CalculateValue(1).ToString();
            }

           else if ((int)inputY == -1)
            {
                //     value -= 1 * Time.deltaTime;
                textField.text = CalculateValue(-1).ToString();
            }
            else if((int)inputY == 0)
            {
                axisUp = true;
            }
        }
    }

    //Calcula el valor del siguente número, incluso si se deja la tecla pulsada
    int CalculateValue(int inputValue)
    {
        if(axisUp)
        {           
            axisUp = false;
            value += inputValue;
            timer = 0;
        }
        else
        {
            if ((timer += Time.deltaTime) >= 1)
            {
                timer = 0;
                value += inputValue;
            }
        }

        if (value > 9)
            value = 0;
        if (value < 0)
            value = 9;
            
        return (int)value;
      
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }
}
