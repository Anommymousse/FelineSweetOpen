using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUnlocks : MonoBehaviour
{

    public void AllDaMonies()
    { //Kitty
        var bootstrap = GameObject.Find("[BOOTSTRAP]");
        if (bootstrap)
        {
            var kittyFund = bootstrap.GetComponent<KittyFund>();
            kittyFund.SetMoney(1,27);
            kittyFund.SetMoney(5,27);
            kittyFund.SetMoney(8,27);
        }
    }
    
    public void UnlockAllCats()
    {
        for (int i = 0; i < 12; i++)
        {
            string key = i + "Unlocked";
            PlayerPrefs.SetInt(key, 1);
        }
    }

    public void LockAllCats()
    {
        for (int i = 0; i < 12; i++)
        {
            string key = i + "Unlocked";
            PlayerPrefs.SetInt(key, 0);
        }
    }

    public void UnlockMaxLevel()
    {
        ES3.Save("UsernameLevel" + "Normal", 203);
    }
}
