using System.Collections;
using System.Collections.Generic;
using DarkTonic.CoreGameKit.Examples;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using Spine;
using Spine.Unity;
using UnityEngine;
using static MyExtensions.MyExtensions;

public class EnemyBook : MonoBehaviour
{
    Vector3 _og_position;
    Rigidbody2D _rigidbody2D;
    bool _isBookAttacking;
    BrickMap _brickMap;
    RaycastHit2D[] _raycastHit2Ds;
    
    Transform _BookArtTransform;
    SpriteRenderer _dormantBookSpriteRenderer;
    SkeletonAnimation _skeletonAnimation;
    float _delayToAttackSound = 0.5f;

    bool _isBookRestarting;
    
    bool _alreadySetup;
    bool facing;
    float _flyingspeed = 4.1f;
    
    int _collisionLayermask;
    Transform _floorWallFeeler;
    Transform _ceilingWallFeeler;
    Vector3 _halfBrick;

    float _bookAttackRange = 7.0f;
    Player _player;
    Vector2 _distanceFromPlayer;

    void OnEnable()
    {
        _alreadySetup = false;
        SetupBook();
        var initial= _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.zero);
        var ones = _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.one);
        _halfBrick = (ones - initial) / 2.0f;
        
        var wht = GameObject.Find("Player");
        _player = wht.GetComponent<Player>();
        _player.OnPlayerReset -= ResetEnemy;
        _player.OnPlayerLevelChange -= ClearAllEnemy;
        _distanceFromPlayer = Vector2.zero;
        _raycastHit2Ds = new RaycastHit2D[10];
    }

    void OnDisable()
    {
        /*var wht = GameObject.Find("Player");
        if (wht)
        {
            Player _player = wht.GetComponent<Player>();
            _player.OnPlayerReset -= ResetEnemy;
            _player.OnPlayerLevelChange -= ClearAllEnemy;
        }*/
        _skeletonAnimation.AnimationState.Complete -= AnimationStateOnComplete;
    }

    void SetupBook()
    {
        if (_alreadySetup)
        {
            gameObject.transform.position = _og_position;
            SetRigidBodyToZero();
        }
        else
        {
            _alreadySetup = true;
            _og_position = gameObject.transform.position;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            SetRigidBodyToZero();
            GrabBrickMapRef();
            SetEnemyResetCallbacks();
            _BookArtTransform = gameObject.transform.Find("BookArt");
            var temp = gameObject.transform.Find("DormantBookArt");
            _dormantBookSpriteRenderer = temp.GetComponent<SpriteRenderer>();
            _skeletonAnimation = _BookArtTransform.GetComponent<SkeletonAnimation>();
            _collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");
            _skeletonAnimation.AnimationState.Complete += AnimationStateOnComplete;
            _ceilingWallFeeler = gameObject.transform.Find("CeilingWallFeeler");
            _floorWallFeeler = gameObject.transform.Find("FloorWallFeeler");
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

    void SetRigidBodyToZero()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = Vector2.zero;
    }

    void GrabBrickMapRef()
    {
        var root = GameObject.Find("TilesBoss");
        _brickMap = root.GetComponentInChildren<BrickMap>();
    }

    void SetEnemyResetCallbacks()
    {
        var wht = GameObject.Find("Player");
        Player _player = wht.GetComponent<Player>();
        _player.OnPlayerReset += ResetEnemy;
        _player.OnPlayerLevelChange += ClearAllEnemy;
    }

    void ClearAllEnemy()
    {
        PoolBoss.Despawn(transform);
    }

    void ResetEnemy()
    {
        EnemyRestartGubbins();
    }

    void EnemyRestartGubbins()
    {
        if (gameObject.activeSelf is false) return;
        //Log($" <color=red> Ghost RE-STARTING {gameObject.name}</color>");
        _isBookRestarting = true; //Guard against update feelers while restarting
        
        SetRigidBodyToZero();
        SetEnemyInitialDirection();
        SetEnemyInitialPosition();
        
        _isBookRestarting = false;
    }

    void SetEnemyFacingLeft()
    {
        //Log($" <color=red> GHOST STARTING {gameObject.name} set to left face </color>");
        Vector3 scaleChange = _BookArtTransform.localScale;
        if (scaleChange.x > 0)
            scaleChange.x *= -1;
        _BookArtTransform.localScale = scaleChange;
    }

    void SetEnemyFacingRight()
    {
        //Log($" <color=red> GHOST STARTING {gameObject.name} set to right face </color>");
        Vector3 scaleChange = _BookArtTransform.localScale;
        if (scaleChange.x < 0)
            scaleChange.x *= -1;
        _BookArtTransform.localScale = scaleChange;
    }
    
    
    void SetEnemyInitialPosition()
    {
        gameObject.transform.position = _og_position;
    }

    void SetEnemyInitialDirection()
    {
    }

    
    bool IsPlayerRightOfEnemy()
    {
        var playerCellPos = _brickMap.NonHiddenTilemap.WorldToCell(Player.GetWorldLocation());
        var pirateCellPos = _brickMap.NonHiddenTilemap.WorldToCell(transform.position);
        if (playerCellPos.x > pirateCellPos.x) return true;
        return false;
    }

    
    
    void EnemyChangeDirection()
    {
        Transform feelerToUse = _floorWallFeeler;
    
        if (HandleHittingABlock(feelerToUse))
        {
        }
        
    }

    bool HandleHittingABlock(Transform feelerToUse)
    {
        var rchit = Physics2D.RaycastAll(feelerToUse.position, Vector2.down, 0.05f, _collisionLayermask);

        if (rchit.Length > 0)
        {
            //Debug.Log($"<color=cyan> hit ={rchit[0].collider.name} td={_traveldirection} hit {rchit.Length} things wp={feelerToUse.position} </color>");

            if (rchit[0].collider.name.Contains("NonHidden"))
            {
                return true;
            }
        }
        return false;
    }

    Vector2 DirectionToMoveIn()
    {
        return _distanceFromPlayer;
    }
    
    void BookMove()
    {
        var direction = DirectionToMoveIn();
        var directionNormalized = direction.normalized;
        Log($"Direction To player = {directionNormalized} direction = {direction} at Time {Time.time}");
        var vel = _flyingspeed * directionNormalized;
        _rigidbody2D.velocity = vel;
    }
    
    // Update is called once per frame
    void Update()
    {
        _isBookAttacking = ShouldBookAttack();
        
        if (_isBookAttacking)
        {
            MeshArtActivate(true);
            bool isFacingRight = IsPlayerRightOfEnemy(); 
            if (isFacingRight)
                SetEnemyFacingRight();
            else
                SetEnemyFacingLeft();
            
            BookMove();
        }
        else
        {
            MeshArtActivate(false);
            EnemyDontMove();
        }
        
    }

    void MeshArtActivate(bool bookArtMeshActive)
    {
        if (bookArtMeshActive)
        {
            
            var meshRenderer = _BookArtTransform.GetComponent<MeshRenderer>();
            if (meshRenderer.enabled is false)
            {
                PoolBoss.SpawnInPool("BuildBlockCTF",transform.position,Quaternion.identity);
                meshRenderer.enabled = true;
            }

            _dormantBookSpriteRenderer.enabled = false;
        }
        else
        {
            var meshRenderer = _BookArtTransform.GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
            if (_dormantBookSpriteRenderer.enabled is false)
            {
                PoolBoss.SpawnInPool("BuildBlockCTF",transform.position,Quaternion.identity);
                _dormantBookSpriteRenderer.enabled = true;
            }
        }
    }

    bool ShouldBookAttack()
    {
        Vector2 playerv2 = _player.transform.position;
        Vector2 transformv2 = transform.position;
        var direction = playerv2 - transformv2;
        _distanceFromPlayer = direction;
        
        var unitDirectionToPlayer = direction.normalized;
        var endpoint = unitDirectionToPlayer * _bookAttackRange + transformv2;
        endpoint.y += _halfBrick.y;
        var numberOfResults = Physics2D.LinecastNonAlloc(transformv2, endpoint, _raycastHit2Ds);
        //var rchit = Physics2D.RaycastAll(transform.position, unitDirectionToPlayer, _bookAttackRange);
        
        var enemyrayEndPoint = transformv2 + unitDirectionToPlayer * _bookAttackRange;
        Log($" dist = {_distanceFromPlayer.magnitude} vector to player {_distanceFromPlayer} src {transformv2} dest {playerv2} at {Time.time}");
        //Debug.DrawLine(transform.position,enemyrayEndPoint,Color.green);

        for(int i=0; i< numberOfResults ; i++)
        {
            
            Log($"<color=cyan> Found a {_raycastHit2Ds[i].collider.name} at {Time.time} </color>");
            // index 1 as book is index 0, they are returned in order from start point.
            if (i != 1) continue;
            if (_raycastHit2Ds[1].collider.name.Contains("ShieldParent"))
            {
                Log($"<color=red> Found a player return true at {Time.time}</color>");
                return true;
            }

            var thing = GameObject.FindGameObjectWithTag("Player");
            
            
        }

        return false;
    }

    void EnemyDontMove()
    {
        _rigidbody2D.velocity = Vector2.zero;        
    }
    
}
