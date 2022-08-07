using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DarkTonic.PoolBoss;
using HiddenLevels;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using static MyExtensions.MyExtensions;

public class LevelLoader : MonoBehaviour
{
    static bool firstPassControllerCheck;
    public static bool runningTestMode=false;
    static LevelLoader _instance;
    public static int _reloadCounter;

    [SerializeField] List<AssetReference> LevelFilesAsText;
    AsyncOperationHandle<string[]> _handleAsyncLevel;
    string[] _levelAsText;

    //Door,Key,PlayerPos hammer
    /*public GameObject PenguinSpawner;
    public GameObject PenguinSpawnerLeft;
    public GameObject enemyTypePumpkinHorizontal;
    public GameObject enemyTypePumpkinVertical;*/
    
    
    public GameObject tileBoss;
        
    Vector3 halfTileAmount;

    bool referencesSet;
    //bool _isEditorModeActivated;
    bool _isGameModeInitialised;
    bool _isTimerDisabled;
    bool _isAsyncLoadLevelDoneYet;

    public BricksScriptable _BricksScriptable;

    public Tilemap _nonHiddenTilemapReference;    
    public Tilemap _hiddenTilemapReference;
    float _timerToUpdateTimer=0.0f;
    Vector3 _doorPosition;
    List<string> _poolBossEntries;
    List<GameObject> _enemyList;
    
    string _LevelDifficulty;
    static string _LevelLevel;
    string _LevelHidden;
    
    void StartUpGamePlayControls()
    {
        Controls _controls;
        _controls = new Controls();
        _controls.Gameplay.Enable();
    }

    public string GetLevelDifficulty()
    {
        return _LevelDifficulty;
    }

    public void SetLevelDifficulty(string leveldiff)
    {
        _LevelDifficulty = leveldiff;
    }
    
    public string GetLevelLevel()
    {
        return _LevelLevel;
    }
    
    public string GetLevelHidden()
    {
        return _LevelHidden;
    }

    bool HandleHiddenLevel()
    {
        if (_LevelDifficulty.ToLower().Contains("custom")) return false;
        if (_LevelDifficulty.ToLower().Contains("hidden"))
        {
            _LevelLevel = ChooseHiddenLevelAtRandom.GetNextLevelnum().ToString();
            _LevelDifficulty = "Normal";
            return false;
        }
        
        var level= ChooseHiddenLevelAtRandom.HiddenLevelCreation(Int32.Parse(_LevelLevel),_LevelDifficulty );
        
        PoolBoss.DespawnAllPrefabs();
        _isGameModeInitialised = false;
        _isTimerDisabled = false;
        _LevelDifficulty = ChooseHiddenLevelAtRandom.GetDifficulty();
        _LevelLevel = ChooseHiddenLevelAtRandom.GetHiddenLevelLevel().ToString();
        
        Log($"<color=blue> sanity check {_LevelDifficulty} {_LevelLevel} </color>");

        if (LevelLoadTiles() == false)
        {
            Log($"ERROR : Hidden file does not exist {level} ");
            SceneManager.LoadScene("DemoEnd");
        }

        LevelLevelSelected.SetLevel(Int32.Parse(_LevelLevel)); //probably set to 1-5
        _instance.StartCoroutine(_instance.DontWaitForPoolBossJustLoad());
            
        var brickMapRef = GameObject.Find("TilesBoss").GetComponent<BrickMap>();
            
        brickMapRef.PowerToSeeStars();
        brickMapRef.PowerToSeeCakes();

        return true;
    }
    
    public void LoadNextLevel(bool hidden)
    {
        if (_LevelDifficulty.ToLower().Contains("hidden"))
            hidden = true;
        
        if (hidden)
        {
            if(HandleHiddenLevel())
                return;
        }
        
        int number;
        {
            _LevelHidden = "N";
            
            bool resultok = Int32.TryParse(_LevelLevel, out number);
            if (!resultok) number = 1;
            number++;
            _LevelLevel = number.ToString();
        }
        
        Log($"Load New Level... diff= {_LevelDifficulty} level={_LevelLevel} hiddenstate={hidden}");
        
        //Clear enemies???
        PoolBoss.DespawnAllPrefabs();
        InitialiseTheLevel(_LevelDifficulty, _LevelLevel, _LevelHidden);
        if ((LevelLoadTiles() == false))//||(Int32.Parse(_LevelLevel)>10))
        {
            ES3.Save("UsernameLevel"+_LevelDifficulty, number-2);
            
            SceneManager.LoadScene("DemoEnd");
        }

        //Will this still work ?
        LevelreStart(); //Wait for poolboss ready gets a little long.
        
        Log($"New level started in theory");
    }
    
    public static void LevelreStart()
    {
        _reloadCounter++;
        Log($"<color=red> Counter at on levelrestart{_reloadCounter} </color>");
        
        if (_reloadCounter > 25)
        {
            Log($"<color=red> Triggered level LOAD! </color>");    
            //Log($"<color=blue> RELOAD TEST!</color>");
            _reloadCounter = 0;
            
            LevelLevelSelected.SetLevel(Int32.Parse(_LevelLevel));
            
            FadeController.FadeOut("LevelManagerEasy"); 
            //SceneManager.LoadScene("LevelManagerEasy");
        }
        
        _instance.StartCoroutine(_instance.DontWaitForPoolBossJustLoad());
        //_instance.TestThing();
        
        var _brickMapRef = GameObject.Find("TilesBoss").GetComponent<BrickMap>();
        _brickMapRef.PowerToSeeStars();
        _brickMapRef.PowerToSeeCakes();
        

    }

    public static void LevelStart()
    {
        _reloadCounter++;
        Log($"<color=red> Counter at on levelrestart{_reloadCounter} </color>");
        if (_reloadCounter > 25)
        {
            //Log($"<color=blue> RELOAD TEST!</color>");
            _reloadCounter = 0;
            FadeController.FadeOut("LevelManagerEasy"); 
            //SceneManager.LoadScene("LevelManagerEasy");
        }
        
        _instance.StartCoroutine(_instance.WaitForPoolBossThenLoad());
        
        
        var _brickMapRef = GameObject.Find("TilesBoss").GetComponent<BrickMap>();
        _brickMapRef.PowerToSeeStars();
        _brickMapRef.PowerToSeeCakes();
    }
    
    public static void ReloadLevel()
    {
        
        Log($"{Time.time} Reload level (just strings)");
        _instance.InitialiseTheLevel(_instance._LevelDifficulty, _LevelLevel, "N"); //_LevelHidden);
        Log($"{Time.time} Reload level end");
        
        Log($"{Time.time} levelloadtiles - looks safe");
        _instance.LevelLoadTiles();
        Log($"{Time.time} levelloadtiles end");
    }
    
    void ConvertAllLevelsToEasySave()
    {
        _LevelDifficulty = "Easy";
        _LevelHidden = "N";
        for (int i = 1; i < 31; i++)
        {
            _LevelLevel = i.ToString();
            LevelLoadTiles();
            string EasyLevelRefNonHidden = _LevelDifficulty + _LevelLevel + _LevelHidden + "NonHidden";
            string EasyLevelRefHidden = _LevelDifficulty + _LevelLevel + _LevelHidden + "Hidden";
            ES3.Save(EasyLevelRefNonHidden,_nonHiddenTilemapReference);
            ES3.Save(EasyLevelRefHidden,_nonHiddenTilemapReference);
        }
        
        _LevelDifficulty = "Normal";
        for (int i = 1; i < 21; i++)
        {
            _LevelLevel = i.ToString();
            LevelLoadTiles();
            string EasyLevelRefNonHidden = _LevelDifficulty + _LevelLevel + _LevelHidden + "NonHidden";
            string EasyLevelRefHidden = _LevelDifficulty + _LevelLevel + _LevelHidden + "Hidden";
            ES3.Save(EasyLevelRefNonHidden,_nonHiddenTilemapReference);
            ES3.Save(EasyLevelRefHidden,_nonHiddenTilemapReference);
        }

        Log("<color=green> ALL LEVELS SAVED IN ES3 FORMAT!!! </color>");   
        
    }
    
    
    void Awake()
    {
        _instance = this;
        runningTestMode = false;
        Log("LevelLoader: Awake...");

        if (firstPassControllerCheck == false)
        {
            firstPassControllerCheck = true;
            _reloadCounter = 0;
            StartUpGamePlayControls();
        }

        //Easy Save - all levels
        //ConvertAllLevelsToEasySave();
        //Application.Quit();
        
        if (IsEditorModeLevel() == true)
        {
            Log("LevelLoader: Editor Mode active");
            
            string PreviousDifficulty = PlayerPrefs.GetString("FSKey_Diff", "Easy");
            string PreviousLevel = PlayerPrefs.GetString("FSKey_Level", "1");
            string PreviousLevelHidden = "N"; //PlayerPrefs.GetString("FSKey_Hidden", "N");
            
            Log($"<color=green> loading pp {PreviousDifficulty} {PreviousLevel} {PreviousLevelHidden} </color>");
            InitialiseTheLevel(PreviousDifficulty, PreviousLevel, PreviousLevelHidden);
        }
        else
        {
            Log("LevelLoader: Normal Mode active");
            
            string localdiff = "Normal";
            string localLevel = "1";
            var difficultyGameObject = GameObject.Find("DifficultyObject");
            if (difficultyGameObject != null)
            {
                var difficultyLevel = difficultyGameObject.GetComponent<DifficultyLevel>();
                localdiff = difficultyLevel.GetDifficultyNS();
            }

            var levelGameObject = GameObject.Find("LevelObject");
            if (levelGameObject != null)
            {
                localLevel = levelGameObject.GetComponent<LevelLevelSelected>().GetLevel().ToString();
            }            
            
            InitialiseTheLevel(localdiff, localLevel, "N");
        }
        
        Log(("Level loader : awake part 2"));
        
        if(LevelLoadTiles())
        {
            Log(("Load Tiles Success"));
        }
        else
        {
            Log(("Load Tiles FAILURE!"));
        }
        
        if(IsEditorModeLevel()==false)
        {
            LevelStart();
        }
    }

    void OnDisable()
    {
    }

    string CreateES3FileKeyName()
    {
        string rv = _LevelDifficulty + _LevelLevel + _LevelHidden;
        return rv;
    }
    
    string CreateFullFileAndPath()
    {
        string rv = Application.dataPath + "/StreamingAssets" + "/" + _LevelDifficulty + _LevelLevel + _LevelHidden + ".txt";
        //string rv = Application.persistentDataPath + "/" + _LevelDifficulty + _LevelLevel + _LevelHidden + ".txt";
       return rv;
    }

    
    void InitialiseTheLevel(string difficulty, string levelNum, string hidden)
    {    
        Log("init load :" + difficulty+levelNum+hidden);
        if (IsEditorModeLevel())
        {
            ///_isEditorModeActivated = true;
            _isGameModeInitialised = false;
            _isTimerDisabled = true;
        }
        else
        {
            //_isEditorModeActivated = false;
            _isGameModeInitialised = false;
            _isTimerDisabled = false;
        }

        _LevelDifficulty = difficulty;
        _LevelLevel = levelNum;
        _LevelHidden = hidden;

        if (IsEditorModeLevel())
            AssignLevelDetailsToUI();
    }
    
    void ClearAllTiles(Tilemap tilemapToClear)
    {    
          for (int x = -11; x < 5; x++)
          {
             for (int y = -10; y < 3; y++)
             {
                Vector3Int postion = Vector3Int.zero;
                postion.x = x;
                postion.y = y;
                tilemapToClear.SetTile(postion, null);
             }
          }
    }


    bool ES3LevelLoadTiles()
    {
        string es3Keystring = CreateES3FileKeyName();
        string es3KeystringHidden = es3Keystring + "Hidden";
        string es3KeystringNonHidden = es3Keystring + "NonHidden";
        if (ES3.KeyExists(es3KeystringNonHidden))
        {
            _nonHiddenTilemapReference = ES3.Load<Tilemap>(es3KeystringNonHidden);
            return true;
        }

        if (ES3.KeyExists(es3KeystringHidden))
        {
            _hiddenTilemapReference = ES3.Load<Tilemap>(es3KeystringHidden);
            return true;
        }

        return false;
    }
    
    bool LevelLoadTiles()
    {
        //return ES3LevelLoadTiles();
        
        string[] levelData;
        
        if(DoesLevelFileExist())
        {
            
            string pathandfilename = CreateFullFileAndPath();
            Log("FULLPATH : " +pathandfilename);
            levelData = LoadFileDataAsString(pathandfilename);
            if (levelData.Length < 2)
            {
                Log("<color=red> levelData too large or non existent</color>");
                return false;
            }
            
            ClearAllTiles(_nonHiddenTilemapReference);
            ClearAllTiles(_hiddenTilemapReference);
            
            var referenceForConversionList = GameObject.Find("[MouseStuffs]");
            var tileListReferenceObject = referenceForConversionList.GetComponent<EditorMouseClickHandler>();     
            
            foreach (var entry in levelData)
            {
                if (entry.Contains("[NonHidden"))
                {
                    Vector3Int position = GetVector3Int(entry);
                    string nameOfTile = GetNameOfTile(entry);

                    if (nameOfTile.Contains("desert"))
                    {
                        nameOfTile = GetRandomDestructableTile().name;
                    }

                    if (nameOfTile.Contains("static"))
                    {
                        nameOfTile = GetRandomIndestructibleTile().name;
                    }
                    
                    tileListReferenceObject.SetTile(position,nameOfTile,false);                    
                }
                if(entry.Contains("[Hidden"))
                {
                    Vector3Int position = GetVector3Int(entry);
                    string nameOfTile = GetNameOfTile(entry);
                    tileListReferenceObject.SetTile(position,nameOfTile,true);                    
                }
            }
            
            return true;                    
        }
            
        Log($"File not found {CreateFullFileAndPath()}");
        
        return false;
    }

    Tile GetRandomDestructableTile()
    {
        int random = Random.Range(0, 6);
        if (random == 3) random = 5;
        if (random == 10) random = 12;
        if (random == 16) random = 18;
        return _BricksScriptable.TiledBricks[random];
    }

    Tile GetRandomIndestructibleTile()
    {
        int random = Random.Range(7,13);
        return _BricksScriptable.TiledBricks[random];
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

    string[] LoadFileDataAsString(string pathandfilename)
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

    bool DoesLevelFileExist()
    {
        string fullpathfilename = Application.dataPath + "/StreamingAssets"+ "/" + _LevelDifficulty + _LevelLevel + _LevelHidden + ".txt";
        //string fullpathfilename = Application.persistentDataPath+ "/" + _LevelDifficulty + _LevelLevel + _LevelHidden + ".txt";
        if(File.Exists(fullpathfilename))
        {
                return true;
        }
        return false;
    }
    
    void LevelStop()
    {
    
    }
    
    public static bool IsEditorModeLevel()
    {
        bool isInEditorMode = false;
        var gameobj = GameObject.Find("EDITORMODE");
        if (gameobj != null)
            isInEditorMode = true;
        return isInEditorMode;
    }

    //Starts and reloads the level
    public void TestLevelPlayToggle()
    {            
        if (_isGameModeInitialised ==false)
        {
            //_isEditorModeActivated = false;
            runningTestMode = true;
            AssignCurrentLevelUItoPlayerPrefs();   
            LevelStart();                                         
            
            var buttonObj1 = GameObject.Find("TestButtonText");
            var buttonObj2 = GameObject.Find("TestButtonText2");
            var text1 = buttonObj1.GetComponent<TMP_Text>();
            var text2 = buttonObj2.GetComponent<TMP_Text>();
            text1.SetText("Stop");
            text2.SetText("Stop");
            
            /*var buttonObj = GameObject.Find("TestButtonParent");
            var tmp_text = buttonObj.GetComponentInChildren<TMP_Text>();
            tmp_text.text = "STOP";*/
        }
        else
        {
            LevelStop();
            runningTestMode = false;
            SceneManager.LoadScene("Scenes/LevelEditor"); //Probably leaks memory
        }
    }

    public void AssignCurrentLevelUItoPlayerPrefs()
    {
        var go = GameObject.Find("DifficultyText");
        _LevelDifficulty = go.GetComponent<TMP_Text>().text;
        PlayerPrefs.SetString("FSKey_Diff", _LevelDifficulty);
        
        go = GameObject.Find("Level");
        _LevelLevel = go.GetComponent<TMP_Text>().text;
        PlayerPrefs.SetString("FSKey_Level", _LevelLevel);
        
        //go = GameObject.Find("AutoloadToggle");
        //_LevelHidden = go.GetComponent<Toggle>().isOn ? "H" : "N";
        //PlayerPrefs.SetString("FSKey_Hidden", _LevelHidden);
    }

    void AssignLevelDetailsToUI()
    {
        
        var diff = PlayerPrefs.GetString("FSKey_Diff", "Easy");
        var go = GameObject.Find("DifficultyText");
        _LevelDifficulty = diff;
        go.GetComponent<TMP_Text>().SetText(_LevelDifficulty);
        
        var level =PlayerPrefs.GetString("FSKey_Level", "1"); 
        go = GameObject.Find("Level");
        go.GetComponent<TMP_Text>().SetText(level);


        var OnOrOff = PlayerPrefs.GetString("FSKey_Hidden", "N");
        
        
        /*bool isOn = false;
        if (OnOrOff == "H") isOn = true;*/
        
        go = GameObject.Find("AutoloadToggle");
        go.GetComponent<Toggle>().isOn = true;

        OnOrOff = "N";
        EditorDifficultyChange.AssignLevel(diff);
        Log($"<color=green> Assigning level dets {diff} {level} {OnOrOff} </color>");
        
    }

    void TestThing()
    {
        _isGameModeInitialised = true;
        
        var bossBricks = tileBoss.GetComponent<BrickMap>();
        halfTileAmount = bossBricks.GetHalfTileAmount();
        _enemyList = new List<GameObject>();

        bossBricks.hiddenCandyCellsList = new List<Vector3Int>();
        bossBricks.hiddenCandyTilesList = new List<Tile>();

        _poolBossEntries = new List<string>();               
        foreach (var item in PoolBoss.Instance.poolItems)
        {
            _poolBossEntries.Add(item.gameObject.name);          
        }
        
        ActivateTileMap(_nonHiddenTilemapReference.gameObject);
        ReadAndSetNonHidden();
        ActivateTileMap(_hiddenTilemapReference.gameObject);
        ReadAndSetHiddenCandy();
        DeactivateTileMap(_hiddenTilemapReference.gameObject);

        FadeController.SetMainGameStallOff();
    }

    IEnumerator DontWaitForPoolBossJustLoad()
    {
        Log($"<color=red> DWPB(1) {Time.time} </color>");
        _isGameModeInitialised = true;
        yield return null;
        Log($"<color=red> DWPB(2) {Time.time} </color>");
        float waitExpiredTimer = Time.time + 5.0f;
        Log($"<color=red> DWPB(3) {Time.time} </color>");
        while (PoolBoss.IsReady == false)
        {
            Log($"<color=red> DWPB(4) {Time.time} </color>");
            yield return null;
            Log($"<color=red> DWPB(5) {Time.time} </color>");
            if (Time.time > waitExpiredTimer)
            {
                Log($"Poolboss took > 5 seconds ","cyan");
                yield break;
            }
        }
        Log($"<color=red> DWPB(6) {Time.time} </color>");

        var bossBricks = tileBoss.GetComponent<BrickMap>();
        halfTileAmount = bossBricks.GetHalfTileAmount();
        _enemyList = new List<GameObject>();

        bossBricks.hiddenCandyCellsList = new List<Vector3Int>();
        bossBricks.hiddenCandyTilesList = new List<Tile>();
        Log($"<color=red> DWPB(7) {Time.time} </color>");
        
        _poolBossEntries = new List<string>();
        Log($"<color=red> DWPB(8) Item count {PoolBoss.Instance.poolItems.Count} {Time.time} </color>");
        foreach (var item in PoolBoss.Instance.poolItems)
        {
            _poolBossEntries.Add(item.gameObject.name);          
        }
        Log($"<color=red> DWPB(9) {Time.time} </color>");
        ActivateTileMap(_nonHiddenTilemapReference.gameObject);
        ReadAndSetNonHidden();
        Log($"<color=red> DWPB(10) {Time.time} </color>");
        ActivateTileMap(_hiddenTilemapReference.gameObject);
        ReadAndSetHiddenCandy();
        Log($"<color=red> DWPB(11) {Time.time} </color>");
        DeactivateTileMap(_hiddenTilemapReference.gameObject);

        FadeController.SetMainGameStallOff();
        Log($"<color=red> DWPB(12) {Time.time} </color>");
    }


    IEnumerator WaitForPoolBossThenLoad()
    {
        _isGameModeInitialised = true;
        yield return null;
        
        while (PoolBoss.IsReady == false)
        {
            yield return null;
        }
        
        var bossBricks = tileBoss.GetComponent<BrickMap>();
        halfTileAmount = bossBricks.GetHalfTileAmount();
        _enemyList = new List<GameObject>();

        bossBricks.hiddenCandyCellsList = new List<Vector3Int>();
        bossBricks.hiddenCandyTilesList = new List<Tile>();

        _poolBossEntries = new List<string>();               
        foreach (var item in PoolBoss.Instance.poolItems)
        {
            _poolBossEntries.Add(item.gameObject.name);          
        }
        
        ActivateTileMap(_nonHiddenTilemapReference.gameObject);
        
        ReadAndSetNonHidden();        
        
        ActivateTileMap(_hiddenTilemapReference.gameObject);
        ReadAndSetHiddenCandy();
        DeactivateTileMap(_hiddenTilemapReference.gameObject);

        FadeController.SetMainGameStallOff();
        
    }

    bool isInPoolBossList(string searchItem)
    {
        return _poolBossEntries.Contains(searchItem);
    }
    
    void ReadAndSetHiddenCandy()
    {
        var go = tileBoss.GetComponent<BrickMap>();
        
        go.hiddenCandyCellsList = go.GetActiveTiles(go.HiddenRewardsTilemap);
        go.hiddenCandyTilesList = go.GetMatchingTiles(go.HiddenRewardsTilemap);

        foreach (var tile in go.hiddenCandyTilesList)
        {
            go.hiddenCandyActiveList.Add(false);
            go.hiddenCandyCollectedList.Add(false);
        }
    }
        
    Transform PoolBossSpawn(string name, Vector3Int cellPosition)
    {
        Vector3 worldposition1 = tileBoss.GetComponent<BrickMap>().NonHiddenTilemap.CellToWorld(cellPosition);
        return PoolBoss.SpawnInPool(name, worldposition1 + halfTileAmount, Quaternion.identity);
    }
    
    [ContextMenu("ReadAndSetPlayerLocation")]
    public void ReadAndSetPlayerLocation(Vector3Int playerPos)
    {
        var worldpos = _nonHiddenTilemapReference.GetComponent<Tilemap>().CellToWorld(playerPos);
        var playerObject = GameObject.Find("Player");
        //var xdiff = go.GetHalfTileAmount().x;
        worldpos.x += 0.5f;
        playerObject.transform.position = worldpos;
    }
    
    public void ReadAndSetNonHidden()
    {
        var go = tileBoss.GetComponent<BrickMap>();
        var activeCells = go.GetActiveTiles(go.NonHiddenTilemap);
        var activeTiles = go.GetMatchingTiles(go.NonHiddenTilemap);

        for (int i = 0; i < activeCells.Count; i++)
        {
            var cellName = activeTiles[i].name;
            
            bool cellwasEnemy = HandleEnemySpawns(cellName, activeCells[i]);

            if (cellwasEnemy == false)
            {
                Log($"non-enemy {cellName}");
                if (isNotBrick(cellName))
                {
                    if (isInPoolBossList(cellName)||cellName.Contains("door")||cellName.Contains("Door"))
                    {
                        if(cellName.Contains("door")||cellName.Contains("Door"))
                        {
                            tileBoss.GetComponent<BrickMap>().SetDoorCellPosition(activeCells[i]);
                            PoolBossSpawn("doorclosed", activeCells[i]);
                        }
                        else
                            PoolBossSpawn(cellName, activeCells[i]);
                    }
                    else
                    {
                        if (cellName == "PlayerStartPos")
                        {
                            ReadAndSetPlayerLocation(activeCells[i]);
                        }
                        Log($"{cellName} was not found in poolboss list ");
                    }
                    go.NonHiddenTilemap.SetTile(activeCells[i], null);

                    if (cellName.Contains("Key"))
                    {
                        
                    }
                }

                //PoolBossSpawn(cellName, activeCells[i]);
            }
            else
            {
                go.NonHiddenTilemap.SetTile(activeCells[i],null);
                go.NonHiddenTilemap.RefreshTile(activeCells[i]);
            }
            
            
        }
    }

    bool isNotBrick(string cellName)
    {
        bool isNotBrick = true;
        cellName = cellName.ToLower();
        if (cellName.Contains("brick"))
            isNotBrick = false;
        if (cellName.Contains("desert"))
            isNotBrick = false;        
        if (cellName.Contains("static"))
            isNotBrick = false;        
        return isNotBrick;
    }


    bool isAnEnemy(string testName)
    {
        bool isAnEnemy = false || testName.Contains("Pumpkin") || testName.Contains("Dragon");

        if (testName.Contains("Penguin")) isAnEnemy = true;
        if (testName == "GreenFlame") isAnEnemy = true;
        if (testName.Contains("Electric")) isAnEnemy = true;
        if (testName.Contains("TestSpawner")) isAnEnemy = true;
        if (testName.Contains("Bird")) isAnEnemy = true;
        if (testName.Contains("Birb")) isAnEnemy = true;
        if (testName.Contains("Gas")) isAnEnemy = true;
        if (testName.ToLower().Contains("magma")) isAnEnemy = true;
        if (testName.Contains("magma")) isAnEnemy = true;
        if (testName.Contains("PirateSkelly")) isAnEnemy = true;
        if (testName.Contains("FireDragon")) isAnEnemy = true;
        if (testName.Contains("Mushroom")) isAnEnemy = true;
        if (testName.Equals("Book")) isAnEnemy = true;
        if (testName.Contains("Ghost")) isAnEnemy = true;
        if (testName.Contains("Mummy")) isAnEnemy = true;
        if (testName.Contains("Yeti")) isAnEnemy = true;
        if (testName.Contains("Wabbit")) isAnEnemy = true;
        if (testName.Contains("FireWolf")) isAnEnemy = true;
        if (testName.Contains("SwordBlock")) isAnEnemy = true;
        if (testName.Contains("MossyCrab")) isAnEnemy = true;
        if (testName.Contains("FluffyBat")) isAnEnemy = true;
        if (testName.Contains("ExtinctOx")) isAnEnemy = true;
        if (testName.Contains("DarkMushroom")) isAnEnemy = true;
        
        
        return isAnEnemy;
    }

    void BirdSpawnSetup(string enemyName, Vector3Int activeCell)
    {
        var goRef = PoolBossSpawn("Bird", activeCell);
        if (goRef == null) return;
        
        var enemyBird = goRef.GetComponent<EnemyBird>();
        Vector3 worldposition = goRef.position;                
        enemyBird.startPosition = worldposition;
        enemyBird.startDirection = Vector3.right;
        if (enemyName.Contains("Up")) enemyBird.startDirection = Vector3.up;
        if (enemyName.Contains("Down")) enemyBird.startDirection = Vector3.down;
        if (enemyName.Contains("Left"))
        {
            enemyBird.startDirection = Vector3.left;
            
        }
        enemyBird.staticBrickFullReverse = false;
    }

    void BirbSpawnSetup(string enemyName, Vector3Int activeCell)
    {
        Log("BirdSpawn start","cyan");
        
        var goRef = PoolBossSpawn("Birb", activeCell);
        if (goRef == null) return;
        
        Log("BirdSpawn continue","cyan");
        var enemyBird = goRef.GetComponent<EnemyBird>();
        Vector3 worldposition = goRef.position;                
        enemyBird.startPosition = worldposition;
        enemyBird.startDirection = Vector3.right;
        if (enemyName.Contains("Up")) enemyBird.startDirection = Vector3.up;
        if (enemyName.Contains("Down")) enemyBird.startDirection = Vector3.down;
        if (enemyName.Contains("Left")) enemyBird.startDirection = Vector3.left;
        
        Log($"Bird name= {enemyName} ","cyan");

        enemyBird.SetupRespawnBirb();
        enemyBird.staticBrickFullReverse = true;
        Log($"Bird start direction = {enemyBird.startDirection} ","cyan");
    }

    void PumpkinSpawnSetup(string enemyName, Vector3Int activeCell)
    {
        if (enemyName.Contains("Up")||enemyName.Contains("Down"))
        {
            var goRef = PoolBossSpawn("PumpkinVert", activeCell);
            if (goRef != null)
            {
                var pingpong = goRef.GetComponent<EnemyPingPong>();
                if (enemyName.Contains("Down"))
                    pingpong.SwapDirection();

                Vector3 worldposition = goRef.position;                
                goRef.position = worldposition;
            }
        }

        if (enemyName.Contains("Left") || enemyName.Contains("Right"))
        {
            var goRef = PoolBossSpawn("CuteFlyer", activeCell);
            var temp = goRef.position;
            temp.y += 0.26f;
            goRef.position= temp;
            if (goRef != null)
            {
                var pingpong = goRef.GetComponent<EnemyPingPong>();
                if (enemyName.Contains("Left"))
                    pingpong.SwapDirection();

                Vector3 worldposition = goRef.position;
                //worldposition.x -= halfTileAmount.x;
                worldposition.y -= halfTileAmount.y;
                goRef.position = worldposition;
            }
        }
    }
    
    bool HandleEnemySpawns(string enemyName, Vector3Int activeCell)
    {
        if (isAnEnemy(enemyName) == false) return false;
        
        if (enemyName.Contains("Pumpkin"))
        {
            PumpkinSpawnSetup(enemyName,activeCell);
            return true;
        }

        if (enemyName.Contains("Bird"))
        {
            BirdSpawnSetup(enemyName,activeCell);
            return true;
        }

        if (enemyName.Contains("Birb"))
        {
            BirbSpawnSetup(enemyName,activeCell);
            return true;
        }

        //Flames and wall huggers.
        if (SimpleSpawnEnemy(enemyName))
        {
            Log($" PoolBoss Spawn EnemyName : {enemyName}");
            PoolBossSpawn(enemyName, activeCell);
            return true;
        }

        if (HandleDragonSpawns(enemyName, activeCell) == true) return true;

        if (enemyName.Contains("Penguin"))
        {
            PenguinSpawnerSetup(enemyName, activeCell);
            return true;
        }

        if (enemyName.ToLower().Contains("magma"))
        {
            string penguinType = enemyName.Contains("Left") ? "MagmaSpawnerLeft" : "MagmaSpawner";
            PoolBossSpawn(penguinType, activeCell);
            return true;
        }

        if( enemyName.ToLower().Contains("yeti")||enemyName.ToLower().Contains("grasswabbit")||enemyName.ToLower().Contains("extinctox"))
        {
            Log($"<color=cyan> Spawning {enemyName} </color>");
            GenericSpawnerSetup(enemyName, activeCell);
            return true;
        }
        
        //Default : Just spawn the thing
        Log($" PoolBoss default Enemy Spawn EnemyName : {enemyName}");
        PoolBossSpawn(enemyName, activeCell);

        return true;
    }

    void GenericSpawnerSetup(string enemyName, Vector3Int activeCell)
    {
        string AdjustedName = "Spawner" + enemyName;
        PoolBossSpawn(AdjustedName, activeCell);
    }

    
    void PenguinSpawnerSetup(string enemyName, Vector3Int activeCell)
    {
        string penguinType = enemyName.Contains("Left") ? "TestSpawnerLeft" : "TestSpawner";
        PoolBossSpawn(penguinType, activeCell);
    }

    bool HandleDragonSpawns(string enemyToTest,Vector3Int cellPosition)
    {
        if (enemyToTest.Contains("Dragon") == false) return false;
        if (enemyToTest.Contains("FireDragon")) return false; 
        float timeFactor = 0.0f;
        string adjustedName = enemyToTest;
        if (enemyToTest.Contains("01"))
        {
            timeFactor = 1.0f;
            adjustedName = enemyToTest.Substring(0, enemyToTest.Length - 2);
        }
        if (enemyToTest.Contains("02"))
        {
            timeFactor = 2.0f;
            adjustedName = enemyToTest.Substring(0, enemyToTest.Length - 2);
        }
        if (enemyToTest.Contains("03")){
            timeFactor = 3.0f;
            adjustedName = enemyToTest.Substring(0, enemyToTest.Length - 2);
        }
        var poolBossRef = PoolBossSpawn(adjustedName,cellPosition);
        poolBossRef.GetComponent<EnemyStaticFirePrefab>().fireInitialDelay = timeFactor;
        
        return true;
    }

    bool SimpleSpawnEnemy(string enemyToTest)
    {
        bool enemyFound = false;

        if (enemyToTest == "GreenFlame") enemyFound = true;
        if (enemyToTest == "GasLeakGreen") enemyFound = true;
        if (enemyToTest == "ElectricClockwise") enemyFound = true;
        if (enemyToTest == "ElectricAntiClockwise") enemyFound = true;
        //if (enemyToTest.Contains("Penguin")) enemyFound = true;
        
        return enemyFound;
    }

    void ActivateTileMap(GameObject tilemapToActivate)
    {
        tilemapToActivate.SetActive(true);
    }

    void DeactivateTileMap(GameObject tilemapToDeactivate)
    {
        tilemapToDeactivate.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _timerToUpdateTimer -= Time.deltaTime;
        if (_timerToUpdateTimer < 0.0f)
        {
            if (_isTimerDisabled)
                InGameCountDownTimer.AddToTimer(15);
            _timerToUpdateTimer = 10.0f;
        }
    }
}
