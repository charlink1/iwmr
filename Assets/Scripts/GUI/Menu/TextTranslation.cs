using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartLocalization;

public class TextTranslation : MonoBehaviour {

   [System.Serializable]
   protected class FieldText
    {
        public Text fieldText;
        public string textKey;
    }

    [SerializeField]
    protected List<FieldText> m_FieldTexts = new List<FieldText>();

   protected  LanguageManager m_LanguageManagerInstance;

    void Awake()
    {
        m_LanguageManagerInstance = LanguageManager.Instance;
        LanguageManager.SetDontDestroyOnLoad();
    }

    void Start()
    {
        TranslateText();
    }
    protected void TranslateText()
    {
        for (int i = 0; i < m_FieldTexts.Count; ++i)
        {
            m_FieldTexts[i].fieldText.text = m_LanguageManagerInstance.GetTextValue(m_FieldTexts[i].textKey);
        }

    }
}
