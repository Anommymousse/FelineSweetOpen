using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EditorSaveButtonPress : MonoBehaviour
{
    public Tilemap layerToSave;
    public Tilemap hiddenLayerToSave;
    public GameObject LevelTextRef;
    public GameObject DifficultyTextRef;
    public GameObject HiddenKeyParentRef;
    public GameObject DescriptionRef;
    List<string> hiddenNameList;

    float _timerToRepress = 1.0f;
    //static bool doneyet = false;
    
    // Start is called before the first frame update
    void Start()
    {
        hiddenNameList = new List<string>();
        GenerateHiddenNameList();
        HiddenKeyParentRef = new GameObject();
    }

    
    public void ButtonPressed()
    {
        if (_timerToRepress < 0.01f)
        {
            SaveDataOutToDiskAsText();
            _timerToRepress = 1.0f;
        }
    }

    void SaveDataOutToDiskAsText()
    {
        string filename = CreateFileName(LevelTextRef, DifficultyTextRef, HiddenKeyParentRef);
//Warning : Could go wrong if so, persistentDataPath
#if UNITY_EDITOR
        string path = Application.streamingAssetsPath + "/" + filename + ".txt";
    #else
        string path = Application.streamingAssetsPath + "/" + filename + ".txt";
    #endif  
        
        if (!File.Exists(path)) {
            File.WriteAllText(path," ");
        }
        File.WriteAllText(path,"Tileset: 0 \n");
        string levelAsText = CreateLevelData();
        File.AppendAllText(path,levelAsText);
    }
    
    string CreateFileName(GameObject levelTextRef, GameObject difficultyTextRef, GameObject hiddenRef)
    {
        var LevelNumber = levelTextRef.GetComponent<TMP_Text>().text;
        var DifficultyMode = difficultyTextRef.GetComponent<TMP_Text>().text;
        var hiddenMode = GetHiddenStatus(hiddenRef);
        string LevelFileName = DifficultyMode + LevelNumber + hiddenMode;
        return LevelFileName;
    }

    string GetHiddenStatus(GameObject hiddenKeyParentRef)
    {
        string hiddenStatus = "N";
        //var getToggleStatus = hiddenKeyParentRef.GetComponent<Toggle>().isOn;
        //if (getToggleStatus == true) hiddenStatus = "H";
        return hiddenStatus;
    }

    string RenameString(string stringToCheck)
    {
        string outputString=stringToCheck;
        if (stringToCheck.Contains("Penguin_Enemy03")) outputString = "Penguin_Spawner";
        return outputString;
    }

    void GenerateHiddenNameList()
    {
        hiddenNameList.Clear();
        hiddenNameList.Add("Angel");
        hiddenNameList.Add("cake_128x128");
        hiddenNameList.Add("Gold_Star_128x128a");
        hiddenNameList.Add("star");
    }

    bool InHiddenList(string nameToTest)
    {
        bool _isHidden = hiddenNameList.Contains(nameToTest);
        return _isHidden;
    }
    
    string CreateLevelData()
    {
        //int _totaltileCount = 0;
        //int _countNonhiddenTiles=0;
        //int _countHiddenTiles=0;

        string levelData="";
        List<Vector3Int> listOfTiles = new List<Vector3Int>();
        List<string> listOfNameForTiles = new List<string>();
        
        levelData += $"\n TileCount: No longer used \n";
        
        var bounds = layerToSave.cellBounds;
        foreach (var pos in layerToSave.cellBounds.allPositionsWithin)
        {
                if (layerToSave.HasTile(pos))
                {
                    string _name = layerToSave.GetTile<Tile>(pos).name;
                    string tileposAsString = pos.ToString();
                    _name = RenameString(_name);
                    
                    if(InHiddenList(_name))
                        levelData += $"[Hidden,{tileposAsString},{_name}] \n";
                    else
                        levelData += $"[NonHidden,{tileposAsString},{_name}] \n";
                }
        }

        bounds = hiddenLayerToSave.cellBounds;
        foreach (var pos in hiddenLayerToSave.cellBounds.allPositionsWithin)
        {
            if (hiddenLayerToSave.HasTile(pos))
            {
                string _name = hiddenLayerToSave.GetTile<Tile>(pos).name;
                string tileposAsString = pos.ToString();
                _name = RenameString(_name);
                levelData += $"[Hidden,{tileposAsString},{_name}] \n";
            }
        }
        
        levelData += $"\n:END:";
        
        return levelData;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (_timerToRepress > 0.0f) 
            _timerToRepress -= Time.deltaTime;
    }
}
