using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using DG.Tweening;
//using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static MyExtensions.MyExtensions;
public class BombMain : MonoBehaviour
{
    Vector3 speedInDirection;
    public float bombTimer = 5.0f;
    public float speed = 2.0f;
    float _rotationSpeed = 30.0f;
    Vector3 _currentRotation;
    SpriteRenderer _spriteRenderer;
    SpriteRenderer _playerSpriteRenderer;
    bool firstround;
    bool _hardExitCoroutine=false;

    public static bool hasBombStarted;

    BrickMap _brickMapRef;
    Tilemap _brickMapRefTilemap;
    PlaySoundResult _bombFuseSFX;

    // Start is called before the first frame update
    void Start()
    {
        _playerSpriteRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        hasBombStarted = false;
        _currentRotation = Vector3.zero;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        firstround = false;
        _brickMapRef = GameObject.Find("TilesBoss").GetComponent<BrickMap>(); 
        _brickMapRefTilemap = _brickMapRef.NonHiddenTilemap;        
    }

    void OnEnable()
    {
        _playerSpriteRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        hasBombStarted = false;
        _currentRotation = Vector3.zero;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        firstround = false;
        _hardExitCoroutine = false;
    }

    void FixedUpdate()
    {
        if (firstround == false)
        {
            firstround = true;
            speedInDirection = _playerSpriteRenderer.flipX ? Vector3.left : Vector3.right;
            speedInDirection *= speed;
        }

        _spriteRenderer.transform.DORotate(_currentRotation, 0.01f);
        gameObject.transform.Translate(speedInDirection.x*Time.deltaTime,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        _currentRotation.z += _rotationSpeed * Time.deltaTime;
        if(hasBombStarted==false)
            StartCoroutine(StartBombCycle());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name != "NonHidden")
        {
            if(_bombFuseSFX.ActingVariation!=null)
                _bombFuseSFX.ActingVariation.Stop();
            StopAllCoroutines();
            PoolBoss.SpawnInPool("ExplosionParent", transform.position, Quaternion.identity);
            PoolBoss.Despawn(transform);
        }
    }

    IEnumerator StartBombCycle()
    {
        hasBombStarted = true;
        _bombFuseSFX = MasterAudio.PlaySound("BombFuse");
        float bombFuseTimer = 3.9f;
        var particle = GetComponentInChildren<ParticleSystem>();
        Vector3 destination = Vector3.zero;
        destination.x = 0.211f;
        destination.y = 0.556f;
        particle.transform.DOLocalMove(destination, bombFuseTimer);

        var spritemask = GetComponentInChildren<SpriteMask>();
        spritemask.transform.DOScaleY(1.05f, bombFuseTimer);

        float bombFuseStartTime = Time.time;
        while (Time.time < bombFuseStartTime + bombFuseTimer)
        {
            yield return null;
            if (!_hardExitCoroutine) continue;
            particle.Stop();
            yield break;
        }
        particle.Stop();

        float FuseOutPregPause = Time.time+0.4f;
        while (Time.time < FuseOutPregPause)
        {
            yield return null;
            if (!_hardExitCoroutine) continue;
            yield break;               
        }
        PoolBoss.SpawnInPool("ExplosionParent",transform.position,Quaternion.identity);
        ActivateTheBrickBomb();
        destination.x = 0.38f;
        destination.y = 0.635f;
        particle.transform.DOLocalMove(destination, 0.05f);
        spritemask.transform.DOScaleY(1.8f, 0.05f);
        yield return new WaitForSeconds(0.1f);
        Kaboom();
    }

    void Kaboom()
    {
        hasBombStarted = false;
        PoolBoss.Despawn(transform);
    }
    
    void ActivateTheBrickBomb()
    {
        var bombCellPosition = GetTheBombSpawnPoint();
        var cellsToDestroy = GetBombAreaOfEffect(bombCellPosition,2);
        DestroyBricks(cellsToDestroy);
    }

    void DestroyBricks(List<Vector3Int> cellsToDestroy)
    {
        Log($" BombCellCount = {cellsToDestroy.Count}","red");
        foreach (var cell in cellsToDestroy)
        {
            if (!_brickMapRefTilemap.HasTile(cell))
            {
                Log($" cell={cell} : Empty","cyan");
                continue;
            }
            
            var tileName = _brickMapRef.NonHiddenTilemap.GetTile<Tile>(cell);
            var isDestructibleTile = _brickMapRef.IsDestructibleTile(cell);
            if (isDestructibleTile)
            {
                Log($" cell={cell} : brick to destroy","cyan");
                _brickMapRef.DestroyBrick(cell);
            }
            else
            {
                Log($" cell={cell} : brick static","cyan");
            }
        }
    }

    List<Vector3Int> GetBombAreaOfEffect(Vector3Int bombCellPosition,int cellrange)
    {
        List<Vector3Int> cellsInRange = new List<Vector3Int>();

        for (int x = -cellrange; x < (cellrange+1); x++)
        {
            int ybounds = cellrange - x;
            for (int y = -ybounds; y < (ybounds+1); y++)
            {
                Vector3Int cellToAdd=Vector3Int.zero;
                cellToAdd.x = x+bombCellPosition.x;
                cellToAdd.y = y+bombCellPosition.y;
                cellsInRange.Add(cellToAdd);        
            }
        }
        
        //Add y =0 in too
        for (int x = -cellrange; x < (cellrange + 1); x++)
        {
            Vector3Int cellToAdd=Vector3Int.zero;
            cellToAdd.x = x+bombCellPosition.x;
            cellToAdd.y = bombCellPosition.y;
            cellsInRange.Add(cellToAdd);                    
        }
        
        return cellsInRange;
    }

    Vector3Int GetTheBombSpawnPoint()
    {
        Vector3Int cellPosition;
        var adjustedPosition = transform.position;
        adjustedPosition.x += 0.5f;
        adjustedPosition.y += 0.5f;
        //adjustedPosition += _brickMapRef._HalfWorldCellOffset;
        var bombCenter = _brickMapRefTilemap.WorldToCell(adjustedPosition);
        return bombCenter;
    }

}
