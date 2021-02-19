using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SmartLocalization;

public class LanguageLoader : TextTranslation {
    [System.Serializable]
    class LanguageCode
    {
#pragma warning disable 0649  // variable declared but not used.
        public string name;
        public string languageCode;
#pragma warning restore 0649  // variable declared but not used.
    }

    [SerializeField]
    List<LanguageCode> m_Languages = new List<LanguageCode>();
   
    void Start()
    {
        m_LanguageManagerInstance.OnChangeLanguage += OnChangeLanguage;
        // Debug.Log(Application.systemLanguage.ToString());
        LanguageCode currentSystemLanguage = m_Languages.Find(p => p.name.Equals(Application.systemLanguage.ToString()));
        if(currentSystemLanguage!= null)
            ChangeLanguage(currentSystemLanguage.languageCode);
    }

    void OnChangeLanguage(LanguageManager thisLanguageManager)
    {
        TranslateText();

    }

    void OnDestroy()
    {
        m_LanguageManagerInstance.OnChangeLanguage -= OnChangeLanguage;
    }

    public void ChangeLanguage(string languageCode)
    {
        if (languageCode != null && m_LanguageManagerInstance.IsCultureSupported(languageCode))
        {
            m_LanguageManagerInstance.ChangeLanguage(languageCode);
        }
      //  Debug.Log(m_LanguageManagerInstance.CurrentlyLoadedCulture.englishName);
    }

   
}
