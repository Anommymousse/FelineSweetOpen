using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using Spine;
using Spine.Unity;
using UnityEngine;
using static MyExtensions.MyExtensions;

public class EnemyFireDragon : MonoBehaviour
{
    public float fireBallCoolDown=3.0f;
    Vector3 _og_position;
    Rigidbody2D _rigidbody2D;
    bool _og_GoingUp;
    bool _isDragonAttacking;
    BrickMap _brickMap;
    
    Transform _fireDragonArtTransform;
    SkeletonAnimation _skeletonAnimation;

    bool _isDragonRestarting;
    
    bool _alreadySetup;
    bool facing;
    float _flyingspeed = 2.1f;
    float _FireBallCastAvailableTime;

    bool _isFiringAtPlayer;
    float _firingAnimationTime = 1.0f;
    float _delayToAttackSound = 0.5f; 
    int _collisionLayermask;
    Transform _floorWallFeeler;
    Transform _ceilingWallFeeler;
    Vector3 _halfBrick;

    void OnEnable()
    {
        _alreadySetup = false;
        _isDragonAttacking = false;
        _isFiringAtPlayer = false;
        SetupDragon();
        var initial= _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.zero);
        var ones = _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.one);
        _halfBrick = (ones - initial) / 2.0f;
    }

    void OnDisable()
    {
        var wht = GameObject.Find("Player");
        if (wht)
        {
            Player _player = wht.GetComponent<Player>();
            _player.OnPlayerReset -= ResetEnemy;
            _player.OnPlayerLevelChange -= ClearAllEnemy;
        }
        _skeletonAnimation.AnimationState.Complete -= AnimationStateOnComplete;
        _isDragonAttacking = false;
    }

    void SetupDragon()
    {
        if (_alreadySetup)
        {
            gameObject.transform.position = _og_position;
            _og_GoingUp = false;
            _isFiringAtPlayer = false;
            SetRigidBodyToZero();
            _FireBallCastAvailableTime = 0.0f;
            
        }
        else
        {
            _alreadySetup = true;
            _og_position = gameObject.transform.position;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _og_GoingUp = false;
            _isFiringAtPlayer = false;
            SetRigidBodyToZero();
            GrabBrickMapRef();
            SetEnemyResetCallbacks();
            _fireDragonArtTransform = gameObject.transform.Find("FireDragonArt");
            _skeletonAnimation = _fireDragonArtTransform.GetComponent<SkeletonAnimation>();
            _collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");
            _skeletonAnimation.AnimationState.Complete += AnimationStateOnComplete;
            _FireBallCastAvailableTime = 0;
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
        _isDragonRestarting = true; //Guard against update feelers while restarting
        
        SetRigidBodyToZero();
        SetEnemyInitialDirection();
        SetEnemyInitialPosition();
        
        _isDragonRestarting = false;
    }

    void SetEnemyFacingLeft()
    {
        //Log($" <color=red> GHOST STARTING {gameObject.name} set to left face </color>");
        Vector3 scaleChange = _fireDragonArtTransform.localScale;
        if (scaleChange.x > 0)
            scaleChange.x *= -1;
        _fireDragonArtTransform.localScale = scaleChange;
    }

    void SetEnemyFacingRight()
    {
        //Log($" <color=red> GHOST STARTING {gameObject.name} set to right face </color>");
        Vector3 scaleChange = _fireDragonArtTransform.localScale;
        if (scaleChange.x < 0)
            scaleChange.x *= -1;
        _fireDragonArtTransform.localScale = scaleChange;
    }
    
    
    void SetEnemyInitialPosition()
    {
        gameObject.transform.position = _og_position;
    }

    void SetEnemyInitialDirection()
    {
        _og_GoingUp = false;
    }

    bool IsPlayerOnSameYAsDragon()
    {
        var playerCellPos = _brickMap.NonHiddenTilemap.WorldToCell(Player.GetWorldLocation());
        var adjPosition = transform.position;
        adjPosition.y -= 0.5f;
        if (_og_GoingUp)
            adjPosition.y -= 1.0f;    
        
        var enemyCellPos = _brickMap.NonHiddenTilemap.WorldToCell(adjPosition);
        if ((playerCellPos.y  ) != enemyCellPos.y) return false;
        //if ((playerCellPos.y + 1) != pirateCellPos.y) return false;
        return true;
    }

    bool IsPlayerRightOfDragon()
    {
        var playerCellPos = _brickMap.NonHiddenTilemap.WorldToCell(Player.GetWorldLocation());
        var pirateCellPos = _brickMap.NonHiddenTilemap.WorldToCell(transform.position);
        if (playerCellPos.x > pirateCellPos.x) return true;
        return false;
    }

    
    
    void DragonChangeDirection()
    {
        Transform feelerToUse = _floorWallFeeler;
        if (_og_GoingUp) feelerToUse = _ceilingWallFeeler;

        if (HandleHittingABlock(feelerToUse))
        {
            _og_GoingUp = !_og_GoingUp;
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

    void DragonMove()
    {
        if (_og_GoingUp)
        {
            var vel = _rigidbody2D.velocity;
            vel.y = _flyingspeed;
            _rigidbody2D.velocity = vel;
        }
        else
        {
            var vel = _rigidbody2D.velocity;
            vel.y = -_flyingspeed;
            _rigidbody2D.velocity = vel;
        }
    }

    void CastFireball(bool isFacingRight, Vector3 transformPosition)
    {
        Vector2 direction = Vector2.left;
        if(isFacingRight) direction = Vector2.right;
        StartCoroutine(AnimateAndCastFireball(direction,transformPosition));
    }

    IEnumerator AnimateAndCastFireball(Vector2 direction, Vector3 transformPosition)
    {
        _isDragonAttacking = true;
        _skeletonAnimation.AnimationName = "Attack";
        yield return new WaitForSeconds(_delayToAttackSound);
        MasterAudio.PlaySound("Fire_Power_Buff_Activate");
        
        var position = transform.position;
        //position += _halfBrick;

        Log($"Casting Fire <color=red>Ball</color> at {Time.time}");
        var pbref = PoolBoss.SpawnInPool("DragonBall2", position, Quaternion.identity);
        if(pbref)
            Log($" something at {Time.time}");
        else
        {
            Log($" null for pb at {Time.time}");
        }
        var moveConsRef = pbref.GetComponent<MoveConstantSpeed>();
        moveConsRef.SetDirection(direction);
        _isDragonAttacking = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        bool isFacingRight = IsPlayerRightOfDragon(); 
        if (isFacingRight)
            SetEnemyFacingRight();
        else
            SetEnemyFacingLeft();

        if (Time.time > _FireBallCastAvailableTime)
        {
            if (IsPlayerOnSameYAsDragon())
            {
                if (_isDragonAttacking == false)
                {
                    _FireBallCastAvailableTime = Time.time + fireBallCoolDown;
                    CastFireball(isFacingRight, transform.position);
                }
            }
        }

        if (_isDragonAttacking == false)
        {
            DragonMove();
            DragonChangeDirection();
        }
        else
        {
            DragonDontMove();
        }
    }

    void DragonDontMove()
    {
        _rigidbody2D.velocity = Vector2.zero;        
    }
}
