using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CakesSetTextup : MonoBehaviour
{
    int framewait = 20;
    void OnEnable()
    {
        var cakeCount = transform.Find("CakeCount");
        if (cakeCount == null) return;

        var cakeMoney = KittyFund.GetCakeMoney();

        TMP_Text text = cakeCount.GetComponent<TMP_Text>();
        text.SetText(cakeMoney.ToString());
                
        var cakeCountBright = GameObject.Find("CakeCountBright");
        TMP_Text text2 = cakeCountBright.GetComponent<TMP_Text>();
        text2.SetText(cakeMoney.ToString());
    }

    void Update()
    {
        
        framewait--;
        if (framewait > 0) return;
        framewait = 20;
        
        var cakecount = transform.Find("CakeCount");
        if (cakecount == null) return;
        
        int cakeMoney = KittyFund.GetCakeMoney();
        
        TMP_Text text = cakecount.GetComponent<TMP_Text>();
        text.SetText(cakeMoney.ToString());
                
        var cakescountbright = GameObject.Find("CakeCountBright");
        TMP_Text text2 = cakescountbright.GetComponent<TMP_Text>();
        text2.SetText(cakeMoney.ToString());
        
        
    }
}
