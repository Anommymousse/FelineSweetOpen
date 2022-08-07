using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsTextData : MonoBehaviour
{
    //Streaming
    //string _filename = "Credits.json";
    string _filepath = Application.streamingAssetsPath;  
    public static CreditsText _creditsText;

    

    void Awake()
    {
        _filepath = Application.streamingAssetsPath;
    }

    public string GetStringAtIndex(int index)
    {
        if (index < 0) return null;
        if(index > (_creditsText.LinesOfText.Length-1)) return null;

        return _creditsText.LinesOfText[index];
    }

    public int GetLineCount()    
    {        
        return (_creditsText.LinesOfText.Length);
    }


    //[ContextMenu("Save strings")]
    public void SaveCredits()
    {
        /*_creditsText = new CreditsText
        {
            LinesOfText = new string[] { "cupcake", "candy", "funsies" }
        };        
        _filepath = "D:/downloads/Credits.json";
        var data = JsonUtility.ToJson(_creditsText);
        if (data != null)
            File.WriteAllText(_filepath, data);*/
    }

    //[ContextMenu("Load strings")]
    public void LoadCredits()
    {
        _filepath = Application.streamingAssetsPath + "/Credits.json";
        //_filepath = "D:/downloads/Credits.json";
        string rawdata = File.ReadAllText(_filepath);        
        _creditsText = JsonUtility.FromJson<CreditsText>(rawdata);
        //Could do with try/catch but that's for later me
        //Debug.Log("LOAD... LoadEND..." + _creditsText.LinesOfText[0] + _creditsText.LinesOfText[2]);
    }
    
    public void LoadEndOfGame()
    {
        _filepath = Application.streamingAssetsPath + "/EndCredits.json";
        //_filepath = "D:/downloads/Credits.json";
        string rawdata = File.ReadAllText(_filepath);        
        _creditsText = JsonUtility.FromJson<CreditsText>(rawdata);
        //Could do with try/catch but that's for later me
        //Debug.Log("LOAD... LoadEND..." + _creditsText.LinesOfText[0] + _creditsText.LinesOfText[2]);
    }


}

