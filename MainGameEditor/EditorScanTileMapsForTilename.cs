using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class EditorScanTileMapsForTilename : MonoBehaviour
{
    public Tilemap NonHidden;
    public Tilemap Hidden;
    static List<Tile> tileList = new List<Tile>();
    static List<string> tileNameList = new List<string>();
    
    public Action OnPlayerSpawnSet;
    public Action OnAngelSpawnSet;
    public Action OnAngelBookSpawnSet;
    public Action OnKeySpawnSet;
    public Action OnDoorSpawnSet;
    
    public Action OnPlayerDespawn;
    public Action OnAngelDespawn;
    public Action OnAngelBookDespawn;
    public Action OnKeyDespawn;
    public Action OnDoorDespawn;

    public Action UpdateSaveStatus;

    static bool _playerSpawnTileSet;
    static bool _angelTileSet;
    static bool _bookTileSet;
    static bool _keyTileSet;
    static bool _doorTileSet;
    static bool _saveIsEnabled;
    
    string PlayerSpawnTilename = "PlayerStartPos";
    string AngelTilename = "Angel";
    string BookTilename = "AngelBook";
    string KeyTilename = "key128x128";
    string DoorTilename = "closeddoor";
    
    public bool IsSaveEnabled => _saveIsEnabled;

    public enum tileNameIndexes
    {
        Player,
        Angel,
        Book,
        Key,
        Door,
    };
    
    void SetupMatchingEnumStrings()
    {
        tileNameIndexMatchingString.Add(PlayerSpawnTilename);
        tileNameIndexMatchingString.Add(AngelTilename);
        tileNameIndexMatchingString.Add(BookTilename);
        tileNameIndexMatchingString.Add(KeyTilename);
        tileNameIndexMatchingString.Add(DoorTilename);
    }

    static List<string> tileNameIndexMatchingString = new List<string>();

    //Tile names are set in stone! Magic text strings m'bad? 
    
    static bool _initialSetupComplete = false;
    bool neverreturn=true;
    
    public bool TestForTilePlaced(string testTilename)
    {
        return tileNameList.Contains(testTilename);
    }

    void Start()
    {
        if (_initialSetupComplete == false)
        {
            _initialSetupComplete = true;
            SetupMatchingEnumStrings();
    
            var tilemap = NonHidden;
            var hiddenmap = Hidden;
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(pos))
                {
                    var curTile = tilemap.GetTile<Tile>(pos);
                    tileList.Add(curTile);
                }

                if (hiddenmap.HasTile(pos))
                {
                    var curTile = hiddenmap.GetTile<Tile>(pos);
                    tileList.Add(curTile);
                }
            }

            foreach (var tile in tileList)
                tileNameList.Add(tile.name);
            
            /*Debug.Log($"player = {_playerSpawnTileSet}");
            Debug.Log($"angel = {_angelTileSet}");
            Debug.Log($"book = {_bookTileSet}");
            Debug.Log($"door = {_doorTileSet}");
            Debug.Log($"key = {_keyTileSet}");*/
            
            
            OnPlayerSpawnSet+= SetPlayerSpawnFlag;
            OnAngelSpawnSet += SetAngelSpawnFlag;
            OnAngelBookSpawnSet += SetAngleBookSpawnFlag;
            OnKeySpawnSet += SetKeySpawnFlag;
            OnDoorSpawnSet += SetDoorSpawnFlag;
            
            OnAngelDespawn += UnsetAngelSpawnFlag;
            OnPlayerDespawn += UnsetPlayerSpawnFlag;
            OnKeyDespawn += UnsetKeySpawnFlag;
            OnAngelBookDespawn += UnsetAngleBookSpawnFlag;
            OnDoorDespawn += UnsetDoorSpawnFlag;

            UpdateSaveStatus += Dummy;
        }
    }

    void Dummy()
    {
    }

    void SetPlayerSpawnFlag() => _playerSpawnTileSet = true;
    void UnsetPlayerSpawnFlag() => _playerSpawnTileSet = false;
    void SetAngelSpawnFlag() => _angelTileSet = true;
    void UnsetAngelSpawnFlag() => _angelTileSet = false;
    void SetAngleBookSpawnFlag() => _bookTileSet = true;
    void UnsetAngleBookSpawnFlag() => _bookTileSet = false;
    void SetKeySpawnFlag() => _keyTileSet = true;
    void UnsetKeySpawnFlag() => _keyTileSet = false;
    void SetDoorSpawnFlag() => _doorTileSet = true;
    void UnsetDoorSpawnFlag() => _doorTileSet = false;

    void EnableSaveFlag() => _saveIsEnabled = true; 
    void DisbleSaveFlag() => _saveIsEnabled = false;

    void OnDestroy()
    {
        neverreturn = false;
        //StopCoroutine(DetectChangeToTilemap());
    }

    void Awake()
    {
        _initialSetupComplete = false;
        _saveIsEnabled = false;
        StartCoroutine(DetectChangeToTilemap());
    }

    List<Vector3Int> GetActiveTiles(Tilemap tilemap)
    {
        List<Vector3Int> returnList = new List<Vector3Int>();
        var bounds = tilemap.cellBounds;
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                returnList.Add(pos);                                
            }
        }
        return returnList;
    }

    
    IEnumerator DetectChangeToTilemap()
    {
        yield return null;

        while (neverreturn)
        {
            yield return new WaitForSeconds(1.0f);
            var cellpos = GetActiveTiles(NonHidden);
            if(TileNameExists(cellpos, PlayerSpawnTilename))
                OnPlayerSpawnSet.Invoke();
            else
                OnPlayerDespawn.Invoke();
            
            if (TileNameExists(cellpos, DoorTilename))
                OnDoorSpawnSet.Invoke();
            else
            {
                OnDoorDespawn.Invoke();
                _doorTileSet = false;
            }
            
            if (TileNameExists(cellpos, KeyTilename))
                OnKeySpawnSet.Invoke();
            else
                OnKeyDespawn.Invoke();
            
            if (TileNameExists(cellpos, BookTilename))
                OnAngelBookSpawnSet.Invoke();
            else
                OnAngelBookDespawn.Invoke();

            cellpos = GetActiveTiles(Hidden);
            if(TileNameExists(cellpos, AngelTilename,true))
                OnAngelSpawnSet.Invoke();
            else
                OnAngelDespawn.Invoke();

            UpdateAllSaveRequirements();
            
            //Debug.Log("<color=green> Updating save state </color>");
        }
        
        yield return null;
    }

    bool TileNameExists(List<Vector3Int> cellpos, string tilename,bool hidden=false)
    {
        bool tileExists = false;
        if(hidden==false)
        {
            foreach (var tilepos in cellpos)
            {
                var tile = NonHidden.GetTile<Tile>(tilepos);
                if(tile!=null)
                    if (tile.name == tilename)
                        tileExists = true;
            } 
        }
        else
        {
            foreach (var tilepos in cellpos)
            {
                var tile = Hidden.GetTile<Tile>(tilepos);
                if (tile != null)
                    if (tile.name == tilename)
                        tileExists = true;
            }
        }
        return tileExists;
    }


    void Update()
    {
        
    }

    void UpdateAllSaveRequirements()
    {
        if (_angelTileSet && _bookTileSet && _doorTileSet && _keyTileSet && _playerSpawnTileSet)
        {
            _saveIsEnabled = true;
            UpdateSaveStatus.Invoke();

            /*if (_saveIsEnabled == false)
            {
                Debug.Log("Save status update allowed");
            }*/
        }
        else
        {
            //Called once a sec.
            _saveIsEnabled = false;
            UpdateSaveStatus.Invoke();
            
            /*if (_saveIsEnabled)
            {
            
            
                Debug.Log("Save status update Nope!");
            }*/
        }
    }
    

}
