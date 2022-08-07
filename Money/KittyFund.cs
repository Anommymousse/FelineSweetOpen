//#define DEMO
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MyExtensions.MyExtensions;

public class KittyFund : MonoBehaviour
{
    public event Action OnMoneyUpdated;
    string _easyMoneyPPID = "EasyMoney";
    string _mediumMoneyPPID = "MediumMoney";
    string _hardMoneyPPID = "HardMoney";
    static string _angelCollectedPPID = "AngelMoney";
    static string _skipMoneyPPID = "SkipMoney";
    static string _cakeMoneyPPID = "CakeMoney";

    int _easyMoney;
    int _mediumMoney;
    int _hardMoney;
    static int _angelMoney;
    static int _skipMoney;
    static int _cakeMoney;
    
    void Start()
    {
        _easyMoney = PlayerPrefs.GetInt(_easyMoneyPPID, 0);
        _mediumMoney = PlayerPrefs.GetInt(_mediumMoneyPPID, 0);
        _hardMoney = PlayerPrefs.GetInt(_hardMoneyPPID, 0);
        _angelMoney = PlayerPrefs.GetInt(_angelCollectedPPID, 0);
        _skipMoney = PlayerPrefs.GetInt(_skipMoneyPPID, 0);
        _cakeMoney = PlayerPrefs.GetInt(_cakeMoneyPPID, 0);

#if (DEMO)
        {
            _easyMoney = 0;
            _mediumMoney = 0;
            _hardMoney = 0;
        }
#endif
        //_easyMoney += 30;
        //_mediumMoney += 30;
        //_hardMoney += 30;
    }

    void OnEnable()
    {
        _easyMoney = PlayerPrefs.GetInt(_easyMoneyPPID, 0);
        _mediumMoney = PlayerPrefs.GetInt(_mediumMoneyPPID, 0);
        _hardMoney = PlayerPrefs.GetInt(_hardMoneyPPID, 0);
        _angelMoney = PlayerPrefs.GetInt(_angelCollectedPPID, 0);
        _skipMoney =  PlayerPrefs.GetInt(_skipMoneyPPID, 0);
        _cakeMoney =  PlayerPrefs.GetInt(_cakeMoneyPPID, 0);
    }

    public string WhichMoneyToAdd()
    {
        var GO = GameObject.Find("LevelManager");
        var levelloader = GO.GetComponent<LevelLoader>();
        var level = levelloader.GetLevelLevel();
        var levelnum = Int32.Parse(level);

        if (levelnum < 31) return _easyMoneyPPID;
        if (levelnum < 60) return _mediumMoneyPPID;
        return _hardMoneyPPID;
    }

    public void IncrementCurrentKittyPot()
    {
        var moneyPPID = WhichMoneyToAdd();
        var currentStars = PlayerPrefs.GetInt(moneyPPID, 0);
        currentStars++;

        UpdateTheMainGameUI();
        
        PlayerPrefs.SetInt(moneyPPID, currentStars);
    }

    public void UpdateTheMainGameUI()
    {
       var starscount = GameObject.Find("StarsCount");
       if (starscount != null)
       {
           var moneyPPID = WhichMoneyToAdd();
           var currentStars = PlayerPrefs.GetInt(moneyPPID, 0);
           
           TMP_Text text = starscount.GetComponent<TMP_Text>();
           int kittyFundStars = UpdatingKittyFund.GetCurrentKittyFund();
           text.SetText(kittyFundStars.ToString());

           var starscountbright = GameObject.Find("StarsCountBright");
           TMP_Text text2 = starscountbright.GetComponent<TMP_Text>();
           text2.SetText(kittyFundStars.ToString());
       }
    }
    
    public void AngelMoneyPlusPlus()
    {
        //Skipmoney and stars are updated
        var editorTest = GameObject.Find("EDITORMODE");
        if (editorTest) return;
        
        _angelMoney++;

        if (_angelMoney > 8)
        {
            //Collected enough...
            //TODO : Visuals and effects on nine
            IncrementCurrentKittyPot();
            _angelMoney = 0;
            
            int skipcount = PlayerPrefs.GetInt(_skipMoneyPPID,0);
            skipcount++;
            PlayerPrefs.SetInt(_skipMoneyPPID,skipcount);
            _skipMoney = skipcount;
            //KittyFund.SkipCountTotalRefresh();
        }
        
        PlayerPrefs.SetInt(_angelCollectedPPID, _angelMoney);
    }

    static void SkipCountTotalRefresh()
    {
        _skipMoney = PlayerPrefs.GetInt(_skipMoneyPPID,0);
    }

    public static void SetAngelMoney(int newAmountInKitty)
    {
        PlayerPrefs.SetInt(_angelCollectedPPID, newAmountInKitty);
        _angelMoney = newAmountInKitty;
    }
    
    public static int GetAngelMoney()
    {
        return _angelMoney;
    }
    
    public static int GetSkipMoney()
    {
        return _skipMoney;
    }
    
    public static void SetSkipMoney(int newMoney)
    {
        _skipMoney = newMoney;
    }

    public static int GetCakesMax()
    {
        return 321;
    }


    public static int GetCakeMoney()
    {
        return PlayerPrefs.GetInt(_cakeMoneyPPID,0);
    }

    public static bool GotAllCakes()
    {
        int cakes = GetCakeMoney();
        if (cakes >= GetCakesMax()) return true;
        return false;
    }

    public static void SetCakeMoney(int newMoney)
    {
        _cakeMoney = newMoney;
        PlayerPrefs.SetInt(_cakeMoneyPPID, newMoney);
    }


    public int GetMoney(int catID)
    {
        if (catID < 3) return PlayerPrefs.GetInt("EasyMoney");
        if (catID < 7) return PlayerPrefs.GetInt("MediumMoney");
        return PlayerPrefs.GetInt("HardMoney");
    }

    public void SetMoney(int catID,int newAmountInKitty)
    {
        string whichKittyFund = _hardMoneyPPID;
        if (catID > 7)
        {
            whichKittyFund = _hardMoneyPPID;
            _hardMoney = newAmountInKitty;
        }
        if ((catID >= 4)&&(catID <= 6))
        {
            whichKittyFund = _mediumMoneyPPID;
            _mediumMoney = newAmountInKitty;
        }
        if (catID < 3)
        {
            whichKittyFund = _easyMoneyPPID;
            _easyMoney = newAmountInKitty;
        }
        PlayerPrefs.SetInt(whichKittyFund, newAmountInKitty);
        OnMoneyUpdated?.Invoke();
        //Debug.Log($"{catID} {whichKittyFund} {newAmountInKitty}");
    }
    

}
