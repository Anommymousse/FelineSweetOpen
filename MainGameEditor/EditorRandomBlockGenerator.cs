using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EditorRandomBlockGenerator : MonoBehaviour
{
    public Tilemap visibleTilemapRef;
    public Tile staticTile;
    public Tile destroTile;
    //string nondestoBricks = "static_bricks_01";
    //string destoBricks ="deserted_bricks_01";

    public void GenerateNewTiles()
    {
        
        visibleTilemapRef.ClearAllTiles();
        OutSideBlocks();
        ProcessLists();
    }

    void OutSideBlocks()
    {
        Vector3Int v3 = Vector3Int.zero;
        //int z = 0;
        for (int x = -12; x < 12; x++)
        {
            v3.x = x;
            v3.y = -11;
            visibleTilemapRef.SetTile(v3, staticTile);
        }

        for (int x = -12; x < 12; x++)
        {
            v3.x = x;
            v3.y = 3;
            visibleTilemapRef.SetTile(v3, staticTile);
        }

        for (int y = -11; y < 4; y++)
        {
            v3.x = -12;
            v3.y = y;
            visibleTilemapRef.SetTile(v3, staticTile);
        }

        for (int y = -11; y < 4; y++)
        {
            v3.x = 5;
            v3.y = y;
            visibleTilemapRef.SetTile(v3, staticTile);
        }

        for (int y = -11; y < 4; y++)
        {
            v3.x = 12;
            v3.y = y;
            visibleTilemapRef.SetTile(v3, staticTile);
        }
    }
    //Modes : 
    //0: Hrun
    //1: Vrun
    //2: Vrun Half
    //3: Vrun Box
    //4: Blank Run
    //5: Top drop

    void Hrun(int y)
    {
        int solidOrDestro = Random.Range(0, 2);
        int startOrEnd = Random.Range(0, 2);
        int howmanygaps = Random.Range(0, 4);
        
        Debug.Log(($"hrun = {y}"));

        List<int> gaps = new List<int>();
        for (int x = 0; x < howmanygaps; x++)
            gaps.Add(Random.Range(-10, 4));

        Tile tileToPlace;
        tileToPlace = destroTile;
        if (solidOrDestro == 0)        
            tileToPlace = staticTile;

        Vector3Int cellpos=Vector3Int.zero;
        
        cellpos.y = y;
        for(int x=-11;x<5;x++)
        {
            cellpos.x = x;
            visibleTilemapRef.SetTile(cellpos, tileToPlace);     
        }

                
        if (startOrEnd < 1)
        {
            Debug.Log($"start = 0 ");
            gaps.Add(-11);
            gaps.Add(-10);
        }
        else
        {
            Debug.Log($"start = 1 ");
            gaps.Add(3);
            gaps.Add(4);                       
        }
        
                       
        foreach (var gap in gaps)
        {
           cellpos.x = gap;
           visibleTilemapRef.SetTile(cellpos, null);
        }                                        
    }

    void Vrun(int x)
    {
        int solidOrDestro = Random.Range(0, 2);
        int startOrEnd = Random.Range(0, 2);
        int howmanygaps = Random.Range(1, 6);
        
        Debug.Log(($"hrun = {x}"));

        List<int> gaps = new List<int>();
        for (int y = 0; y < howmanygaps; y++)
            gaps.Add(Random.Range(-10, 3));

        Tile tileToPlace;
        tileToPlace = destroTile;
        if (solidOrDestro == 0)        
            tileToPlace = staticTile;

        Vector3Int cellpos=Vector3Int.zero;
        
        cellpos.x = x;
        for(int y=-10;y<3;y++)
        {
            cellpos.y = y;
            visibleTilemapRef.SetTile(cellpos, tileToPlace);     
        }
                
        if (startOrEnd < 1)
        {
            Debug.Log($"start = 0 ");
            gaps.Add(-9);
            gaps.Add(-10);
        }
        else
        {
            Debug.Log($"start = 1 ");
            gaps.Add(1);
            gaps.Add(2);                       
        }
        
                       
        foreach (var gap in gaps)
        {
            cellpos.y = gap;
            visibleTilemapRef.SetTile(cellpos, null);
        }                                        
    }

    
    // Start is called before the first frame update
    void ProcessLists()
    {                
        var result = Random.Range(0, 4);
        if (result > 0)
        {
            List<int> HrunList = new List<int>();
            for (int y = -11; y < -2;)
            {
                y += Random.Range(2, 5);
                HrunList.Add(y);
            }
            foreach (var hrun in HrunList)
            {
                Hrun(hrun);
            }            
        }
        else
        {
            List<int> VrunList = new List<int>();
            for (int x = -10; x < 1;)
            {
                x += Random.Range(2, 5);
                VrunList.Add(x);
            }
            foreach (var hrun in VrunList)
            {
                Vrun(hrun);
            }            

        }


        
        /*foreach (var half in halfrunlist)
        {
            HalfRun(half);
        }*/
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
    }
}
