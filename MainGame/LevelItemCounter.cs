using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelItemCounter : MonoBehaviour
{
    public static int GetItemCountMaxForLevel(string levelName,string level,string hidden, string itemNameToCount)
    {
        int howmany = 0;
        string fullpathfilename = Application.dataPath + "/StreamingAssets"+ "/" + levelName+ level + hidden + ".txt";

        if (File.Exists(fullpathfilename))
        {
            var levelData = LoadFileDataAsString(fullpathfilename);
            foreach (var entry in levelData)
            {
                if (entry.Contains(itemNameToCount))
                {
                    Vector3Int position = GetVector3Int(entry);
                    howmany++;
                }
            }
        }

        return howmany;
    }

    public static int GetItemCountForLevel(string levelName,string level,string hidden, string itemNameToCount)
    {
        int howmany = 0;
        string fullpathfilename = Application.dataPath + "/StreamingAssets"+ "/" + levelName+ level + hidden + ".txt";

        if (File.Exists(fullpathfilename))
        {
            var levelData = LoadFileDataAsString(fullpathfilename);
            foreach (var entry in levelData)
            {
                if (entry.Contains(itemNameToCount))
                {
                    Vector3Int position = GetVector3Int(entry);
                    if(HasItemBeenTakenbefore(levelName, level, position.ToString(), itemNameToCount)==false)
                        howmany++;
                }
            }
        }

        return howmany;
    }

    public static string GetItemsForLevel(string levelName, string level, string hidden, string itemNameToCount)
    {
        string returnstring="";
        string fullpathfilename = Application.dataPath + "/StreamingAssets"+ "/" + levelName+ level + hidden + ".txt";

        if (File.Exists(fullpathfilename))
        {
            var levelData = LoadFileDataAsString(fullpathfilename);
            foreach (var entry in levelData)
            {
                if (entry.Contains(itemNameToCount))
                {
                    Vector3Int position = GetVector3Int(entry);
                    returnstring += "{";
                    returnstring += position.ToString();
                    returnstring += "}";
                }
            }
        }

        return returnstring;
    }
    
    static Vector3Int GetVector3Int(string entry)
    {
        Vector3Int cellPosition = Vector3Int.zero;
        int numbersstart = entry.IndexOf("(")+1;
        int numbersend = entry.IndexOf(")")-1;
        var numbersAsString = entry.Substring(numbersstart, numbersend - numbersstart);
        var splitNumbers = numbersAsString.Split(","[0]);

        int result;
        var x = int.TryParse(splitNumbers[0],out result);
        cellPosition.x = result;
        var y = int.TryParse(splitNumbers[1],out result);
        cellPosition.y = result;
        
        return cellPosition;
    }

    static bool HasItemBeenTakenbefore(string difficulty, string level,string position,string cakeormoney)
    {
        if (difficulty.ToLowerInvariant().Contains("normal"))
            difficulty = "EasyMoney";
        var extra = "";
        if (cakeormoney.ToLowerInvariant().Contains("cake"))
            extra = "Cake";
        var KittyNoRepeatStars = difficulty + level + extra + "Money" + position;
        var isTaken = PlayerPrefs.GetInt(KittyNoRepeatStars,0);
        return isTaken == 1;
    }

    public static string[] LoadFileDataAsString(string pathandfilename)
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

    
}
