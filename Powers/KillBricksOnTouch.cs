using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static MyExtensions.MyExtensions;
public class KillBricksOnTouch : MonoBehaviour
{
    BrickMap _brickMapRef;
    
    void Start()
    {
        _brickMapRef = GameObject.Find("TilesBoss").GetComponent<BrickMap>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("NonHidden"))
        {
            
            var position = _brickMapRef.NonHiddenTilemap.WorldToCell(other.transform.position);
            var tileName = _brickMapRef.NonHiddenTilemap.GetTile<Tile>(position);
            if(tileName)
                Log($" position = {position} name = {tileName} ");
            else
            {
                Log($" position = {position} name = null? ");
            }
            
            if (tileName.name.ToLower().Contains("desert"))
            {
                Log($" Destroy et ");
                _brickMapRef.DestroyBrick(position);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.collider.name.Contains("NonHidden"))
        {
            var position = _brickMapRef.NonHiddenTilemap.WorldToCell(other.transform.position);
            var tileName = _brickMapRef.NonHiddenTilemap.GetTile<Tile>(position);

            if(tileName)
                Log($" position = {position} name = {tileName} ");
            else
            {
                Log($" position = {position} name = null? ");
            }
            
            if (tileName.name.ToLower().Contains("desert"))
            {
                _brickMapRef.DestroyBrick(position);
            }
        }
        
    }
}
