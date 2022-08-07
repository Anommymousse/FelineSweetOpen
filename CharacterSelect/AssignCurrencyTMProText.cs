using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssignCurrencyTMProText : MonoBehaviour
{
    [SerializeField] string keyForPrefs; 
    KittyFund theKittyFund;
    
    void Start()
    {
        UpdateText();
        theKittyFund = FindObjectOfType<KittyFund>();
        theKittyFund.OnMoneyUpdated += TheKittyFundOnOnMoneyUpdated;
    }

    void OnDestroy()
    {
        if(theKittyFund!=null)
        theKittyFund.OnMoneyUpdated -= TheKittyFundOnOnMoneyUpdated;
    }

    void TheKittyFundOnOnMoneyUpdated()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        int currencyValue = PlayerPrefs.GetInt(keyForPrefs, 0);

        var _TMPtextArray = GetComponentsInChildren<TMP_Text>();
        if (_TMPtextArray != null)
        {
            if (_TMPtextArray.Length == 2)
            {
                var text_index0 = _TMPtextArray[0].GetComponentsInChildren<TMP_Text>();
                var text_index1 = _TMPtextArray[1].GetComponentsInChildren<TMP_Text>();
                string currencyAsString = String.Format($"{currencyValue}");
                _TMPtextArray[0].SetText(currencyAsString);
                _TMPtextArray[1].SetText(currencyAsString);
            }
        }
    }
}
