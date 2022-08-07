using System;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using UnityEngine;

public class CollectAngelActivator : MonoBehaviour
{
    BrickMap _brickMapObject;
    Vector3Int cellLocation;
    Vector3 worldLocationOfAngel;

    Vector3 GetAngelWorldSpawnLocation()
    {
        var tileWorldLocations = new List<Vector3>();

        foreach (var pos in _brickMapObject.HiddenRewardsTilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = _brickMapObject.HiddenRewardsTilemap.CellToWorld(localPlace);
            if (_brickMapObject.HiddenRewardsTilemap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
            }
        }

        if (tileWorldLocations.Count > 0)
            return tileWorldLocations[0]+ _brickMapObject.GetHalfTileAmount();
        else
            return Vector3.zero;
    }
    
    void Awake()
    {
        var temp = GameObject.Find("TilesBoss");
        _brickMapObject = temp.GetComponent<BrickMap>();
        
        var _player = GameObject.Find("Player").GetComponent<Player>();
        _player.OnPlayerLevelChange += GetNewLocationForAngel;
        
        worldLocationOfAngel = GetAngelWorldSpawnLocation();
    }

    void GetNewLocationForAngel()
    {
        worldLocationOfAngel = GetAngelWorldSpawnLocation();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            HandleCollectingActivator();
            PoolBoss.Despawn(transform);
        }
    }

    void HandleCollectingActivator()
    {
        //Assumes 1
        PoolBoss.SpawnInPool("Angel",worldLocationOfAngel,Quaternion.identity);
        MasterAudio.PlaySound("Positive Effect 1_5");
    }
}