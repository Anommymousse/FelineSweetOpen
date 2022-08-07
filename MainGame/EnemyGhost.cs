using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Tilemaps;
using static MyExtensions.MyExtensions;

public class EnemyGhost : MonoBehaviour
{
    //From object
    Transform _leftWallFeeler;
    Transform _rightWallFeeler;
    Transform _groundWallFeeler;
    Transform _roofWallFeeler;
    public bool isClockwise;
    public Vector3 startPosition;
    public Vector3 startDirection;
    public float speed;
    public bool staticBrickFullReverse;
    float _swapBlockLastTimeUsed;
    float _coolDownOnSwapBlock=2.5f;

    static int counterThing=0; 
    
    Vector2 _traveldirection;
    Player _playerRef;
    
    
    //Saved vars...
    Rigidbody2D _rigidbody2D;
    BrickMap _brickMap;
    Vector2 _direction;
    //Animator _animatorBase;
    int _collisionLayermask;
    
    //Startup save position and direction
    Vector3 _saved_InitialStartDirection;
    Vector3 _saved_InitialStartPosition;
    Vector2 _saved_rigidbody_position;

    Transform _ghostArtTransform;
    
    bool _isGhostRestarting;
    bool _isGhostSwappingTile;

    SkeletonAnimation _skeletonAnimation;


    void SetRigidBodyToZero()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = Vector2.zero;
        _saved_rigidbody_position = _rigidbody2D.position;
    }

    void SetUpSpriteRenderer()
    {
        
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _ghostArtTransform = gameObject.transform.Find("GhostArt");
        _skeletonAnimation = _ghostArtTransform.GetComponent<SkeletonAnimation>();
        
        SetRigidBodyToZero();
        GrabBrickMapRef();
        SetGhostResetCallbacks();
        SetGhostInitialDirection();
        SetGhostInitialPosition();
        _direction = startDirection;
        SetGhostInitialSpeedAndFlip();
        
        _leftWallFeeler = gameObject.transform.Find("LeftFeeler");
        _rightWallFeeler = gameObject.transform.Find("RightFeeler");//this.gameObject.GetComponentInChildren<RightFeeler>();
        _groundWallFeeler = gameObject.transform.Find("DownFeeler");
        _roofWallFeeler = gameObject.transform.Find("UpFeeler");
        
        //Yeah only bricks? maybe player?
        _collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");
        
        _isGhostRestarting = false;

        _swapBlockLastTimeUsed = Time.time;
 

        //Log($" <color=red> GHOST STARTING {gameObject.name} with direction {_direction} and SD {startDirection} </color>");

    }

    void OnEnable()
    {
        var wht = GameObject.Find("Player");
        _playerRef = wht.GetComponent<Player>();
    }

    void OnDisable()
    {
        _playerRef.OnPlayerReset -= ResetEnemy;
        _playerRef.OnPlayerLevelChange -= ClearAllEnemy;
    }


    void SetGhostInitialSpeedAndFlip()
    {
        _traveldirection = startDirection;

        if (_traveldirection.x < -0.01f)
        {
            SetEnemyFacingLeft();
        }
        else
        {
            SetEnemyFacingRight();
        }
            
        _rigidbody2D.velocity = _traveldirection*speed;
    }
    
    void SetGhostInitialPosition()
    {
        _saved_InitialStartPosition = transform.position;
    }

    void SetGhostInitialDirection()
    {
        _saved_InitialStartDirection = startDirection;
    }

    void GrabBrickMapRef()
    {
        var root = GameObject.Find("TilesBoss");
        _brickMap = root.GetComponentInChildren<BrickMap>();
    }

    void SetGhostResetCallbacks()
    {
        var wht = GameObject.Find("Player");
        Player _player = wht.GetComponent<Player>();
        _player.OnPlayerReset += ResetEnemy;
        _player.OnPlayerLevelChange += ClearAllEnemy;
    }


    void EnemyRestartGubbins()
    {
        if (gameObject.activeSelf is false) return;
        Log($" <color=red> Ghost RE-STARTING {gameObject.name}</color>");
        _isGhostRestarting = true; //Guard against update feelers while restarting
        
        SetRigidBodyToZero();
        SetGhostInitialDirection();
        SetGhostInitialPosition();
        SetGhostInitialSpeedAndFlip();

        _isGhostRestarting = false;
    }
    void ResetEnemy()
    {
        /* New stuffs*/
        EnemyRestartGubbins();
        
    }

    void ClearAllEnemy()
    {
        PoolBoss.Despawn(transform);
        //PoolBoss.Destroy(transform);
    }
    
    void NextDirection()
    {
        if (_isGhostRestarting) return;
        
        //Debug.Log($"<color=red> direction = {_traveldirection} </color>");
            if (_traveldirection == Vector2.up)
            {
                _traveldirection = Vector2.left;
                SetEnemyFacingLeft();
                if (isClockwise)
                {
                    _traveldirection = Vector2.right;
                    SetEnemyFacingRight(); 
                }
                _rigidbody2D.velocity = _traveldirection * speed;
                return;
            }
            if (_traveldirection == Vector2.down)
            {
                _traveldirection = Vector2.right;
                SetEnemyFacingRight();
                if (isClockwise)
                {
                    _traveldirection = Vector2.left;
                    SetEnemyFacingLeft();
                }
                _rigidbody2D.velocity = _traveldirection * speed;
                
                
                return;
            }
            if (_traveldirection == Vector2.left)
            {
                _traveldirection = Vector2.down;
                if (isClockwise) _traveldirection = Vector2.up;
                _rigidbody2D.velocity = _traveldirection * speed;
                SetEnemyFacingRight();
                return;
            }
            if (_traveldirection == Vector2.right)
            {
                _traveldirection = Vector2.up;
                if (isClockwise) _traveldirection = Vector2.down;
                _rigidbody2D.velocity = _traveldirection * speed;
                SetEnemyFacingLeft();
                return;
            }
            
            Debug.LogError($"No direction found {gameObject.name} direction = {_traveldirection} {speed}");
    }

    void SetEnemyFacingLeft()
    {
        //Log($" <color=red> GHOST STARTING {gameObject.name} set to left face </color>");
        Vector3 scaleChange = _ghostArtTransform.localScale;
        if (scaleChange.x > 0)
            scaleChange.x *= -1;
        _ghostArtTransform.localScale = scaleChange;
    }

    void SetEnemyFacingRight()
    {
        //Log($" <color=red> GHOST STARTING {gameObject.name} set to right face </color>");
        Vector3 scaleChange = _ghostArtTransform.localScale;
        if (scaleChange.x < 0)
            scaleChange.x *= -1;
        _ghostArtTransform.localScale = scaleChange;
    }

    //Handle the brick collision and movement changes.
    void OnCollisionEnter2D(Collision2D other)
    { 
        if (_isGhostRestarting) return;
        //Debug.Log($"Ghost test 2d collision {other.collider.name}");
        Vector2 vector2direction = Vector2.zero;

        var thing = other.GetContact(0);
        var contactpoint = _brickMap.NonHiddenTilemap.WorldToCell(thing.point);
        if (_brickMap.NonHiddenTilemap.HasTile(contactpoint))
        {
            var tilename = _brickMap.NonHiddenTilemap.GetTile<Tile>(contactpoint);
            //Debug.Log($"Ghost hits tile ?{tilename} {thing.point}");
            _brickMap.DestroyBrick(contactpoint);
        }

        if (staticBrickFullReverse)
            counterThing++;
        //if (staticBrickFullReverse)
        //    Debug.Log($"Ghost Hit Counter={counterThing} </color>");
        
        //NextDirection();
        _rigidbody2D.velocity = _traveldirection*speed;

    }

    void UpdateGhostFeelers()
    {
        if (_isGhostRestarting) return;
        if (_isGhostSwappingTile) return;
        
        Transform feelerToUse = _rightWallFeeler;
        if(_traveldirection==Vector2.left) feelerToUse = _leftWallFeeler;
        if(_traveldirection==Vector2.up) feelerToUse = _roofWallFeeler;
        if(_traveldirection==Vector2.down) feelerToUse = _groundWallFeeler;
        
        var rchit = Physics2D.RaycastAll(feelerToUse.position, Vector2.down, 0.05f,_collisionLayermask);
        
        if (rchit.Length > 0)
        {
            //Debug.Log($"<color=cyan> hit ={rchit[0].collider.name} td={_traveldirection} hit {rchit.Length} things wp={feelerToUse.position} </color>");
            
            if (rchit[0].collider.name.Contains("NonHidden"))
            {
                Vector2 contact = rchit[0].collider.ClosestPoint(feelerToUse.position);
                contact += _traveldirection * 0.3f;
                var contactpoint = _brickMap.NonHiddenTilemap.WorldToCell(contact);
                if (_brickMap.NonHiddenTilemap.HasTile(contactpoint))
                {
                    var tilename = _brickMap.NonHiddenTilemap.GetTile<Tile>(contactpoint);
                    
                    if (staticBrickFullReverse)
                    {
                        if (tilename.name.ToLower().Contains("static"))
                        {
                            //Log($"<color=cyan> direction now {_traveldirection} </color>");
                            StartCoroutine(GhostSwappingTile(contactpoint,_traveldirection));
                            //if(_isGhostSwappingTile is false)
                            //    NextDirection();
                        }
                    }
                    if(_isGhostSwappingTile is false) NextDirection();
                    _brickMap.DestroyBrick(contactpoint);
                }
                else
                {
                 //   Debug.LogError($"<color=red> No tile! </color>");
                }
            }
            //NextDirection();
        }
        
    }

    IEnumerator GhostSwappingTile(Vector3Int tileToMove,Vector2 direction)
    {

        if (Time.time < _swapBlockLastTimeUsed + _coolDownOnSwapBlock)
        {
            NextDirection();
            yield break;
        }
        _swapBlockLastTimeUsed = Time.time;
        _isGhostSwappingTile = true;
        
        //Test bounds
        if (tileToMove.x is 5 or -12 || tileToMove.y is 3 or -11)
        {
            NextDirection();
            _swapBlockLastTimeUsed = Time.time - _coolDownOnSwapBlock - 1.0f;
            _isGhostSwappingTile = false;
            yield break;
        }
        
        _skeletonAnimation.AnimationName = "Attack";
        Log($"<color=cyan> direction now {direction} </color>");
        yield return null;

        //Coords for other block
        Vector3Int OtherTile = Vector3Int.left;
//        Log($"<color=cyan> direction = {_traveldirection} </color>");
        
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y < 0)
                OtherTile = Vector3Int.up;
            else
                OtherTile = Vector3Int.down;
        }
        else
        {
            if (direction.x < 0)
                OtherTile = Vector3Int.right;
        }
        OtherTile *= 2;
        var tileToReplace = tileToMove + OtherTile;
        
        //Log($"<color=cyan> Ghost trying to swap at {tileToMove} with tile at {tileToReplace} dir {direction} </color>");
        
        var tilenameHit = _brickMap.NonHiddenTilemap.GetTile<Tile>(tileToMove);
        var tilenameReplace = _brickMap.NonHiddenTilemap.GetTile<Tile>(tileToReplace);

        StartSwapBlockAnimation(tileToMove,tileToReplace);
        
        yield return new WaitForSeconds(1.0f);
        
        //NOTE ADD TO ONDISABLE!
        _skeletonAnimation.AnimationName = "Walk";
        
        //Swap the blocks in map
        _brickMap.NonHiddenTilemap.SetTile(tileToMove,tilenameReplace);
        _brickMap.NonHiddenTilemap.SetTile(tileToReplace,tilenameHit);
        _brickMap.NonHiddenTilemap.RefreshTile(tileToMove);
        _brickMap.NonHiddenTilemap.RefreshTile(tileToReplace);

        NextDirection();
        
        _isGhostSwappingTile = false;
    }

    
    
    void StartSwapBlockAnimation(Vector3Int sourceBlock, Vector3Int destBlock)
    {
        var SRC_tile = _brickMap.NonHiddenTilemap.GetTile<Tile>(sourceBlock);
        //var DEST_tile = _brickMap.NonHiddenTilemap.GetTile<Tile>(sourceBlock);

        if (SRC_tile == null) return;
        
        //Blank the tiles out.
        /*_brickMap.NonHiddenTilemap.SetTile(sourceBlock,null);
        _brickMap.NonHiddenTilemap.SetTile(destBlock,null);
        _brickMap.NonHiddenTilemap.RefreshTile(sourceBlock);
        _brickMap.NonHiddenTilemap.RefreshTile(destBlock);*/
        
        //Start swap.
        var worldsrcpos = _brickMap.NonHiddenTilemap.CellToWorld(sourceBlock);
        var worlddestpos = _brickMap.NonHiddenTilemap.CellToWorld(destBlock);

        var zerolockpos = _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.zero);
        var oneblockpos = _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.one);
        var halfBlock = (oneblockpos-zerolockpos) / 2.0f;
        PoolBoss.SpawnInPool("EnemyBlockSwap",worldsrcpos+halfBlock, Quaternion.identity);
        PoolBoss.SpawnInPool("EnemyBlockSwap",worlddestpos+halfBlock, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGhostFeelers();

        if (!_isGhostSwappingTile)
            _rigidbody2D.velocity = _traveldirection*speed;
        else
            _rigidbody2D.velocity = Vector2.zero;
    }
}
