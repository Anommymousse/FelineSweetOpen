using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkipsSetTextup : MonoBehaviour
{
    int _framewait = 20;

    void UpdateSkipMoney()
    {
        var skipscount = transform.Find("SkipsCount");
        if (skipscount == null) return;

        var skipMoney = KittyFund.GetSkipMoney();

        TMP_Text text = skipscount.GetComponent<TMP_Text>();
        text.SetText(skipMoney.ToString());
        text.ForceMeshUpdate();
                
        var starscountbright = GameObject.Find("SkipsCountBright");
        TMP_Text text2 = starscountbright.GetComponent<TMP_Text>();
        text2.SetText(skipMoney.ToString());
        text2.ForceMeshUpdate();
    }
    
    void OnEnable()
    {
        UpdateSkipMoney();
    }

    void Update()
    {
        _framewait--;
        if (_framewait > 0) return;
        _framewait = 21;
        
        UpdateSkipMoney();
    }
}
