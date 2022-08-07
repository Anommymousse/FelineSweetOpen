using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using static MyExtensions.MyExtensions;  

public class BrickMap : MonoBehaviour
{
        static BrickMap _instance;
        
        public Tile brokenTileToUse;
        public Tile brokenTileToUse01;
        public Tile brokenTileToUse02;
        public Tile brokenTileToUse03;
        public Tile brokenTileToUse04;
        public Tile brokenTileToUse05;
        public Tile brokenTileToUseCat;
        public Tile solidTileToUse;
        public Tile SpecialKey01;
        public Tilemap NonHiddenTilemap;
        public Tilemap HiddenRewardsTilemap;
        //public Tilemap ShownRewardsTilemap;
        //public Tilemap AngelStartTilemap;

        public string EasySaveLevelString = "Level001";

        static List<Vector3Int> _tilesToDestroyThisFrame;
        static List<Vector3Int> _tilesImmuneThisFrame;

        public List<Vector3Int> hiddenCandyCellsList;
        public List<Tile> hiddenCandyTilesList;
        public List<bool> hiddenCandyActiveList;
        public List<bool> hiddenCandyCollectedList;

        //static string playerHitsBrickParticles = "BrickChunks";
        static string PlayerCreateBrickByCast = "BuildBlockCTF";
        static string PlayerDestroyBrickByCast = "SmokePoof";
        static string EnemyDestroyBrick = "EnemyDestroyBrick";

        public Vector3 _HalfWorldCellOffset;
        Vector3 _FullWorldCellOffset;

        static bool _doorOpen = false;

        static Vector3Int doorCellPosition;

        void Start()
        {
                //GenerateTilemapWorldPointToCellPointFile(); //Animator

                //auto grab tilemaps?
                _tilesImmuneThisFrame = new List<Vector3Int>();
                _tilesToDestroyThisFrame = new List<Vector3Int>();
                hiddenCandyActiveList = new List<bool>();
                hiddenCandyCollectedList = new List<bool>();

                Vector3Int cellPos = Vector3Int.zero;
                var worldcellposition = NonHiddenTilemap.CellToWorld(cellPos);
                cellPos.x += 1;
                cellPos.y += 1;
                var worldcellposition2 = NonHiddenTilemap.CellToWorld(cellPos);
                _FullWorldCellOffset = (worldcellposition2 - worldcellposition);
                _HalfWorldCellOffset = _FullWorldCellOffset / 2.0f;

                var _player = GameObject.Find("Player").GetComponent<Player>();
                _player.OnPlayerReset += ResetBricks;
                Player.GetPlayerBuildEvent.AddListener(ListenForPlayerAttemptedBuild);
                _player.controller.GetRoofHitEvent.AddListener(ListenForPlayerHitsRoof);
                

                _doorOpen = false;
                _instance = this;
                
                //Reveal if we can see them at the start.
                PowerToSeeStars();
                PowerToSeeCakes();
        }

        static void CheckForCompletionOfEasyNormalAndHard()
        {
                var GO = GameObject.Find("LevelManager");
                var levelloader = GO.GetComponent<LevelLoader>();
                var difficulty = levelloader.GetLevelDifficulty();
                var level = levelloader.GetLevelLevel();
                int levelnum = Int32.Parse(level);

                if (difficulty.Contains("Custom")) return;
                
                if (levelnum > 29)
                        GenericUnlockAchievement.UnlockAchievement("EasyDoneIt");
                if (levelnum > 59)
                        GenericUnlockAchievement.UnlockAchievement("NormalDoneIt");
                if (levelnum > 199)
                        GenericUnlockAchievement.UnlockAchievement("HardDoneIt");
        }

        void ListenForPlayerHitsRoof(Vector3 positionHit)
        {
                Debug.Log($" Hit Rooof point = {positionHit}");
                positionHit += Vector3.up * 0.2f;
                if(IsDestructibleTile(positionHit))
                        AttemptToBreak(positionHit, Vector2.up * 0.2f );
        }

        void ListenForPlayerAttemptedBuild(Vector3 arg0, float arg1)
        {
                Vector2 direction;
                direction.x = arg1;
                direction.y = 0;
                BuildOrDestroy(arg0, direction);
        }

        public void SetDoorCellPosition(Vector3Int pos)
        {
                doorCellPosition = pos;
        }

        void ResetBricks()
        {
                _doorOpen = false;
                LevelLoader.ReloadLevel();
                
                /*var LevelManangerObject = GameObject.Find("LevelManager");
                if(LevelManangerObject != null)
                {                                              
                     //var levelLoaderComponent = LevelManangerObject.GetComponent<LevelLoader>();
                                          
                }*/
                //LoadAllTiles(); TimedDespawner
        }

        public Vector3 GetHalfTileAmount()
        {
                Vector3Int thing1 = new Vector3Int(0, 0, 0);
                Vector3Int thing2 = new Vector3Int(1, 1, 0);
                var world1 = NonHiddenTilemap.CellToWorld(thing1);
                var world2 = NonHiddenTilemap.CellToWorld(thing2);
                var worlddiff = (world2 - world1) / 2.0f;

                return worlddiff;
        }

        public List<Vector3Int> GetActiveTiles(Tilemap tilemap)
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

        public List<Tile> GetMatchingTiles(Tilemap tilemap)
        {
                List<Tile> tempList = new List<Tile>();
                var bounds = tilemap.cellBounds;
                foreach (var pos in tilemap.cellBounds.allPositionsWithin)
                {
                        if (tilemap.HasTile(pos))
                        {
                                var curTile = tilemap.GetTile<Tile>(pos);
                                tempList.Add(curTile);
                        }
                }

                return tempList;
        }

        [ContextMenu("SaveAllTiles")]
        void SaveAllTiles()
        {
                List<Vector3Int> nonHiddenList = GetActiveTiles(NonHiddenTilemap);
                List<Tile> nonHiddenMatchList = GetMatchingTiles(NonHiddenTilemap);
                ES3.Save(EasySaveLevelString + "NonHiddenPos", nonHiddenList);
                ES3.Save(EasySaveLevelString + "NonHiddenTiles", nonHiddenMatchList);

                List<Vector3Int> hiddenRewardsList = GetActiveTiles(HiddenRewardsTilemap);
                List<Tile> hiddenRewardsMatchList = GetMatchingTiles(HiddenRewardsTilemap);
                ES3.Save(EasySaveLevelString + "HiddenRewardsPos", hiddenRewardsList);
                ES3.Save(EasySaveLevelString + "HiddenRewardsTiles", hiddenRewardsMatchList);
        }

        void LoadInTileMap(String positionsKey, String tileMatchKey, Tilemap tilemapname)
        {
                var positions = (List<Vector3Int>) ES3.Load(EasySaveLevelString + positionsKey);
                var tile = (List<Tile>) ES3.Load(EasySaveLevelString + tileMatchKey);
                tilemapname.ClearAllTiles();
                for (int i = 0; i < positions.Count; i++)
                {
                        tilemapname.SetTile(positions[i], tile[i]);
                }
        }

        [ContextMenu("LoadAllTiles")]
        public void LoadAllTilesOld()
        {
                LoadInTileMap("NonHiddenPos", "NonHiddenTiles", NonHiddenTilemap);
                LoadInTileMap("HiddenRewardsPos", "HiddenRewardsTiles", HiddenRewardsTilemap);
        }
        
//Player version feel
        public void BuildOrDestroy(Vector3 worldposition,Vector2 direction)
        {
                Vector3Int cellPos = NonHiddenTilemap.WorldToCell(worldposition);
                
                Debug.Log($"<color=green> Player Build Or destroy Cell OG ({cellPos}) dest=({IsDestructibleTile(cellPos)})</color>");
                
                cellPos.x += (int) direction.x; 
                cellPos.y += (int) direction.y;
                
                Debug.Log($"<color=green> Player Build Or destroy target Cell({cellPos}) dest=({IsDestructibleTile(cellPos)}) </color>");
                
                if(IsAnEnemyInTile(cellPos)==false)
                if(IsDestructibleTile(cellPos)==true)
                {
                        
                        NonHiddenTilemap.SetTile(cellPos, null);
                        NonHiddenTilemap.RefreshTile(cellPos);
                                
                                MasterAudio.PlaySound("PlayerBrickDestroy");
                                var worldcellposition = NonHiddenTilemap.CellToWorld(cellPos);
                                worldcellposition += _HalfWorldCellOffset;
                                PoolBoss.SpawnInPool(PlayerDestroyBrickByCast,worldcellposition,Quaternion.identity);
                                if (hiddenCandyCellsList.Contains(cellPos))
                                {
                                        if (HasStarOrCakeAlreadyBeenTaken(cellPos))
                                        {
                                                var index = hiddenCandyCellsList.IndexOf(cellPos);
                                                hiddenCandyActiveList[index] = false;
                                                hiddenCandyCollectedList[index] = true;    
                                        }
                                        else
                                        {

                                                var index = hiddenCandyCellsList.IndexOf(cellPos);
                                                var worldCellPosition = HiddenRewardsTilemap.CellToWorld(cellPos);
                                                cellPos.x += 1;
                                                cellPos.y += 1;
                                                var worldCellPosition2 = HiddenRewardsTilemap.CellToWorld(cellPos);
                                                var placed = (worldCellPosition + worldCellPosition2) / 2.0f;
                                                PoolBoss.SpawnInPool(hiddenCandyTilesList[index].name, placed,
                                                         Quaternion.identity);
                                                hiddenCandyActiveList[index] = true;
                                                hiddenCandyCollectedList[index] = false;
                                        }
                                }
                }
                else
                {
                        if(IsNonDestructibleTilePresent(cellPos)==false)
                        if (IsHiddenTileInCellPosition(cellPos) == false) //Always false @+@
                        {
                                var worldcellposition = NonHiddenTilemap.CellToWorld(cellPos);
                                worldcellposition += _HalfWorldCellOffset;
                                
                                if (IsKeyInTile(cellPos))
                                {
                                        PoolBoss.SpawnInPool(PlayerDestroyBrickByCast,worldcellposition,Quaternion.identity);
                                        MasterAudio.PlaySound("SpellEmpty");
                                        return;
                                }
                                
                                MasterAudio.PlaySound("BuildABlock");
                                PoolBoss.SpawnInPool(PlayerCreateBrickByCast, worldcellposition, Quaternion.identity);
                                NonHiddenTilemap.SetTile(cellPos, solidTileToUse);
                        }
                }
        }

        bool HasStarOrCakeAlreadyBeenTaken(Vector3Int cellPos)
        {
                       var GO = GameObject.Find("LevelManager");
                        var levelloader = GO.GetComponent<LevelLoader>();
                        var difficulty = levelloader.GetLevelDifficulty();
                        var level = levelloader.GetLevelLevel();

                        int levelnum = Int32.Parse(level);

                        var difficultyForMoney = "EasyMoney";
                        if (levelnum > 30) difficultyForMoney = "MediumMoney";
                        if (levelnum > 60) difficultyForMoney = "HardMoney";
                        
                        if(PlayerPrefs.GetInt(difficultyForMoney+level+"Money"+cellPos).Equals(1))
                        {
                                Log($"Star Already taken *");
                                return true;
                        }
                        if (PlayerPrefs.GetInt(difficulty + level + "CakeMoney" + cellPos).Equals(1))
                        {
                                Log($"Cake Already taken *");
                                return true;
                        }
                        return false;
        }

        bool IsHiddenTileInCellPosition(Vector3Int cellPos)
        {
                bool HiddenTileFound = false;
                return HiddenTileFound;
        }

        public void DestroyBrick(Vector3 worldposition)
        {
                Vector3Int cellPos = NonHiddenTilemap.WorldToCell(worldposition);
                
                if (IsDestructibleTile(cellPos))
                {
                        var worldcellposition = NonHiddenTilemap.CellToWorld(cellPos);
                        MasterAudio.PlaySound("EnemyHitsBrick");
                        worldcellposition+= _HalfWorldCellOffset;
                        PoolBoss.SpawnInPool(EnemyDestroyBrick,worldcellposition,Quaternion.identity);
                        NonHiddenTilemap.SetTile(cellPos, null);
                        NonHiddenTilemap.RefreshTile(cellPos);
                        Debug.Log($"V3V {cellPos} destroyed og = {worldposition.ToString("F3")}");
                }
                else
                {
                        /*if (false)
                        {
                                var tile = NonHiddenTilemap.GetTile<Tile>(cellPos);
                                if (tile != null)
                                        Debug.Log(
                                                $"V3V {worldposition.ToString("F3")} and {cellPos} {tile.name} not destroyed");
                                else
                                {
                                        Debug.Log(
                                                $"V3V <color=red> {worldposition.ToString("F3")} and {cellPos} no tile present </color>");
                                }
                        }*/
                }
        }

        public void DestroyBrick(Vector3Int cellPos)
        {
                if (IsDestructibleTile(cellPos))
                {
                        var worldcellposition = NonHiddenTilemap.CellToWorld(cellPos);
                        MasterAudio.PlaySound("EnemyHitsBrick");
                        worldcellposition+= _HalfWorldCellOffset;
                        PoolBoss.SpawnInPool(EnemyDestroyBrick,worldcellposition,Quaternion.identity);
                        NonHiddenTilemap.SetTile(cellPos, null);
                        NonHiddenTilemap.RefreshTile(cellPos);
                }
                else
                {
                        var tile = NonHiddenTilemap.GetTile<Tile>(cellPos);
                        if(tile!=null)
                                Debug.Log($" {cellPos} {tile.name} not destroyed");
                        else
                        {
                                Debug.Log($"<color=red> {cellPos} no tile present </color>");
                        }
                }
        }
        
        public bool IsDestructibleTile(Vector3 worldCellPos)
        {
                bool canDestruct = IsDestructibleTile(NonHiddenTilemap.WorldToCell(worldCellPos));
                return canDestruct;
        }

        public bool IsDestructibleTile(Vector3Int cellPos)
        {
                bool canDestruct = false;

                var tile = NonHiddenTilemap.GetTile<Tile>(cellPos);
                if (tile != null)
                {
                    bool contains = tile.name.IndexOf("desert", StringComparison.OrdinalIgnoreCase) >= 0;
                        
                    if (contains)
                    {
                        canDestruct = true;
                    }
                }

                return canDestruct;
        }

        public bool IsNonDestructibleTilePresent(Vector3Int cellPos)
        {
                bool canDestruct = false;

                var tile = NonHiddenTilemap.GetTile<Tile>(cellPos);
                if (tile != null)
                {
                        bool contains = tile.name.IndexOf("static", StringComparison.OrdinalIgnoreCase) >= 0;
                        
                        if (tile.name.Contains("Window")) contains = true;

                        if (contains)
                        {
                                canDestruct = true;
                        }
                }

                return canDestruct;
        }

        bool CanPlaceTile(Vector3Int cellPos)
        {
                bool canPlace = true;
                
                if (NonHiddenTilemap.GetTile(cellPos) != null)
                        canPlace = false;
                
                return canPlace;
        }
        
        bool IsAnEnemyInTile(Vector3Int cellPos)
        {
                bool isEnemyInTile = false;
                var fullsize = (Vector2)_HalfWorldCellOffset;
                Vector2 boxsize = fullsize;
                
                var worldpos = NonHiddenTilemap.CellToWorld(cellPos);
                var worldpos4 = (Vector2) worldpos + boxsize / 2;
                var collisionbox = Physics2D.OverlapBoxAll(worldpos4, boxsize, 0f);
                if (collisionbox.Length > 0)
                {
                        foreach (var thing in collisionbox)
                        {
                                if (thing.GetComponent<KillThePlayer>()!=null)
                                {
                                        isEnemyInTile = true;
                                }
                        }
                }

                return isEnemyInTile;
        }
        
        bool IsKeyInTile(Vector3Int cellPos)
        {
                var keyObj = GameObject.FindWithTag("Key");
                if (keyObj == null) return false;
                
                var keycellPos = NonHiddenTilemap.WorldToCell(keyObj.transform.position);
                //Log($" <color=red> should MATCH {keycellPos} and {cellPos} </color>");
                return keycellPos == cellPos;
        }

        public void ReverseKey(Vector3 worldposition,Vector2 direction)
        {
                Vector3Int cellPos = NonHiddenTilemap.WorldToCell(worldposition);
                cellPos.x += (int) direction.x; 
                cellPos.y += (int) direction.y;
                //Debug.Log($"Reverse Key? Cell({cellPos})");
                
                if (IsKeyInTile(cellPos))
                {
                        var keyObj = GameObject.FindWithTag("Key");
                        if (keyObj)
                        {
                                var scriptOnKey = keyObj.GetComponent<ActivateSpecialKey>();
                                scriptOnKey.ForceKeyFlip();
                        }
                }
        }

        public void AttemptToBreak(Vector3 worldposition,Vector2 direction)
        {
                worldposition.x += direction.x;
                worldposition.y += direction.y;
                Vector3Int cellPos = NonHiddenTilemap.WorldToCell(worldposition);
                if (IsDestructibleTile(cellPos))
                {
                        if (_tilesToDestroyThisFrame.Contains(cellPos) == false)
                        {
                                MasterAudio.PlaySound("hit_01");
                                _tilesToDestroyThisFrame.Add(cellPos);
                        }
                }
        }

        void Update()
        {
                if(_tilesToDestroyThisFrame!=null)
                        if (_tilesToDestroyThisFrame.Count > 0)
                        {
                                StartCoroutine(DestroyTheBricks());
                        }
        }
        
        IEnumerator DestroyTheBricks()
        {
                yield return null;
                for (int n = 0; n < _tilesToDestroyThisFrame.Count; n++)
                {
                        foreach (var tileposition in _tilesToDestroyThisFrame)
                        {
                                if (NonHiddenTilemap.GetTile(tileposition) != null)
                                {
                                        var tileName = NonHiddenTilemap.GetTile(tileposition).name;

                                        if (!tileName.Contains("crack"))
                                        {
                                                Tile crackedTile = GetCrackedVersionOfTileByTileName(tileName);
                                                NonHiddenTilemap.SetTile(tileposition, crackedTile);
                                                _tilesImmuneThisFrame.Add(tileposition);
                                                StartCoroutine(ImmunityTimer(tileposition));
                                        }
                                        else
                                        {
                                                if (_tilesImmuneThisFrame.Contains(tileposition) == false)
                                                {
                                                        NonHiddenTilemap.SetTile(tileposition, null);
                                                        NonHiddenTilemap.RefreshTile(tileposition);
                                                        var worldcellposition = NonHiddenTilemap.CellToWorld(tileposition);
                                                        worldcellposition += _HalfWorldCellOffset;
                                                        PoolBoss.SpawnInPool("BrickChunks",worldcellposition,Quaternion.FromToRotation(Vector3.zero,Vector3.right));//.FromToRotation(Vector3.zero, Vector3.up));
                                                }
                                                else
                                                {
                                                        //Debug.Log($"Found immune tile {tileposition}");
                                                }
                                        }
                                }
                        }
                }
                yield return null;
                _tilesToDestroyThisFrame.Clear();
        }

        //I hate the tile system
        Tile GetCrackedVersionOfTileByTileName(string tileName)
        {
                Debug.Log($"searching for {tileName}");
                Tile testMatchBrokenTile = brokenTileToUse; //default
                
                string brokenTileName = tileName + "cracked";
                Debug.Log($"means we searching for {brokenTileName}");
                
                testMatchBrokenTile = brokenTileToUse;
                if (brokenTileName == testMatchBrokenTile.name) return testMatchBrokenTile;
                testMatchBrokenTile = brokenTileToUse01;
                if (brokenTileName == testMatchBrokenTile.name) return testMatchBrokenTile;
                testMatchBrokenTile = brokenTileToUse02;
                if (brokenTileName == testMatchBrokenTile.name) return testMatchBrokenTile;
                testMatchBrokenTile = brokenTileToUse03;
                if (brokenTileName == testMatchBrokenTile.name) return testMatchBrokenTile;
                testMatchBrokenTile = brokenTileToUse04;
                if (brokenTileName == testMatchBrokenTile.name) return testMatchBrokenTile;
                testMatchBrokenTile = brokenTileToUse05;
                if (brokenTileName == testMatchBrokenTile.name) return testMatchBrokenTile;
                
                testMatchBrokenTile = brokenTileToUseCat;
                if (brokenTileName == testMatchBrokenTile.name) return testMatchBrokenTile;

                return testMatchBrokenTile;
        }

        IEnumerator ImmunityTimer(Vector3Int immuneTile)
        {
            yield return new WaitForSeconds(0.2f);
            _tilesImmuneThisFrame.Remove(immuneTile);
        }
        
        static void SpawnDoorPortals()
        {
                var brickmap = GameObject.FindWithTag("BrickMap").GetComponent<BrickMap>();
                var cellloc = BrickMap.FindDoorOnMap();
                Vector3 worldPlace= brickmap.NonHiddenTilemap.CellToWorld(cellloc);
                worldPlace+= brickmap._HalfWorldCellOffset;
            
                Log($"<color=red> On Trigger enter name contains key</color>");
            
                var keyObj = GameObject.FindWithTag("Key");
                if (keyObj)
                {
                        var keyScript = keyObj.GetComponent<ActivateSpecialKey>();
                        if (keyScript)
                        {
                                Log($"<color=red> KEYSCRIPT FOUND </color>");
                                PoolBoss.SpawnInPool(keyScript.IsHiddenLevelActive() ? "DoorPowerAuraSpecial" : "DoorPowerAuraV2",
                                        worldPlace, Quaternion.identity);
                        }
                }
                else
                {
                        Log($"<color=red> TAG NOT FOUND </color>");
                }
        }

        public static void CollectKey()
        {
                Log($" COLLECT KEY!");
                SpawnDoorPortals();
                OpenDoorTile();
        }

        public static Vector3Int FindDoorOnMap()
        {
                Vector3Int maploc = doorCellPosition;
                return maploc;
        }
        static void OpenDoorTile()
        {
                var cellloc = FindDoorOnMap();
                Vector3 worldPlace= _instance.NonHiddenTilemap.CellToWorld(cellloc);
                MasterAudio.PlaySound("KeyPickAndDoorOpen");
                worldPlace+= _instance._HalfWorldCellOffset;
                
                PoolBoss.SpawnInPool("DoorOpen",worldPlace,Quaternion.identity);
                _doorOpen = true;
        }

        public static bool GoThroughDoor()
        {
                if (_doorOpen)
                {
                        Debug.Log("<color=green> Open Portal Entered </color>");
                        PoolBoss.DespawnAllPrefabs();
                        var thing = _instance.StartCoroutine(_instance.WaitForDespawn());
                        Debug.Log("<color=green> Done with loading all tiles? </color>");

                        if (LevelLoader.IsEditorModeLevel() is false)
                        {
                                CheckForCompletionOfEasyNormalAndHard();
                                if (InGameCountDownTimer._timerAmountLeft > 90)
                                {
                                        if(!DifficultyLevel.GetDifficulty().Contains("Custom"))
                                        GenericUnlockAchievement.UnlockAchievement("TimeTravel");
                                }


                                Log($"Power Count {PowerManager.PowerCountLeft()} setting","cyan");
                                PowerManager.SetPowerCountInPrefs();
                        }
                        else
                        {
                                Log($"Power Count {PowerManager.PowerCountLeft()} not setting as editor mode","cyan");
                        }
                }
                
                return _doorOpen;
        }

        IEnumerator WaitForDespawn()
        {
                bool notdoneyet = true;
                while (notdoneyet)
                {
                        Debug.Log($"Waiting...");
                        yield return new WaitForSeconds(0.1f);
                        var GO = GameObject.Find("LevelManager");
                        var levelloader = GO.GetComponent<LevelLoader>();
                        
                        //Save Progress to playerprefs via EasySave
                        var difficulty = levelloader.GetLevelDifficulty();
                        var level = levelloader.GetLevelLevel();
                        var levelnum = Int32.Parse(level);
                        var currentClearedLevel = ES3.Load("UsernameLevel"+difficulty,0); //?
                        if (currentClearedLevel < levelnum)
                        {
                                //Only save if bigger.
                                ES3.Save("UsernameLevel" + difficulty, levelnum);
                        }
                        
                        
                        levelloader.LoadNextLevel(ActivateSpecialKey.HiddenLevelActive);
                        yield return null;
                        notdoneyet = false;
                        Debug.Log($"All done ?");
                }
        }


        void ActivatePowerAtStarPositions()
        {
                Log($"<color=red>Looking for stars to reveal</color>");
                var hiddenTiles =  GetActiveTiles(HiddenRewardsTilemap);
                foreach (var hiddentilepos in hiddenTiles)
                {
                        var tiledetails = HiddenRewardsTilemap.GetTile<Tile>(hiddentilepos);
                        if (HasStarOrCakeAlreadyBeenTaken(hiddentilepos) == false)
                        {
                                if (tiledetails.name.Contains("Star"))
                                {
                                        var index = hiddenCandyCellsList.IndexOf(hiddentilepos);

                                        var worldCellPosition = HiddenRewardsTilemap.CellToWorld(hiddentilepos);
                                        var tilespos = hiddentilepos;
                                        tilespos.x += 1;
                                        tilespos.y += 1;
                                        var worldCellPosition2 = HiddenRewardsTilemap.CellToWorld(tilespos);

                                        var placed = (worldCellPosition + worldCellPosition2) / 2.0f;
                                        PoolBoss.SpawnInPool(hiddenCandyTilesList[index].name, placed,
                                                Quaternion.identity);
                                        hiddenCandyActiveList[index] = true;
                                        hiddenCandyCollectedList[index] = false;

                                        PoolBoss.SpawnInPool(PlayerDestroyBrickByCast, placed,
                                                Quaternion.identity);
                                }
                        }
                }
        }
        
        void ActivatePowerAtCakePositions()
        {
                var hiddenTiles =  GetActiveTiles(HiddenRewardsTilemap);
                foreach (var hiddentilepos in hiddenTiles)
                {
                        var tiledetails = HiddenRewardsTilemap.GetTile<Tile>(hiddentilepos);
                        if (HasStarOrCakeAlreadyBeenTaken(hiddentilepos) == false)
                        {
                                if (tiledetails.name.Contains("ake_128x128"))
                                {
                                        var index = hiddenCandyCellsList.IndexOf(hiddentilepos);

                                        var worldCellPosition = HiddenRewardsTilemap.CellToWorld(hiddentilepos);
                                        var tilespos = hiddentilepos;
                                        tilespos.x += 1;
                                        tilespos.y += 1;
                                        var worldCellPosition2 = HiddenRewardsTilemap.CellToWorld(tilespos);

                                        var placed = (worldCellPosition + worldCellPosition2) / 2.0f;

                                        Log(
                                                $"<color=red>Cake power attempts to spawn {hiddenCandyTilesList[index].name} at {tilespos}</color>");
                                        PoolBoss.SpawnInPool(hiddenCandyTilesList[index].name, placed,
                                                Quaternion.identity);
                                        hiddenCandyActiveList[index] = true;
                                        hiddenCandyCollectedList[index] = false;

                                        PoolBoss.SpawnInPool(PlayerDestroyBrickByCast, placed,
                                                Quaternion.identity);
                                }
                        }
                }
        }
        
        public void PowerToSeeStars()
        {
                if (Player.CanPlayerSeeStars())
                {
                        if (LevelLoader.IsEditorModeLevel() == false)
                        {
                                StartCoroutine(WaitFor3SecondsRevealStars());
                        }
                }
        }
        
        public void PowerToSeeCakes()
        {
                if (Player.CanPlayerSeeCakes())
                        if(LevelLoader.IsEditorModeLevel()==false)
                                StartCoroutine(WaitFor3SecondsRevealCakes());
        }


        IEnumerator WaitFor3SecondsRevealStars()
        {
                yield return new WaitForSeconds(2.5f);
                ActivatePowerAtStarPositions();
        }
        
        IEnumerator WaitFor3SecondsRevealCakes()
        {
                yield return new WaitForSeconds(2.5f);
                ActivatePowerAtCakePositions();
        }
}
