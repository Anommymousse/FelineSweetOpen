using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using DG.Tweening;
using Spine;
using Spine.Unity;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using static MyExtensions.MyExtensions;

public class EnemyMushroom : MonoBehaviour
{
    //From object
    Transform _leftWallFeeler;
    Transform _rightWallFeeler;
    Transform _leftHighWallFeeler;
    Transform _rightHighWallFeeler;
    Transform _GroundDetectFeeler;
    Transform _leftLowWallFeeler;
    Transform _rightLowWallFeeler;
    
    public bool isClockwise;
    public Vector3 startPosition;
    public Vector3 startDirection;
    public float speed;
    public bool staticBrickFullReverse;
    public float chargeSpeed = 4.0f;
    public float delayToAttackSound = 0.1f;
    public float delayToAttack = 0.18f;
    public BricksScriptable _BricksScriptable;

    Vector3Int _contactFromBlockHit;
    
    float _swapBlockLastTimeUsed;
    float _coolDownOnSwapBlock=2.5f;

    static int counterThing=0; 
    
    Vector2 _traveldirection;

    bool _IsCharging;
    Vector3 _halfBrick;
    
    
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

    Transform _mushroomArtTransform;
    
    bool _isMushroomRestarting;
    bool _isMushroomSwappingTile;

    SkeletonAnimation _mushroomAnimation;
    float _mushroomAttackStartTime;
    float _mushroomAnimTime = 1.1f;
    Player _playerRef;


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
    void OnEnable()
    {
        string artname = CleanCloneToGetArtBaseName(gameObject.name);
        artname += "Art";
//        Log($"Artname = {artname} gameobject = {gameObject.name}");
        _mushroomArtTransform = gameObject.transform.Find(artname);
        _mushroomAnimation = _mushroomArtTransform.GetComponent<SkeletonAnimation>();
        
        SetRigidBodyToZero();
        GrabBrickMapRef();
        SetMushroomResetCallbacks();
        SetMushroomInitialDirection();
        SetMushroomInitialPosition();
        _direction = startDirection;
        SetMushroomInitialSpeedAndFlip();
        
        _leftWallFeeler = gameObject.transform.Find("LeftWallFeeler");
        _rightWallFeeler = gameObject.transform.Find("RightWallFeeler");//this.gameObject.GetComponentInChildren<RightFeeler>();
        _leftHighWallFeeler = gameObject.transform.Find("LeftHighWallFeeler");
        _rightHighWallFeeler = gameObject.transform.Find("RightHighWallFeeler");
        _GroundDetectFeeler = gameObject.transform.Find("GroundDetectFeeler");
        _leftLowWallFeeler = gameObject.transform.Find("LeftLowWallFeeler");
        _rightLowWallFeeler = gameObject.transform.Find("RightLowWallFeeler");
        
        //Yeah only bricks? maybe player?
        _collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");
        
        _isMushroomRestarting = false;

        _swapBlockLastTimeUsed = Time.time;
        _IsCharging = false;

        _mushroomAnimation.AnimationName = "Walk";
        _mushroomAttackStartTime = 0.0f;
        //Log($" <color=red> GHOST STARTING {gameObject.name} with direction {_direction} and SD {startDirection} </color>");

        var initial= _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.zero);
        var ones = _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.one);
        _halfBrick = (ones - initial) / 2.0f;
        
        _mushroomAnimation.AnimationState.Complete += AnimationStateOnComplete;
    }

    string CleanCloneToGetArtBaseName(string gameObjectName)
    {
        var stringSectioned = gameObjectName.Split('(');
        stringSectioned[0] = stringSectioned[0].Replace("Right", "");
        stringSectioned[0] = stringSectioned[0].Replace("Left", "");
        stringSectioned[0] = stringSectioned[0].Replace(" ", "");
        return stringSectioned[0];
    }

    void OnDisable()
    {
        _playerRef.OnPlayerReset -= ResetEnemy;
        _playerRef.OnPlayerLevelChange -= ClearAllEnemy;
        _mushroomAnimation.AnimationState.Complete -= AnimationStateOnComplete;
    }

    void SetMushroomInitialSpeedAndFlip()
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
    
    void SetMushroomInitialPosition()
    {
        _saved_InitialStartPosition = transform.position;
    }

    void SetMushroomInitialDirection()
    {
        _saved_InitialStartDirection = startDirection;
    }

    void GrabBrickMapRef()
    {
        var root = GameObject.Find("TilesBoss");
        _brickMap = root.GetComponentInChildren<BrickMap>();
    }

    void SetMushroomResetCallbacks()
    {
        var wht = GameObject.Find("Player");
        _playerRef = wht.GetComponent<Player>();
        _playerRef.OnPlayerReset += ResetEnemy;
        _playerRef.OnPlayerLevelChange += ClearAllEnemy;
    }


    void EnemyRestartGubbins()
    {
        if (gameObject.activeSelf is false) return;
        //Log($" <color=red> Ghost RE-STARTING {gameObject.name}</color>");
        _isMushroomRestarting = true; //Guard against update feelers while restarting
        
        SetRigidBodyToZero();
        SetMushroomInitialDirection();
        SetMushroomInitialPosition();
        SetMushroomInitialSpeedAndFlip();

        _isMushroomRestarting = false;
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
        if (_isMushroomRestarting) return;
        
        //Debug.Log($"<color=red> direction = {_traveldirection} </color>");
            if (_traveldirection == Vector2.left)
            {
                _traveldirection = Vector2.right;
                _rigidbody2D.velocity = _traveldirection * speed;
                SetEnemyFacingRight();
                return;
            }
            if (_traveldirection == Vector2.right)
            {
                _traveldirection = Vector2.left;
                _rigidbody2D.velocity = _traveldirection * speed;
                SetEnemyFacingLeft();
                return;
            }
            
            Debug.LogError($"No direction found {gameObject.name} direction = {_traveldirection} {speed}");
    }

    void SetEnemyFacingLeft()
    {
        //Log($" <color=red> GHOST STARTING {gameObject.name} set to left face </color>");
        Vector3 scaleChange = _mushroomArtTransform.localScale;
        if (scaleChange.x > 0)
            scaleChange.x *= -1;
        _mushroomArtTransform.localScale = scaleChange;
    }

    void SetEnemyFacingRight()
    {
        //Log($" <color=red> GHOST STARTING {gameObject.name} set to right face </color>");
        Vector3 scaleChange = _mushroomArtTransform.localScale;
        if (scaleChange.x < 0)
            scaleChange.x *= -1;
        _mushroomArtTransform.localScale = scaleChange;
    }

    //Handle the brick collision and movement changes.
    void OnCollisionEnter2D(Collision2D other)
    { 
        if (_isMushroomRestarting) return;
        //Debug.Log($"Ghost test 2d collision {other.collider.name}");
        Vector2 vector2direction = Vector2.zero;

        var thing = other.GetContact(0);
        var contactpoint = _brickMap.NonHiddenTilemap.WorldToCell(thing.point);
        if (_brickMap.NonHiddenTilemap.HasTile(contactpoint))
        {
            var tilename = _brickMap.NonHiddenTilemap.GetTile<Tile>(contactpoint);
            //Debug.Log($"Ghost hits tile ?{tilename} {thing.point}");
            //_brickMap.DestroyBrick(contactpoint);
        }
        
        _rigidbody2D.velocity = _traveldirection*speed;

    }

    bool HandledHittingABlock(Transform feelerToUse)
    {
        var rchit = Physics2D.RaycastAll(feelerToUse.position, Vector2.down, 0.05f,_collisionLayermask);
        
        if (rchit.Length > 0)
        {
            //Debug.Log($"<color=cyan> hit ={rchit[0].collider.name} td={_traveldirection} hit {rchit.Length} things wp={feelerToUse.position} </color>");
            
            if (rchit[0].collider.name.Contains("NonHidden"))
            {
                Vector2 contact = rchit[0].collider.ClosestPoint(feelerToUse.position);
                contact += _traveldirection * 0.3f;
                var contactpoint = _brickMap.NonHiddenTilemap.WorldToCell(contact);
                _contactFromBlockHit = contactpoint;
                /*if (_brickMap.NonHiddenTilemap.HasTile(contactpoint))
                {
                    _brickMap.DestroyBrick(contactpoint);
                }*/
                //Turn off charge when we hit something.
                //NextDirection();
                //_IsCharging = false;
                return true;
            }
        }
        return false;
    }

    void UpdateMushroomFeelers()
    {
        if (_isMushroomRestarting) return;
        if (_isMushroomSwappingTile) return;
        
        Transform feelerToUse = _rightWallFeeler;
        if(_traveldirection==Vector2.left) feelerToUse = _leftWallFeeler;

        /*if (HandledHittingABlock(feelerToUse)) return;

        bool test=false;
        Transform highFeelerToUse = _rightHighWallFeeler;
        if (_traveldirection == Vector2.left)
        {
            test = true;
            highFeelerToUse = _leftWallFeeler;
        }

        HandledHittingABlock(highFeelerToUse);*/
        
        //Log($"<color=red> dirtest  left is {test} {Time.time} gon={gameObject.name} </color>");
        /*if (HandledHittingABlock(highFeelerToUse))
        {
            Log($"<color=red> hit the block {Time.time} gon={gameObject.name}</color>");
        }
        else
        {
            Log($"<color=red> no block hit {Time.time} gon={gameObject.name}</color>");
        }*/

    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateMushroomFeelers();

        HandleAttackAnimation();
        
        if (_mushroomAnimation.AnimationName == "Attack")
        {
            var thing = _rigidbody2D.velocity;
            thing.x = 0.0f;
            _rigidbody2D.velocity = thing;
        }
        else
        {
            var thing = _rigidbody2D.velocity;
            thing.x = 0;
            thing.x = _IsCharging ? _traveldirection.x * (speed + chargeSpeed) : _traveldirection.x * speed;
            _rigidbody2D.velocity = thing;
        }

        HandleStopFallingOnGround();
        
        HandleStopMovingIntoWalls();
    }

    void HandleStopMovingIntoWalls()
    {
        if (IsAgainstAWall())
        {
            Log("Against a wall","cyan");
            var thing = _rigidbody2D.velocity;
            thing.x = 0;
            _rigidbody2D.velocity = thing;
        }
    }

    bool IsAgainstAWall()
    {
        Vector2 feelerTop;
        Vector2 feelerLow;
        if (_traveldirection.x > 0)
        {
            Log("Right","cyan");
            feelerTop = _rightHighWallFeeler.position;
            feelerLow = _rightLowWallFeeler.position;
        }
        else
        {
            Log("Left","red");
            feelerTop = _leftHighWallFeeler.position;
            feelerLow = _leftLowWallFeeler.position;
        }

        var rchit = Physics2D.RaycastAll(feelerTop, Vector2.down, 0.01f,_collisionLayermask);
        if (rchit.Length > 0)
        {
            //Debug.Log($"<color=cyan> hit ={rchit[0].collider.name} td={_traveldirection} hit {rchit.Length} things wp={feelerToUse.position} </color>");
            if (rchit[0].collider.name.Contains("NonHidden"))
            {
                var cellloc = _brickMap.NonHiddenTilemap.WorldToCell(feelerTop);
                Log($"Feeler top {cellloc}", "cyan");
                return true;
            }
        }
        
        rchit = Physics2D.RaycastAll(feelerLow, Vector2.down, 0.05f,_collisionLayermask);
        if (rchit.Length > 0)
        {
            //Debug.Log($"<color=cyan> hit ={rchit[0].collider.name} td={_traveldirection} hit {rchit.Length} things wp={feelerToUse.position} </color>");
            if (rchit[0].collider.name.Contains("NonHidden"))
            {
                Log($"Feeler bot", "red");
                var cellloc = _brickMap.NonHiddenTilemap.WorldToCell(feelerLow);
                Log($"Feeler low {cellloc}", "cyan");
                return true;
            }
        }
        return false;
    }

    void HandleStopFallingOnGround()
    {
        var rchit = Physics2D.RaycastAll(_GroundDetectFeeler.position, Vector2.down, 0.05f, _collisionLayermask);
        if (rchit.Length <= 0) return;
        
        //Debug.Log($"<color=cyan> hit ={rchit[0].collider.name} td={_traveldirection} hit {rchit.Length} things wp={_GroundDetectFeeler.position} </color>");
        if (rchit[0].collider.name.Contains("NonHidden"))
        {
            var temp = _rigidbody2D.velocity;
            temp.y = 0;
            _rigidbody2D.velocity = temp;
        }
    }

    void HandleAttackAnimation()
    {
        if (Time.time < _mushroomAttackStartTime + _mushroomAnimTime) return;
        int testBlock = 0;
        var direction = _traveldirection.x;
        if (direction > Single.Epsilon)
        {
            testBlock = 1;
        }

        if (direction < Single.Epsilon)
        {
            testBlock = -1;
        }

        Transform feelerToUse = _rightWallFeeler;
        if(_traveldirection==Vector2.left) feelerToUse = _leftWallFeeler;
        if (HandledHittingABlock(feelerToUse))
        {
            if(_traveldirection.x > 0)
            {
                _mushroomAttackStartTime = Time.time;
                _mushroomAnimation.AnimationName = "Attack";
                StartCoroutine(DelayedAttackKillbox(false,_contactFromBlockHit));
            }
                        
            if(_traveldirection.x < 0)
            {
                _mushroomAttackStartTime = Time.time;
                _mushroomAnimation.AnimationName = "Attack";
                _mushroomAnimation.loop = false;
                StartCoroutine(DelayedAttackKillbox(true,_contactFromBlockHit));
            }
        }
    }

    void AnimationStateOnComplete(TrackEntry trackentry)
    {
        if(trackentry.Animation.Name == "Attack")
        {
            _mushroomAnimation.AnimationName = "Walk";
            _mushroomAnimation.loop = true;
        }
        else
        {
            _mushroomAnimation.AnimationName = "Walk";
            _mushroomAnimation.loop = true;
        }
    }

    IEnumerator DelayedAttackKillbox(bool isLeft,Vector3Int mushroomCellLocation)
    {
        Log($"<color=red>  DelayedKill started </color>");
        yield return new WaitForSeconds(delayToAttackSound);
        MasterAudio.PlaySound("EnemySwoosh");
        yield return new WaitForSeconds(delayToAttack-delayToAttackSound);
        Log($"<color=red>  DelayedKill delays done </color>");
        
        var worldposOfBrick = _brickMap.NonHiddenTilemap.CellToWorld(mushroomCellLocation);
        worldposOfBrick += _halfBrick;
        
        Log($"<color=cyan> OG position = {worldposOfBrick} and cellloc = {mushroomCellLocation} </color>");
        
        if (_brickMap.IsDestructibleTile(mushroomCellLocation))
        {
            Log($"<color=red> Destructible at {mushroomCellLocation} </color>");
            var tileName =_brickMap.NonHiddenTilemap.GetTile(mushroomCellLocation).name;
            _brickMap.NonHiddenTilemap.SetTile(mushroomCellLocation, null);
            _brickMap.NonHiddenTilemap.RefreshTile(mushroomCellLocation);
            yield return null;

            Transform pbItem;
            if (!isLeft) worldposOfBrick.x -= 1.0f;
            pbItem = PoolBoss.SpawnInPool(isLeft ? "ThrowBrickLeft" : "ThrowBrickRight", worldposOfBrick, quaternion.identity);

            Log($"Spawning brick throw {tileName} at {mushroomCellLocation}","cyan");
            
            if (pbItem != null)
                HandleSpriteChange(pbItem,tileName);
        }
        else
        {
            Log($"<color=blue> Not destructible at {mushroomCellLocation} </color>");
            /*mapcellToBlast.y += 1;
            if (_brickMap.IsDestructibleTile(mapcellToBlast))
                Log($"<color=cyan> destructible at +1y {mapcellToBlast} </color>");
            mapcellToBlast.y -= 2;
            if (_brickMap.IsDestructibleTile(mapcellToBlast))
                Log($"<color=cyan> destructible at -1y {mapcellToBlast} </color>");
            mapcellToBlast.y += 1;
            mapcellToBlast.x += 1;
            if (_brickMap.IsDestructibleTile(mapcellToBlast))
                Log($"<color=cyan> destructible at +1x {mapcellToBlast} </color>");
            mapcellToBlast.x -= 1;
            if (_brickMap.IsDestructibleTile(mapcellToBlast))
                Log($"<color=cyan> destructible at -1x {mapcellToBlast} </color>");*/
            
        }
        NextDirection();
    }

    void HandleSpriteChange(Transform pbItem,string tileName)
    {
        var throwBrickComponent = pbItem.GetComponent<ThrowBrick>();
        var spriteRenderer = pbItem.GetComponent<SpriteRenderer>();
                    
        var foundSprite = _BricksScriptable.TiledBricksSprites.Find(d => d.name == tileName);
        if (foundSprite)
            spriteRenderer.sprite = foundSprite;    
                    
        throwBrickComponent.carryThrough = true;
        
    }
}
