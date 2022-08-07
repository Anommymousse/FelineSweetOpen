using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePowerCountFromPrefs : MonoBehaviour
{
    int _defaultPowerCount = 0;
    string powerID = "PowerCount";
    

    public int GetPowerCount()
    {
        int powerCount = PlayerPrefs.GetInt(powerID, _defaultPowerCount);
        if (powerCount < 0) powerCount = 0;
        return powerCount;
    }

    public void SetPowerCount(int newPowerCount)
    {
        PlayerPrefs.SetInt(powerID, newPowerCount);
    }
        

}
