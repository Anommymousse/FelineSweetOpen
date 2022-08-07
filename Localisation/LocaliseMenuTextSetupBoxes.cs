using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocaliseMenuTextSetupBoxes : MonoBehaviour
{
    ConvertLanguage _convertLanguageRef;
    
    void Awake()
    {
        var go = GameObject.Find("[BOOTSTRAP]");
        _convertLanguageRef = go.GetComponent<ConvertLanguage>();

        if (go.name == "Resume")
        {
            Debug.Log($"<color=red> GO RESUME AWAKE!</color>");
        }
    }

    IEnumerator ChangeTextBox()
    {
        yield return null;
        var setupBox = this.gameObject.GetComponent<SetupMenuBox>();
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
