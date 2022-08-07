using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AngelsSetTextup : MonoBehaviour
{
    int framewait = 20;
    void OnEnable()
    {
        var starscount = transform.Find("AngelsCount");
        if (starscount == null) return;

        var angelMoney = KittyFund.GetAngelMoney();

        TMP_Text text = starscount.GetComponent<TMP_Text>();
        text.SetText(angelMoney.ToString());
                
        var starscountbright = GameObject.Find("AngelsCountBright");
        TMP_Text text2 = starscountbright.GetComponent<TMP_Text>();
        text2.SetText(angelMoney.ToString());
    }

    void Update()
    {
        
        framewait--;
        if (framewait > 0) return;
        framewait = 20;
        
        var starscount = transform.Find("AngelsCount");
        if (starscount == null) return;
        
        int currentangelmoney = KittyFund.GetAngelMoney();
        
        TMP_Text text = starscount.GetComponent<TMP_Text>();
        text.SetText(currentangelmoney.ToString());
                
        var starscountbright = GameObject.Find("AngelsCountBright");
        TMP_Text text2 = starscountbright.GetComponent<TMP_Text>();
        text2.SetText(currentangelmoney.ToString());
        
        
    }
}
