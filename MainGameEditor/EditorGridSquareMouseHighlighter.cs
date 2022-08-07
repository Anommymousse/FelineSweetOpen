using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class EditorGridSquareMouseHighlighter : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject HighlightParticleEffect;

    public GameObject spriteCursor;
    
    Vector3 mousePos3d;
    Vector3 worldmousePos3d;
    Vector3Int cellPositionReturned;
    Vector3 cellworldposition;
    Vector3 gridadjustment;
    Vector3 halfcellAdjust; 

    GameObject cathighlighter;
    // Start is called before the first frame update
    void Start()
    {
        cathighlighter = Instantiate(HighlightParticleEffect, Vector3.zero, Quaternion.identity);
        var tf = GameObject.Find("Grid");
        gridadjustment = tf.transform.position;
        Vector3Int point = Vector3Int.zero;
        var worldPosition = tilemap.CellToWorld(point);
        point.x = 1;
        point.y = 1;
        var worldPosition1 = tilemap.CellToWorld(point);

        halfcellAdjust = (worldPosition1 - worldPosition) / 2.0f;


    }
    
    Vector3Int GetCellMousePositionFromSpriteCursor()
    {
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

    // Update is called once per frame
    void Update()
    {
        //is cursor off
        //var needToDrawCircle = TurnOffMouseCursor.IsMouseVisible();
        //if (needToDrawCircle == false) return;

        if (spriteCursor == null) return;

        mousePos3d = Mouse.current.position.ReadValue();
        mousePos3d.z = -Camera.main.transform.position.z;
        worldmousePos3d = Camera.main.ScreenToWorldPoint(mousePos3d);
        var cellPosition = tilemap.WorldToCell(worldmousePos3d);
        cellPosition.x -= 1;
        cellPosition.y -= 3;

        cellPosition = GetCellMousePositionFromSpriteCursor();

        if ((cellPosition.x > 3) || (cellPosition.x < -12) || (cellPosition.y > -1) || (cellPosition.y < -13))
        {
            cathighlighter.transform.position = Vector3.zero;
        }
        else
        {
            var worldPosition = tilemap.CellToWorld(cellPosition);
            cellworldposition = worldPosition;
            var tile = tilemap.GetTile<Tile>(cellPosition);
            cellPositionReturned = cellPosition;
            cathighlighter.transform.position = worldPosition - halfcellAdjust; 
        }
    }
}
