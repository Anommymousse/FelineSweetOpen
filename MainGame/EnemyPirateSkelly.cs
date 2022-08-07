using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Tilemaps;
using static MyExtensions.MyExtensions;

public class EnemyPirateSkelly : MonoBehaviour
{
    //From object
    Transform _leftWallFeeler;
    Transform _rightWallFeeler;
    Transform _leftHighWallFeeler;
    Transform _rightHighWallFeeler;
    Transform _GroundDetectFeeler;
    public bool isClockwise;
    public Vector3 startPosition;
    public Vector3 startDirection;
    public float speed;
    public bool staticBrickFullReverse;
    public float chargeSpeed = 4.0f;
    public float delayToAttackSound = 0.1f;
    public float delayToAttack = 0.18f;


    Vector2 Velocties;

    bool _hitWallThisTurn = false;
    

    float _swapBlockLastTimeUsed;
    float _coolDownOnSwapBlock=2.5f;

    static int counterThing=0; 
    
    Vector2 _traveldirection;

    bool _IsCharging;
    Vector3 _halfBrick;
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

    Transform _pirateSkellyArtTransform;
    
    bool _isPirateRestarting;
    bool _isPirateSwappingTile;

    SkeletonAnimation _skeletonAnimation;
    float _skellyAttackStartTime;
    float _skellyAnimTime = 1.1f;
    float _coolDownTimer;


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
        _pirateSkellyArtTransform = gameObject.transform.Find("PirateSkellyArt");
        _skeletonAnimation = _pirateSkellyArtTransform.GetComponent<SkeletonAnimation>();
        
        SetRigidBodyToZero();
        GrabBrickMapRef();
        SetPirateResetCallbacks();
        SetPirateInitialDirection();
        SetPirateInitialPosition();
        _direction = startDirection;
        SetPirateInitialSpeedAndFlip();
        
        _leftWallFeeler = gameObject.transform.Find("LeftWallFeeler");
        _rightWallFeeler = gameObject.transform.Find("RightWallFeeler");//this.gameObject.GetComponentInChildren<RightFeeler>();
        _leftHighWallFeeler = gameObject.transform.Find("LeftHighWallFeeler");
        _rightHighWallFeeler = gameObject.transform.Find("RightHighWallFeeler");
        _GroundDetectFeeler = gameObject.transform.Find("GroundDetectFeeler");
        
        //Yeah only bricks? maybe player?
        _collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");
        
        _isPirateRestarting = false;

        _swapBlockLastTimeUsed = Time.time;
        _IsCharging = false;

        _skeletonAnimation.AnimationName = "Walk";
        _skellyAttackStartTime = 0.0f;
        //Log($" <color=red> GHOST STARTING {gameObject.name} with direction {_direction} and SD {startDirection} </color>");

        var initial= _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.zero);
        var ones = _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.one);
        _halfBrick = (ones - initial) / 2.0f;
        
        _skeletonAnimation.AnimationState.Complete += AnimationStateOnComplete;
    }

    void OnDisable()
    {
        _playerRef.OnPlayerReset -= ResetEnemy;
        _playerRef.OnPlayerLevelChange -= ClearAllEnemy;
        _skeletonAnimation.AnimationState.Complete -= AnimationStateOnComplete;
    }

    void SetPirateInitialSpeedAndFlip()
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
    
    void SetPirateInitialPosition()
    {
        _saved_InitialStartPosition = transform.position;
    }

    void SetPirateInitialDirection()
    {
        _saved_InitialStartDirection = startDirection;
    }

    void GrabBrickMapRef()
    {
        var root = GameObject.Find("TilesBoss");
        _brickMap = root.GetComponentInChildren<BrickMap>();
    }

    void SetPirateResetCallbacks()
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
        _isPirateRestarting = true; //Guard against update feelers while restarting
        
        SetRigidBodyToZero();
        SetPirateInitialDirection();
        SetPirateInitialPosition();
        SetPirateInitialSpeedAndFlip();

        _isPirateRestarting = false;
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
        if (Time.time < _coolDownTimer) return;
        if (_isPirateRestarting) return;
        _coolDownTimer = Time.time + 0.1f;
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
        Vector3 scaleChange = _pirateSkellyArtTransform.localScale;
        if (scaleChange.x > 0)
            scaleChange.x *= -1;
        _pirateSkellyArtTransform.localScale = scaleChange;
    }

    void SetEnemyFacingRight()
    {
        //Log($" <color=red> GHOST STARTING {gameObject.name} set to right face </color>");
        Vector3 scaleChange = _pirateSkellyArtTransform.localScale;
        if (scaleChange.x < 0)
            scaleChange.x *= -1;
        _pirateSkellyArtTransform.localScale = scaleChange;
    }

    //Handle the brick collision and movement changes.
    void OnCollisionEnter2D(Collision2D other)
    { 
        if (_isPirateRestarting) return;
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
        
        _rigidbody2D.velocity = _traveldirection*speed;

    }

    bool HandledHittingABlock(Transform feelerToUse)
    {
        var rchit = Physics2D.RaycastAll(feelerToUse.position, Vector2.down, 0.05f,_collisionLayermask);
        
        if (rchit.Length > 0)
        {
            //Debug.Log($"<color=cyan> hit ={rchit[0].collider.name} td={_traveldirection} hit {rchit.Length} things wp={feelerToUse.position} </color>");
            for (var index = 0; index < rchit.Length; index++)
            {
                var raycastHit2D = rchit[index];
                if (raycastHit2D.collider.name.Contains("NonHidden"))
                {
                    Vector2 contact = rchit[index].collider.ClosestPoint(feelerToUse.position);
                    contact += _traveldirection * 0.3f;
                    var contactpoint = _brickMap.NonHiddenTilemap.WorldToCell(contact);
                    if (_brickMap.NonHiddenTilemap.HasTile(contactpoint))
                    {
                        _brickMap.DestroyBrick(contactpoint);
                    }
                    //Turn off charge when we hit something.
                    NextDirection();
                    _IsCharging = false;
                    _hitWallThisTurn = true;
                    return true;
                }
            }

            /*if (rchit[0].collider.name.Contains("NonHidden"))
            {
                Vector2 contact = rchit[0].collider.ClosestPoint(feelerToUse.position);
                contact += _traveldirection * 0.3f;
                var contactpoint = _brickMap.NonHiddenTilemap.WorldToCell(contact);
                if (_brickMap.NonHiddenTilemap.HasTile(contactpoint))
                {
                    _brickMap.DestroyBrick(contactpoint);
                }
                //Turn off charge when we hit something.
                NextDirection();
                _IsCharging = false;
                return true;
            }*/
        }
        return false;
    }

    void UpdatePirateFeelers()
    {
        if (_isPirateRestarting) return;
        if (_isPirateSwappingTile) return;
        
        Transform feelerToUse = _rightWallFeeler;
        if(_traveldirection==Vector2.left) feelerToUse = _leftWallFeeler;

        if (HandledHittingABlock(feelerToUse)) return;
        
        bool test=false;
        Transform highFeelerToUse = _rightHighWallFeeler;
        if (_traveldirection == Vector2.left)
        {
            test = true;
            highFeelerToUse = _leftHighWallFeeler;
        }

        HandledHittingABlock(highFeelerToUse);
        
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
        _hitWallThisTurn = false;
        
        UpdatePirateFeelers();
        
        HandleChargeAtPlayer();
        
        HandleAttackAnimation();
        
        if (_skeletonAnimation.AnimationName == "Attack")
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
            if (_hitWallThisTurn) thing.x = 0;
            _rigidbody2D.velocity = thing;
            
        }

        HandleStopFallingOnGround();

        Velocties.x = _rigidbody2D.velocity.x;
        Velocties.y = _rigidbody2D.velocity.y;

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
        if (Time.time < _skellyAttackStartTime + _skellyAnimTime) return; 

        var playerCellPos = _brickMap.NonHiddenTilemap.WorldToCell(Player.GetWorldLocation());
        var pirateCellPos = _brickMap.NonHiddenTilemap.WorldToCell(transform.position);
        
        
        //if (Mathf.Abs(playerCellPos.x - pirateCellPos.x) < 1) //2
        if(_IsCharging)
            if(Mathf.Abs((Player.GetWorldLocation().x - transform.position.x)) < _halfBrick.x*1.9f )
            {
                if(_traveldirection.x > 0)
                {
                        var killbox = pirateCellPos;
                        _skellyAttackStartTime = Time.time;
                        _skeletonAnimation.AnimationName = "Attack";
                        StartCoroutine(DelayedAttackKillbox(false));
                }
                        
                if(_traveldirection.x < 0)
                {
                        var killbox = pirateCellPos;
                        _skellyAttackStartTime = Time.time;
                        _skeletonAnimation.AnimationName = "Attack";
                        _skeletonAnimation.loop = false;
                        StartCoroutine(DelayedAttackKillbox(true));
                }
            }
            else
            {
                _skeletonAnimation.AnimationName = "Walk";
            }
        else
        {
            if(_skeletonAnimation.AnimationName != "Walk")
                _skeletonAnimation.AnimationName = "Walk";
        }
    }

    void AnimationStateOnComplete(TrackEntry trackentry)
    {
        if(trackentry.Animation.Name == "Attack")
        {
            _skeletonAnimation.AnimationName = "Walk";
            _skeletonAnimation.loop = true;
        }
        else
        {
            _skeletonAnimation.AnimationName = "Walk";
            _skeletonAnimation.loop = true;
        }
    }

    IEnumerator DelayedAttackKillbox(bool isLeft)
    {
        yield return new WaitForSeconds(delayToAttackSound);
        MasterAudio.PlaySound("EnemySwoosh");
        yield return new WaitForSeconds(delayToAttack-delayToAttackSound);
        
        //var position = _brickMap.NonHiddenTilemap.CellToWorld(killboxSpawn);
        var position = transform.position;
        position += _halfBrick;
        var spawnedSwipe = PoolBoss.SpawnInPool("PirateSkellySwipe",position,Quaternion.identity);
        var pirateSkellySwipeRef = spawnedSwipe.GetComponent<PirateSkellySwipe>();
        
        pirateSkellySwipeRef.SetFacingRight();
        if (isLeft)
        {
            pirateSkellySwipeRef.SetFacingLeft();
            var goSwipeArt = spawnedSwipe.Find("GameObject");
            var scaled = goSwipeArt.localScale;
            scaled.x *= -1;
            goSwipeArt.localScale = scaled;
            goSwipeArt.position -= _halfBrick;
        }
        
        Vector3 rotation = Vector3.zero;
        rotation.x = 0;
        rotation.y = 0;
        rotation.z = 0f;
        spawnedSwipe.DORotate(rotation, 0f);
        
    }

    void HandleChargeAtPlayer()
    {
        if (_IsCharging) return;
        
        var playerCellPos = _brickMap.NonHiddenTilemap.WorldToCell(Player.GetWorldLocation());
        var pirateCellPos = _brickMap.NonHiddenTilemap.WorldToCell(transform.position);
        if ((playerCellPos.y+1) != pirateCellPos.y) return;

        if (playerCellPos.x < pirateCellPos.x) 
        {
            if (_traveldirection.x < 0)
            {
                //Charge
                _IsCharging = true;
            }
        }
        
        if (playerCellPos.x > pirateCellPos.x) 
        {
            if (_traveldirection.x > 0)
            {
                //Charge
                _IsCharging = true;
            }
        }

    }
}
