using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartLocalization;

public class LanguageButton : MonoBehaviour {

    [SerializeField]
    Text[] m_TextField; 

    LanguageManager m_LanguageManagerInstance;

    void Start()
    {
        m_LanguageManagerInstance = LanguageManager.Instance;
      
    }

   public void ChangeLanguage(string languageCode)
    {
        m_LanguageManagerInstance.ChangeLanguage(languageCode);
    }

   
}
