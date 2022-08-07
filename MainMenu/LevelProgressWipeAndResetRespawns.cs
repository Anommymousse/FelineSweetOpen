using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressWipeAndResetRespawns : MonoBehaviour
{

    


    public void ResetAllKeys()
    {
        ES3.Save("UsernameLevel"+"Easy",0);
        ES3.Save("UsernameLevel"+"Normal",0);
        ES3.Save("UsernameLevel"+"Hard",0);
        
       // PlayerPrefs
       PlayerPrefs.SetInt("PlayerRespawnCount",0);
       MainMenuFullWipeOfPPLevels.WipeAllStarAndCakeProgress();
       PlayerPrefs.DeleteKey("AngelMoney");
       PlayerPrefs.DeleteKey("CakeMoney");
       PlayerPrefs.DeleteKey("SkipMoney");
       KittyFund.SetCakeMoney(0);
       KittyFund.SetSkipMoney(0);
       KittyFund.SetAngelMoney(0);

    }
}
