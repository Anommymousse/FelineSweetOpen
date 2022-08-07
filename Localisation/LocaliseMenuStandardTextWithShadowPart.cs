using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocaliseMenuStandardTextWithShadowPart : MonoBehaviour
{
    ConvertLanguage _convertLanguageRef;
    
    void Awake()
    {
        var go = GameObject.Find("[BOOTSTRAP]");
        _convertLanguageRef = go.GetComponent<ConvertLanguage>();
    }

    string RemoveTextAnimatorsParseStuff(string stingwithparses)
    {
        int thing = stingwithparses.LastIndexOf(">");
        int length = stingwithparses.Length;
        string parsed = stingwithparses.Substring(thing+1);
        return parsed;
    }

    IEnumerator ChangeTextBox()
    {
        yield return null;
        var firstTMPText = gameObject.GetComponent<TMP_Text>();
        if (firstTMPText != null)
        {
            var lowercase = firstTMPText.text.ToLower();
            var tmpTextsToChange = gameObject.GetComponentsInChildren<TMP_Text>();

            lowercase = RemoveTextAnimatorsParseStuff(lowercase);

            string returnedConvertedString = _convertLanguageRef.ConvertString(lowercase);
            foreach (var text in tmpTextsToChange)
            {
                text.SetText(returnedConvertedString);
                text.font = _convertLanguageRef.GetCurrentFont();
            }
        }
        else
        {
            Debug.Log($"{gameObject.name} doesn't seem to have TMP_Text");
        }

    }

    void OnEnable()
    {
        if(_convertLanguageRef.GetCurrentLanguage()==ConvertLanguage.Languages.Chinese)
            StartCoroutine(ChangeTextBox());
    }
}
