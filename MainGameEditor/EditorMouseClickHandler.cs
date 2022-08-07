using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class EditorMouseClickHandler : MonoBehaviour,IPointerClickHandler
{
    public Tile AngelTile;
    public Tilemap _tilemap;
    public Tilemap _tilemapHidden;
    EditorReplaceMouseCursor _refToMainCursorChanger;
    public BricksScriptable _BricksScriptable;
    List<string> _OnePerLevelItems= new List<string>();

    public GameObject spriteCursor;
    
    void Awake()
    {
        _refToMainCursorChanger = GetComponent<EditorReplaceMouseCursor>();
        _OnePerLevelItems.Clear();
        _OnePerLevelItems.Add("Angel");
        _OnePerLevelItems.Add("AngelBook");
        _OnePerLevelItems.Add("key128x128");
        _OnePerLevelItems.Add("PlayerStartPos");
        _OnePerLevelItems.Add("closeddoor");
    }

    bool IsOnePerLevel(string testItem)
    {
        if (_OnePerLevelItems.Contains(testItem))
            return true;
        return false;
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

    Vector3Int GetCellMousePositionFromSpriteCursor()
    {
        if(spriteCursor==null) return Vector3Int.zero;
        
        var sc = spriteCursor.GetComponent<ManualCursorMouseAndGamepad>();
        Vector3 spriteMouse = sc.GetManualCursorCoords();
        //Tilemap
        //xspan -13 <---> 10/11?
        //yspan -14 <---- > 0  
        //valid xspan = -12 <---> 3
        //valid yspan = -1 <----> -13
        
        //Screenspace (note always 1920x1080)
        //xspan = 0 <----> 1920
        //yspan = 0 <----> 1080
        
        //So
        // ssx of 0 = -12, ssx of 1920 = 3
        // ssy of 0 = -13, ssy of 1080 = -1
        
        //maffs
        var perssx = spriteMouse.x / Screen.width;//1920.0f;
        var perssy = spriteMouse.y / Screen.height;//1080.0f;

        //Approx not exact :S
        var percellx = perssx * 24.0f;
        var percelly = perssy * 14.0f;

        var cellx = percellx;
        var celly = percelly;
        
        Vector3Int cellpos = new Vector3Int((int)cellx, (int)celly, 0);
        
        var cellPositionRaw = cellpos;

        var cellPosition = cellPositionRaw + new Vector3Int(-13, -13, 0);

        return cellPosition;

    }

    
    public void OnClickyClick()
    {
        Vector3 mousePos3d = Mouse.current.position.ReadValue();
        mousePos3d.z = -Camera.main.transform.position.z;

        var cellPosition = GetCellMousePositionFromSpriteCursor();

        //var worldmousePos3d = Camera.main.ScreenToWorldPoint(spriteMouse);
        //var cellPosition = _tilemap.WorldToCell(worldmousePos3d);
        
        //Debug.Log($"spritemouse = {spriteMouse} wp = {worldmousePos3d} and cell non adj{cellPosition}");
        
        //cellPosition.x -= 1;//?
        //cellPosition.y -= 3;//?
        
        Debug.Log($"OnPointerClick- EditorMouseClickHandler - cellpos = {cellPosition}");

        if ((cellPosition.x > 3) || (cellPosition.x < -12) || (cellPosition.y > -1) || (cellPosition.y < -13))
        {
            Debug.Log("Pressed outside of range");
        }
        else
        {
            String nameofCursor = _refToMainCursorChanger.GetCurrentCursor().name;
            var tile = FindTileMatchingString(nameofCursor);
            if (tile != null)
            {
                Debug.Log($"Setting cell{cellPosition} to {nameofCursor}");
                cellPosition.x += 1;
                cellPosition.y += 3;
                
                
                var presentTile = _tilemap.GetTile<Tile>(cellPosition);
                if (nameofCursor == "Angel")
                    presentTile = _tilemapHidden.GetTile<Tile>(cellPosition);
                if(nameofCursor.Contains("Gold_Star_128x128a"))
                    presentTile = _tilemapHidden.GetTile<Tile>(cellPosition);
                
                if (presentTile == null)
                {
                    if(nameofCursor == "Angel")
                        SetTile(cellPosition, nameofCursor, nameofCursor == "Angel");
                    else
                        SetTile(cellPosition, nameofCursor, nameofCursor == "Gold_Star_128x128a");
                }
                else
                {
                    if (presentTile.name != nameofCursor)
                    {
                        SetTile(cellPosition, nameofCursor, nameofCursor == "Angel");
                    }
                    else
                    {
                        if (nameofCursor is "Angel" or "Gold_Star_128x128a")
                        {
                            _tilemapHidden.SetTile(cellPosition,null);
                            _tilemapHidden.RefreshTile(cellPosition);
                        }
                        else
                        {
                            _tilemap.SetTile(cellPosition, null);
                            _tilemap.RefreshTile(cellPosition);
                        }
                    } 
                }
            }
        }
    }

    public void SetTile(Vector3Int cellPosition, string nameofCursor, bool hidden=false)
    {
        if (IsOnePerLevel(nameofCursor))
        {
            FindAndClearTile(nameofCursor);
            if (hidden == false)
            {
                _tilemap.SetTile(cellPosition, FindTileMatchingString(nameofCursor));
                _tilemap.RefreshTile(cellPosition);
            }
            else
            {
                _tilemapHidden.SetTile(cellPosition, FindTileMatchingString(nameofCursor));
                _tilemapHidden.RefreshTile(cellPosition);
            }
        }
        else
        {
            if (hidden == false)
            {
                _tilemap.SetTile(cellPosition, FindTileMatchingString(nameofCursor));
                _tilemap.RefreshTile(cellPosition);
            }
            else
            {
                _tilemapHidden.SetTile(cellPosition, FindTileMatchingString(nameofCursor));
                _tilemapHidden.RefreshTile(cellPosition);
            }
        }
    }

    void FindAndClearTile(string nameToClear)
    {
        var whichTilemap = _tilemap;
        if (nameToClear == "Angel")
            whichTilemap = _tilemapHidden;
        
        if (nameToClear == "Gold_Star_128x128a")
            whichTilemap = _tilemapHidden;
        
        //Find instances or scroll through checking.
        var activeTiles= GetActiveTiles(whichTilemap);
        foreach (var tilepos in activeTiles)
        {
            var tiledata = whichTilemap.GetTile<Tile>(tilepos);
            if (tiledata != null)
            {
                if (tiledata.name == nameToClear)
                {
                    whichTilemap.SetTile(tilepos, null);
                    whichTilemap.RefreshTile(tilepos);
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePos3d = Mouse.current.position.ReadValue();
        mousePos3d.z = -Camera.main.transform.position.z;
        var worldmousePos3d = Camera.main.ScreenToWorldPoint(mousePos3d);
        var cellPosition = _tilemap.WorldToCell(worldmousePos3d);
        cellPosition.x -= 1;
        cellPosition.y -= 3;
        
        Debug.Log("OnPointerClick- EditorMouseClickHandler");

        if ((cellPosition.x > 3) || (cellPosition.x < -12) || (cellPosition.y > -1) || (cellPosition.y < -13))
        {
            Debug.Log("Pressed outside of range");
        }
        else
        {
            String nameofCursor = _refToMainCursorChanger.GetCurrentCursor().name;
            var tile = FindTileMatchingString(nameofCursor);
            if (tile != null)
            {
                Debug.Log($"Setting cell{cellPosition} to {nameofCursor}");
                _tilemap.SetTile(cellPosition, FindTileMatchingString(nameofCursor));
                _tilemap.RefreshTile(cellPosition);
            }
            else
            {
                Debug.Log($"Not found in tile list {nameofCursor}");
            }
        }
        
    }

    Tile FindTileMatchingString(string nameofCursor)
    {
        if (nameofCursor.ToLower().Contains("angelbook"))
        {
            return _BricksScriptable.TiledBricks.Find(d => d.name == nameofCursor);
        }
        else
        {
            if (nameofCursor.ToLower().Contains("angel"))
            {
                return AngelTile;
            }
        }

    // var thingy = _BricksScriptable.TiledBricks.Find(d => d.name == nameofCursor);
    // if(thingy!=null)
    //     Debug.Log($" {thingy.name}");
    // else
    //     Debug.Log($" {nameofCursor} = null in scriptable :(");

    return _BricksScriptable.TiledBricks.Find(d => d.name == nameofCursor);
        
        //return(_tilesList.Find(d => d.name == nameofCursor));
    }
}
