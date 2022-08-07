using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EditorLoadButtonPress : MonoBehaviour
{
    public Tilemap layerToLoad;
    public GameObject LevelTextRef;
    public GameObject DifficultyTextRef;
    public GameObject HiddenKeyParentRef;
    public GameObject DescriptionRef;
    public GameObject EditorMouseStuffReference;
    List<string> hiddenNameList;
    float _timerToRepress = 1.0f;
    //static bool doneyet = false;
    
    void Start()
    {
        hiddenNameList = new List<string>();
        HiddenKeyParentRef = new GameObject();
        GenerateHiddenNameList();
    }
    
    public void ButtonPressed()
    {
        if (LevelLoader.runningTestMode == false)
        if (_timerToRepress < 0.01f)
        {
            LoadDataFromDiskAsText();
            _timerToRepress = 1.0f;
        }
    }

    void LoadDataFromDiskAsText()
    {
        string filename = CreateFileName(LevelTextRef, DifficultyTextRef, HiddenKeyParentRef);

#if UNITY_EDITOR
        string path = Application.streamingAssetsPath + "/" + filename + ".txt";
#else
        string path = Application.streamingAssetsPath + "/" + filename + ".txt";
#endif  
        
        if (!File.Exists(path)) {
            Debug.Log($"File : {path} not loaded as not found ");
        }
        else
        {
            LoadLevelData(path);    
        }
        
    }
    
    string CreateFileName(GameObject levelTextRef, GameObject difficultyTextRef, GameObject hiddenRef)
    {
        var LevelNumber = levelTextRef.GetComponent<TMP_Text>().text;
        var DifficultyMode = difficultyTextRef.GetComponent<TMP_Text>().text;
        var hiddenMode = "N";
        string LevelFileName = DifficultyMode + LevelNumber + hiddenMode;
        return LevelFileName;
    }

    string GetHiddenStatus(GameObject hiddenKeyParentRef)
    {
        string hiddenStatus = "N";
        var getToggleStatus = hiddenKeyParentRef.GetComponent<Toggle>().isOn;
        if (getToggleStatus == true) hiddenStatus = "H";
        return hiddenStatus;
    }

    void GenerateHiddenNameList()
    {
        hiddenNameList.Clear();
        hiddenNameList.Add("Angel");
        hiddenNameList.Add("cake128x128");
        hiddenNameList.Add("Gold_Star_128x128a");
        hiddenNameList.Add("star");
    }

    bool InHiddenList(string nameToTest)
    {
        bool _isHidden = hiddenNameList.Contains(nameToTest);
        return _isHidden;
    }
    
    void LoadLevelData(string pathandfilename)
    {
     //   int _totaltileCount = 0;
     //   int _countNonhiddenTiles=0;
     //   int _countHiddenTiles=0;

     //   string levelData="";
    //    List<Vector3Int> listOfTiles = new List<Vector3Int>();
    //    List<string> listOfNameForTiles = new List<string>();
        
        var sr = new StreamReader(pathandfilename);
        var fileContents = sr.ReadToEnd();
        sr.Close();
        Debug.Log($"{fileContents.Length} file length?");
        var filecontents2 = fileContents.Split("\n"[0]);

        if (filecontents2.Length > 400)
        {
            return;
        }
        
        Debug.Log($"<color=green> File Start {pathandfilename} </color> ");
        Debug.Log($"{filecontents2[2]}");
        Debug.Log("<color=green> File END </color> ");

        ClearAllTiles();
        var editorClickHandle = EditorMouseStuffReference.GetComponent<EditorMouseClickHandler>();
        foreach (var entry in filecontents2)
        {
            if (entry.Contains("[NonHidden"))
            {
                Vector3Int position = GetVector3Int(entry);
                string nameOfTile = GetNameOfTile(entry);
                editorClickHandle.SetTile(position,nameOfTile);        
            }
            if(entry.Contains("[Hidden"))
            {
                Vector3Int position = GetVector3Int(entry);
                string nameOfTile = GetNameOfTile(entry);
                editorClickHandle.SetTile(position,nameOfTile);
            }
        }
        
        
    }
    
    void ClearAllTiles()
    {
        for (int x = -11; x < 5; x++)
        {
            for (int y = -10; y < 3; y++)
            {
                Vector3Int postion = Vector3Int.zero;
                postion.x = x;
                postion.y = y;
                layerToLoad.SetTile(postion, null);
            }
        }
      
        layerToLoad.RefreshAllTiles();
    }

    string GetNameOfTile(string entry)
    {
        int endbraceplus2 = entry.IndexOf(")")+2;
        int endOfString = entry.IndexOf("]");
        var tilename = entry.Substring(endbraceplus2, endOfString - endbraceplus2);
        return tilename;
    }

    Vector3Int GetVector3Int(string entry)
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


    // Update is called once per frame
    void Update()
    {
        if (LevelLoader.runningTestMode == false)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }

        if (_timerToRepress > 0.0f) 
            _timerToRepress -= Time.deltaTime;
    }
}