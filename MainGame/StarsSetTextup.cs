using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarsSetTextup : MonoBehaviour
{
    int previousKittyfund=0;
    int framewait = 20;
    void OnEnable()
    {
        var starscount = transform.Find("StarsCount");
        if (starscount == null) return;
        
        var starCount = UpdatingKittyFund.GetCurrentKittyFund();

        TMP_Text text = starscount.GetComponent<TMP_Text>();
        text.SetText(starCount.ToString());
                
        var starscountbright = GameObject.Find("StarsCountBright");
        TMP_Text text2 = starscountbright.GetComponent<TMP_Text>();
        text2.SetText(starCount.ToString());
    }
    
    void Update()
    {
        framewait--;
        if (framewait > 0) return;
        framewait = 20;
        
        var starscount = transform.Find("StarsCount");
        if (starscount == null) return;
        
        int currentKittyFund = UpdatingKittyFund.GetCurrentKittyFund();
        if (previousKittyfund == currentKittyFund) return;
        previousKittyfund = currentKittyFund;
        
        TMP_Text text = starscount.GetComponent<TMP_Text>();
        text.SetText(currentKittyFund.ToString());
                
        var starscountbright = GameObject.Find("StarsCountBright");
        TMP_Text text2 = starscountbright.GetComponent<TMP_Text>();
        if (currentKittyFund > 8)
        {
            //Rainbow mode
            text2.SetText($"<rainb>{currentKittyFund}</rainb>");
        }
        else
            text2.SetText($"{currentKittyFund.ToString()}");

    }
}
