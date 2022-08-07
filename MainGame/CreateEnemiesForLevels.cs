using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class CreateEnemiesForLevels : MonoBehaviour
{
    public GameObject enemyType01;
    public GameObject enemyTypePumpkinHorizontal;
    public GameObject enemyTypePumpkinVertical;
    public GameObject tilemaps;
    
    Vector3 _halfWorldPosition;
    
    void AssignHalfTileOffset()
    {
        var things = tilemaps.GetComponent<BrickMap>();
        Vector3Int Position0 = new Vector3Int(0,0,0);
        Vector3Int Position1 = new Vector3Int(1,1,0);
        Vector3 worldposition1 = things.NonHiddenTilemap.CellToWorld(Position0);
        Vector3 worldposition2 = things.NonHiddenTilemap.CellToWorld(Position1);
        _halfWorldPosition = (worldposition2 - worldposition1)/2.0f;
    }
    void CreateEnemyAtCellPosition(GameObject enemyType, Vector3Int cellposition)
    {
        Vector3 worldposition1 = tilemaps.GetComponent<BrickMap>().NonHiddenTilemap.CellToWorld(cellposition);
        var instantce1 = Instantiate(enemyType, worldposition1+_halfWorldPosition, Quaternion.identity);
    }
    
    void CreateEnemyAtCellPositionNoAdjustment(GameObject enemyType, Vector3Int cellposition)
    {
        Vector3 worldposition1 = tilemaps.GetComponent<BrickMap>().NonHiddenTilemap.CellToWorld(cellposition);
        worldposition1.x += _halfWorldPosition.x;
        var instantce1 = Instantiate(enemyType, worldposition1, Quaternion.identity);
    }

    /*void Start()
    {
        return;
        AssignHalfTileOffset();

        var levelstring = GameObject.Find("TilesBoss").GetComponent<BrickMap>().EasySaveLevelString;
        
        //Level 1
        if (levelstring == "Level001")
        {
            Vector3Int enemypos01 = new Vector3Int(1, -2, 0);
            CreateEnemyAtCellPosition(enemyType01, enemypos01);

            Vector3Int enemypos04 = new Vector3Int(-4, -2, 0);
            CreateEnemyAtCellPositionNoAdjustment(enemyTypePumpkinHorizontal, enemypos04);
            Vector3Int enemypos05 = new Vector3Int(3, -1, 0);
            CreateEnemyAtCellPositionNoAdjustment(enemyTypePumpkinVertical, enemypos05);
        }
        else
        {
            Vector3Int enemypos01 = new Vector3Int(1, -2, 0);
            CreateEnemyAtCellPosition(enemyType01, enemypos01);
            Vector3Int enemypos02 = new Vector3Int(-3, 0, 0);
            CreateEnemyAtCellPosition(enemyType01, enemypos02);
            Vector3Int enemypos03 = new Vector3Int(-5, 0, 0);
            CreateEnemyAtCellPosition(enemyType01, enemypos03);

            Vector3Int enemypos04 = new Vector3Int(1, -2, 0);
            CreateEnemyAtCellPositionNoAdjustment(enemyTypePumpkinHorizontal, enemypos04);
        }

    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
