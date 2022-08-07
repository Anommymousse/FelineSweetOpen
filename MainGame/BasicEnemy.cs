using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class BasicEnemy : MonoBehaviour
{
    public float pingpongSpeed = 20;
    public bool isPingPongLeftRight=true;
    public bool canFall = true;
    public float groundCastRaySize=3.7f;
    public float wallCastRaySize=0.1f;
    public float lifeTimer = 10.0f;
    public bool groundKiller = false; 
    
    protected float _direction = 1.0f;

    //Reused stuffs.
    float _timerToStopReuseFailing;
    bool _currentPingPong;
    Vector3 _originalTrans=Vector3.zero;
    
    Vector3 _startPosition;
    float _startDirection= 1.0f;

    Color _startColor; 
    protected Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    protected BrickMap BrickMap;
    Collider2D _collider2D;
    bool _isGrounded;

    Transform leftWallFeeler;
    Transform rightWallFeeler;
    
    Transform groundLeft;
    Transform groundRight;
    Transform groundKillerPos;

    //float idleTimerWhenSpawned = 2.0f;
    
    bool isTimed = true;

    int _collisionLayermask;
    bool _isWalking;
    
    protected Animator _animatorBase;


    protected void SetIsGrounded(bool value)
    {
        _isGrounded = value;
    }
    protected void SetIsWalking(bool value)
    {
        _isWalking = value;
    }

    void Awake()
    {
        _direction = 1.0f;
        _startDirection = 1.0f;
    }

    void WasInStartMoveToEnable()
    {
        bool nullOrNot = false;
        if (_originalTrans == Vector3.zero)
        {
            //Debug.Log($"<color=red> og scale {gameObject.transform.localScale} {gameObject.name} </color>");
            _originalTrans = gameObject.transform.localScale;
            nullOrNot = true;
        }
        else
        {
            gameObject.transform.localScale = _originalTrans;
            nullOrNot = false;
        }

        if (gameObject.transform.localScale.x < 0)
        {
            Debug.LogError($"WTH null or not{nullOrNot} {gameObject.transform.localScale} {_originalTrans}");
        }

        _timerToStopReuseFailing = lifeTimer;
        
        //Grab animator
        _animatorBase = gameObject.GetComponentInChildren<Animator>();

        //Enemy ref
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = Vector2.zero;
        _spriteRenderer =  gameObject.GetComponent<SpriteRenderer>();
        
        if(_spriteRenderer==null)
            _spriteRenderer =  gameObject.GetComponentInChildren<SpriteRenderer>();
        
        if(_spriteRenderer!=null)
            _startColor = _spriteRenderer.color;
        
        //Grab bricks ref
        var root = GameObject.Find("TilesBoss");
        BrickMap = root.GetComponentInChildren<BrickMap>();
        
        //Grab player
        var wht = GameObject.Find("Player");
        Player _player = wht.GetComponent<Player>();
        _player.OnPlayerReset += ResetEnemy;
        _player.OnPlayerLevelChange += ClearAllEnemy;
        _startPosition = gameObject.transform.position;
        
        _direction = _startDirection; 

        leftWallFeeler = gameObject.transform.Find("LeftWallFeeler");//this.gameObject.GetComponentInChildren<LeftFeeler>();
        rightWallFeeler = gameObject.transform.Find("RightWallFeeler");//this.gameObject.GetComponentInChildren<RightFeeler>();
        groundLeft = gameObject.transform.Find("GroundLeft");
        groundRight = gameObject.transform.Find("GroundRight");

        if (groundKiller)
            groundKillerPos = gameObject.transform.Find("GroundDestruct");
        
        
        //Yeah only bricks? maybe player?
        _collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");


        /*destroyCountdownTimer = 10.0f;
        if (isTimed)
        {
            StartCoroutine(DestroyWhenTimerDone());
        }*/

    }
    
    protected virtual void OnEnable()
    {
        WasInStartMoveToEnable();
        
        if (isTimed)
        {
            StartCoroutine(DestroyWhenTimerDone());
        }

        if (_spriteRenderer != null)
            _spriteRenderer.color = _startColor;
    }

    void OnDisable()
    {
        StopCoroutine(DestroyWhenTimerDone());
    }

    IEnumerator DestroyWhenTimerDone()
    {
        while (_timerToStopReuseFailing > 0.0)
        {
            yield return null;
            _timerToStopReuseFailing -= Time.deltaTime;

            if (_timerToStopReuseFailing < 3.0f)
            {
                if (_spriteRenderer != null)
                {
                    var scolor = _spriteRenderer.color;
                    scolor.a = 1 - ((3.0f - lifeTimer) / 3.0f);
                    if (scolor.a < 0.1f) scolor.a = 0.1f;
                    _spriteRenderer.color = scolor;
                }
            }
        }

        PoolBoss.Despawn(transform);
    }


    void ResetEnemy()
    {
        if( isTimed ) 
            PoolBoss.Despawn(transform);
        else
        {
            gameObject.transform.position = _startPosition;
            _direction = _startDirection;
        }
    }

    void ClearAllEnemy()
    {
        
        PoolBoss.Despawn(transform);
    }

    protected void SwapDirection()
    {
        _direction *= -1;
        if (_spriteRenderer == null)
        {
            var getlLocalScale = gameObject.transform.localScale;
            getlLocalScale.x *= -1f;
            gameObject.transform.localScale = getlLocalScale;
        }
        else
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX; 
        }
    }
    
    void TestLeftAndRightColliders()
    {
        var leftstufffound = Physics2D.RaycastAll(leftWallFeeler.transform.position, Vector2.left, wallCastRaySize,_collisionLayermask);
        var rightstufffound = Physics2D.RaycastAll(rightWallFeeler.transform.position, Vector2.right, wallCastRaySize,_collisionLayermask);

        if (transform.localScale.x < 0.0f)
        {
            //I've never seen this before
            (leftstufffound, rightstufffound) = (rightstufffound, leftstufffound);
        }
        
        
        if (groundKiller)
        {
            var groundStuffound = Physics2D.RaycastAll(groundKillerPos.transform.position, Vector2.down,
                wallCastRaySize, _collisionLayermask);
            if (groundStuffound.Length > 0)
            {
                BrickMap.DestroyBrick(groundKillerPos.transform.position);
            }
        }


        var zcle = rightWallFeeler.transform.position + Vector3.right * 2.0f;
        Debug.DrawLine(rightWallFeeler.transform.position,zcle,Color.green);

        var leftposition = leftWallFeeler.transform.position + (Vector3)(Vector2.left * wallCastRaySize);
        var rightposition = rightWallFeeler.transform.position + (Vector3)(Vector2.right * wallCastRaySize);

        if ((rightstufffound.Length > 0)||(leftstufffound.Length>0))
        {
            if ((_direction > 0) && (rightstufffound.Length > 0))
            {
                if(!groundKiller)
                    BrickMap.DestroyBrick(rightposition);
                SwapDirection();
            }

            if ((_direction < 0) && (leftstufffound.Length > 0))
            {
                if(!groundKiller)
                    BrickMap.DestroyBrick(leftposition);
                SwapDirection();
            }
        }
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D other)
    { 
        Debug.Log($"{other.collider.name}");
        Vector2 vector2direction = Vector2.zero;

        var thing = other.GetContact(0);
        
        var contactpoint = BrickMap.NonHiddenTilemap.WorldToCell(thing.point);
        if (BrickMap.NonHiddenTilemap.HasTile(contactpoint))
        {
            var tilename = BrickMap.NonHiddenTilemap.GetTile<Tile>(contactpoint);
            Debug.Log($"enemy tile ?{tilename} {thing.point}");
            BrickMap.DestroyBrick(contactpoint);
        }
        
    }


    void HandleLeftRightVelocity()
    {
        TestLeftAndRightColliders();
        
        if (isPingPongLeftRight)
        {
            Vector3 targetVelocity = new Vector2(pingpongSpeed * _direction, 0);
            _rigidbody2D.velocity = targetVelocity;
        }
        else
        {
            Vector3 targetVelocity = new Vector2(0,pingpongSpeed * _direction);
            _rigidbody2D.velocity = targetVelocity;
        }
    }

    void UpdateIsGrounded()
    {
        _isGrounded = false;
        var positionToTest = groundLeft.position;
        var rchit = Physics2D.RaycastAll(positionToTest, Vector2.down, 0.05f,_collisionLayermask);
        if (rchit.Length>0) _isGrounded = true;
        var endline = positionToTest + Vector3.down;
        Debug.DrawLine(positionToTest,endline,Color.cyan);
        
        positionToTest = groundRight.position;
        rchit = Physics2D.RaycastAll(positionToTest, Vector2.down, 0.05f,_collisionLayermask);
        if (rchit.Length>0) _isGrounded = true;
        
    }

    protected void OncePerFrameMovement()
    {
        UpdateIsGrounded();
        
        if (canFall)
        {
            if (!_isGrounded)
            {
                var enemySpd = _rigidbody2D.velocity;
                enemySpd.x = 0f;
                _rigidbody2D.velocity = enemySpd;
                return;
            }
        }
        
        HandleLeftRightVelocity();
        
        if(( isTimed )&&(lifeTimer<0.0))
            PoolBoss.Despawn(transform);

    }
    // Update is called once per frame
    void Update()
    {
    }
}
