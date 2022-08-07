using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocaliseMenuTextSetupSingleTextBoxes : MonoBehaviour
{
    ConvertLanguage _convertLanguageRef;
    
    void Awake()
    {
        var go = GameObject.Find("[BOOTSTRAP]");
        _convertLanguageRef = go.GetComponent<ConvertLanguage>();
    }
    
    
    IEnumerator ChangeTextBox()
    {
        yield return null;
        yield return new WaitForSeconds(0.05f);
        var setupBox = this.gameObject.GetComponent<SetupSingleTextBox>();
        if (setupBox != null)
        {
            var lowercase = setupBox._menuText.ToLower();
            string returnedConvertedString = _convertLanguageRef.ConvertString(lowercase);
            setupBox._menuText = returnedConvertedString;
            setupBox.UpdateText();
            var tmpTexts = setupBox.GetComponentsInChildren<TMP_Text>();

            tmpTexts[0].font = _convertLanguageRef.GetCurrentFont();
            tmpTexts[1].font = _convertLanguageRef.GetCurrentFont();
            if(_convertLanguageRef.GetCurrentLanguage()!= ConvertLanguage.Languages.English)
                setupBox.SetWidth(_convertLanguageRef.GetCurrentWidth());
            setupBox.UpdateText();
        }
    }

    void OnEnable()
    {
        if(_convertLanguageRef.GetCurrentLanguage()==ConvertLanguage.Languages.Chinese)
        StartCoroutine(ChangeTextBox());
    }
}
