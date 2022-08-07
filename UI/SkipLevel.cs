using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipLevel : MonoBehaviour
{
    int framedelay = 60;
    public Button buttonRef;
    public void NextLevel()
    {
        var GO = GameObject.Find("LevelManager");
        var levelloader = GO.GetComponent<LevelLoader>();
        levelloader.LoadNextLevel(false);

        int skipmoney = KittyFund.GetSkipMoney();
        if (skipmoney > 0)
        {
            skipmoney--;
            KittyFund.SetSkipMoney(skipmoney);
        }
    }

    void Update()
    {
        framedelay--;
        if (framedelay >= 0) return;
        
        framedelay = 60;
        var skipMoney = KittyFund.GetSkipMoney();
        buttonRef.interactable = skipMoney > 0;
    }
}
