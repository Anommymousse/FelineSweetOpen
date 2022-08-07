using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using Spine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyVerticalThrowBrickDown : MonoBehaviour
{
    BrickMap _brickMapRef;
    Tilemap _mapRef;
    Vector3 _halfBrick;
    float coolDownTimer;
    
    void OnEnable()
    {
        var root = GameObject.Find("TilesBoss");
        _brickMapRef = root.GetComponentInChildren<BrickMap>();
        _mapRef = _brickMapRef.NonHiddenTilemap;
        
        _halfBrick = _brickMapRef.NonHiddenTilemap.WorldToCell(Vector3.zero) -
                     _brickMapRef.NonHiddenTilemap.WorldToCell(Vector3.one);
        _halfBrick /= 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        var batsCell = GetCellTile();
        var playerCell = GetPlayerCellTile();
        if (Time.time > coolDownTimer)
        {
            if ((batsCell.x == playerCell.x) && (batsCell.y > playerCell.y))
            {
                coolDownTimer = Time.time + 2.0f;
                var cellPosition = _mapRef.WorldToCell(transform.position);
                var worldpos = _mapRef.CellToWorld(cellPosition);
                worldpos += _halfBrick;
                worldpos.y += 1.0f;
                PoolBoss.SpawnInPool("BatProjectile", worldpos, Quaternion.identity);
            }
        }
    }

    Vector3Int GetPlayerCellTile()
    {
        var temp = _mapRef.WorldToCell(Player.GetWorldLocation());
        temp.x += 1;//?
        return temp;
    }

    Vector3Int GetCellTile()
    {
        var batpos = transform.position;
        return _mapRef.WorldToCell(batpos);
    }
}
