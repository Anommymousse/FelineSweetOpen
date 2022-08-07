using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static MyExtensions.MyExtensions;
public class MainMenuFullWipeOfPPLevels : MonoBehaviour
{
    //"EasyMoney" + level + "Money" + coords
    //"MediumMoney" + lvl + "Money" + coords

    static int maxLevel = 200;

    static string CreateFullFileAndPath(int passedInLevel)
    {
        string levelDifficulty = "Normal"; //Always normal

        
        string fullFileName = Application.dataPath + "/StreamingAssets" + "/" + levelDifficulty + passedInLevel + "N" + ".txt";
        return fullFileName;

    }


    static string[] LoadFileDataAsString(string pathandfilename)
    {
        string[] toobigafile = new string[0];
        var sr = new StreamReader(pathandfilename);
        var fileContents = sr.ReadToEnd();
        sr.Close();
                        
        var filecontents2 = fileContents.Split("\n"[0]);
          
        if (filecontents2.Length > 400)
        {
            return toobigafile;
        }
        
        return filecontents2;
    }
    
    public static void LockAllCats()
    {
        for (int i = 0; i < 12; i++)
        {
            string key = i + "Unlocked";
            PlayerPrefs.SetInt(key, 0);
        }
    }
    
    public static void NoMonies()
    { //Kitty
        var bootstrap = GameObject.Find("[BOOTSTRAP]");
        if (bootstrap)
        {
            var kittyFund = bootstrap.GetComponent<KittyFund>();
            kittyFund.SetMoney(1,0);
            kittyFund.SetMoney(5,0);
            kittyFund.SetMoney(8,0);
        }
    }


    public static void WipeAllStarAndCakeProgress()
    {
        maxLevel = 200;
        int cakecounter = 0;
        int starcounter = 0;
        
        for (int i = 1; i < maxLevel;i++)
        {
            string filename = CreateFullFileAndPath(i);
            string[] fileContents = LoadFileDataAsString(filename);

            foreach (var line in fileContents)
            {
                if (line.Contains("Gold_Star_128x128"))
                {
                    starcounter++;
                    int startindex = line.IndexOf('(');
                    int endindex = line.IndexOf(')');
                    int length = endindex - startindex;
                    if (startindex > 0)
                    {
                        string coords = line.Substring(startindex,length);
                        coords = coords + ")";
                        
                        string Keypart1_DifMoney = "EasyMoney";
                        if(i>30) Keypart1_DifMoney = "MediumMoney";
                        if(i>60) Keypart1_DifMoney = "HardMoney";
                    
                        string keypart2_intlevel = i.ToString();
                        string keypart3_money = "Money";
                        string keypart4 = coords;
                        string ppkey = Keypart1_DifMoney + keypart2_intlevel + keypart3_money + keypart4;
                        Log($"DELETING {ppkey}","cyan");
                        PlayerPrefs.DeleteKey(ppkey);
                    }
                }

                if (line.Contains("cake_128x128"))
                {
                    cakecounter++;
                    int startindex = line.IndexOf('(');
                    int endindex = line.IndexOf(')');
                    int length = endindex - startindex;
                    if (startindex > 0)
                    {
                        string coords = line.Substring(startindex,length);
                        coords = coords + ")";
                        
                        string Keypart1_DifMoney = "EasyMoney";
                        if(i>30) Keypart1_DifMoney = "MediumMoney";
                        if(i>60) Keypart1_DifMoney = "HardMoney";
                    
                        string keypart2_intlevel = i.ToString();
                        string keypart3_money = "CakeMoney";
                        string keypart4 = coords;
                        string ppkey = Keypart1_DifMoney + keypart2_intlevel + keypart3_money + keypart4;
                        Log($"DELETING {ppkey}","cyan");
                        PlayerPrefs.DeleteKey(ppkey);
                    }
                }
            }
        }
        Log($"TOTAL Star count {starcounter}","red");
        Log($"TOTAL Cake count {cakecounter}","red");
    }

    public void WipeAllStarsCollected()
    {
        WipeAllStarAndCakeProgress();
        LockAllCats();
        NoMonies();
    }
}
