using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyExtensions.MyExtensions;

public class UpdatingKittyFund : MonoBehaviour
{
    
    public static int GetCurrentKittyFund()
    {
        Debug.Log($"<color=green> KITTY : GetStar Called");

        var kittyFund = GameObject.Find("[BOOTSTRAP]").GetComponent<KittyFund>();
        var ppid = kittyFund.WhichMoneyToAdd();
        /*
        var difficultyObj = GameObject.Find("DifficultyObject");
        if (difficultyObj == null) return 0;
        var difficultyLevel = difficultyObj.GetComponent<DifficultyLevel>();
        if (difficultyLevel == null) return 0;
        
        var difficultyKittyKey = difficultyLevel.difficultySelected;
        difficultyKittyKey += "Money";*/
        int money = PlayerPrefs.GetInt(ppid,0);
        return money;
    }
    
    public static void AddStarToCurrentKittyFund(Vector3Int position)
    {
        AddItemToCurrentKittyFund(position, "Money", true);
    }
    public static void AddCakeToCurrentKittyFund(Vector3Int cakeCellLocation)
    {
        if (AddItemToCurrentKittyFund(cakeCellLocation, "CakeMoney", false))
        {
            int cakemoney = KittyFund.GetCakeMoney();
            cakemoney++;
            KittyFund.SetCakeMoney(cakemoney);
            Log($"Kitty fund cake to : {cakemoney}", "red");
        }
    }

    //format of check in prefs -: Difficulty + Level + itemName + itemCellLocation;
    public static bool AddItemToCurrentKittyFund(Vector3Int itemCellLocation,string itemName,bool isDifficultyUsed)
    {
        //Uses levelnum   
        var kittyFund = GameObject.Find("[BOOTSTRAP]").GetComponent<KittyFund>();
        var ppid = kittyFund.WhichMoneyToAdd(); //"EasyMoney" etc...

        var GO = GameObject.Find("LevelManager");
        var levelloader = GO.GetComponent<LevelLoader>();
        var level = levelloader.GetLevelLevel(); //level

        if (levelloader.GetLevelDifficulty().Contains("Custom")) return false;

        var starKeyDifficultyforPrefs = ppid;
        var starKeyLevelForPrefs = level;
        var starBaseKeyAndLevel = ppid + level;

        //Check for item already taken
        var KittyNoRepeatCakes = starBaseKeyAndLevel + itemName + itemCellLocation;
        var alreadyFound = PlayerPrefs.GetInt(KittyNoRepeatCakes, 0);
        if (alreadyFound == 1) return false;

        
        var currentFunds = GetCurrentKittyFund();
        currentFunds++;
        if (itemName.ToLowerInvariant().Contains("cake"))
        {
            PlayerPrefs.SetInt(KittyNoRepeatCakes, 1);
            return true;
        }
        else
        {
            PlayerPrefs.SetInt(starKeyDifficultyforPrefs, currentFunds);
            PlayerPrefs.SetInt(KittyNoRepeatCakes, 1);
            return true;
        }
        //New Item
        //var KittyVersionKey = itemName;
        //if (isDifficultyUsed) KittyVersionKey = starKeyDifficultyforPrefs + KittyVersionKey;
        /*int money = PlayerPrefs.GetInt(KittyVersionKey, 0);
        money++;
        PlayerPrefs.SetInt(KittyVersionKey, money);
        PlayerPrefs.SetInt(KittyNoRepeatCakes, 1);*/
    }
}
/*
     var difficultyObj = GameObject.Find("DifficultyObject");
     if (difficultyObj == null) return;
     var difficultyLevel = difficultyObj.GetComponent<DifficultyLevel>();
     if (difficultyLevel == null) return;
     
     var levelObj = GameObject.Find("LevelObject");
     if (levelObj == null) return;
     var levelLevel = levelObj.GetComponent<LevelLevelSelected>();
     if (levelLevel == null) return;
     
     var starKeyDifficultyforPrefs = difficultyLevel.difficultySelected;
     var starKeyLevelForPrefs = levelLevel.GetLevel();

     //Repeat?
     var KittyNoRepeatStars = starKeyDifficultyforPrefs + starKeyLevelForPrefs + "Money" + position;
     var alreadyFound = PlayerPrefs.GetInt(KittyNoRepeatStars,0);
     if (alreadyFound == 1) return;
     
     //New Star
     var KittyVersionKey = starKeyDifficultyforPrefs + "Money";
     int money = PlayerPrefs.GetInt(KittyVersionKey, 0);
     money++;
     PlayerPrefs.SetInt(KittyVersionKey, money);
     PlayerPrefs.SetInt(KittyNoRepeatStars, 1);*/