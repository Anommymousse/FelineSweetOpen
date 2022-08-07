using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;
using UnityEngine.Tilemaps;
using static MyExtensions.MyExtensions;

public class EnemyBird : MonoBehaviour
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

    static int counterThing=0; 
    
    Vector2 _traveldirection;
    
    //Saved vars...
    Rigidbody2D _rigidbody2D;
    
    SpriteRenderer _spriteRenderer; //?
    BrickMap _brickMap;
    Vector2 _direction;
    //Animator _animatorBase;
    int _collisionLayermask;
    
    //Startup save position and direction
    Vector3 _saved_InitialStartDirection;
    Vector3 _saved_InitialStartPosition;
    Vector2 _saved_rigidbody_position;
    bool _isBirdRestarting;

    public void SetupRespawnBirb()
    {
        SetRigidBodyToZero();
        SetUpSpriteRenderer();
        GrabBrickMapRef();
        SetBirdInitialDirection();
        SetBirdInitialPosition();
        SetBirdInitialSpeedAndFlip();
    }

    public void SetRigidBodyToZero()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = Vector2.zero;
        _saved_rigidbody_position = _rigidbody2D.position;
    }

    void SetUpSpriteRenderer()
    {
        _spriteRenderer =  gameObject.GetComponent<SpriteRenderer>();
        if(_spriteRenderer==null)
            _spriteRenderer =  gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        SetRigidBodyToZero();
        SetUpSpriteRenderer();
        GrabBrickMapRef();
        SetBirdResetCallbacks();
        SetBirdInitialDirection();
        SetBirdInitialPosition();
        SetBirdInitialSpeedAndFlip();
        
        _leftWallFeeler = gameObject.transform.Find("LeftFeeler");
        _rightWallFeeler = gameObject.transform.Find("RightFeeler");//this.gameObject.GetComponentInChildren<RightFeeler>();
        _groundWallFeeler = gameObject.transform.Find("DownFeeler");
        _roofWallFeeler = gameObject.transform.Find("UpFeeler");
        
        //Yeah only bricks? maybe player?
        _collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");
        
        _isBirdRestarting = false;

        Log($" <color=red> BIRD STARTING {gameObject.name}</color>");
    }

    public void SetBirdInitialDirectionNoFlip()
    {
        _traveldirection = startDirection;
        _rigidbody2D.velocity = _traveldirection*speed;        
    }

    void SetBirdInitialSpeedAndFlip()
    {
        _traveldirection = startDirection;
        if (_traveldirection.x < -0.01f)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
        _rigidbody2D.velocity = _traveldirection*speed;
    }
    
    

    public void SetBirdInitialPosition()
    {
        _saved_InitialStartPosition = transform.position;
    }

    public void SetBirdInitialDirection()
    {
        _saved_InitialStartDirection = startDirection;
    }

    void GrabBrickMapRef()
    {
        var root = GameObject.Find("TilesBoss");
        _brickMap = root.GetComponentInChildren<BrickMap>();
    }

    void SetBirdResetCallbacks()
    {
        var wht = GameObject.Find("Player");
        Player _player = wht.GetComponent<Player>();
        _player.OnPlayerReset += ResetEnemy;
        _player.OnPlayerLevelChange += ClearAllEnemy;
    }


    void EnemyRestartGubbins()
    {
        if (gameObject.activeSelf is false) return;
        Log($" <color=red> BIRD RE-STARTING {gameObject.name}</color>");
        _isBirdRestarting = true; //Guard against update feelers while restarting
        
        SetRigidBodyToZero();
        SetUpSpriteRenderer();
        SetBirdInitialDirection();
        SetBirdInitialPosition();
        SetBirdInitialSpeedAndFlip();

        _isBirdRestarting = false;
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
        if (_isBirdRestarting) return;
        //Debug.Log($"<color=red> direction = {_traveldirection} </color>");
            if (_traveldirection == Vector2.up)
            {
                _traveldirection = Vector2.left;
                if (isClockwise) _traveldirection = Vector2.right;
                _rigidbody2D.velocity = _traveldirection * speed;
                _spriteRenderer.flipX = false;
                return;
            }
            if (_traveldirection == Vector2.down)
            {
                _traveldirection = Vector2.right;
                if (isClockwise) _traveldirection = Vector2.left;
                _rigidbody2D.velocity = _traveldirection * speed;
                _spriteRenderer.flipX = true;
                return;
            }
            if (_traveldirection == Vector2.left)
            {
                _traveldirection = Vector2.down;
                if (isClockwise) _traveldirection = Vector2.up;
                _rigidbody2D.velocity = _traveldirection * speed;
                _spriteRenderer.flipX = true;
                return;
            }
            if (_traveldirection == Vector2.right)
            {
                _traveldirection = Vector2.up;
                if (isClockwise) _traveldirection = Vector2.down;
                _rigidbody2D.velocity = _traveldirection * speed;
                _spriteRenderer.flipX = false;
                return;
            }
            
            Debug.LogError($"No direction found {gameObject.name} direction = {_traveldirection} {speed}");
    }
    
    //Handle the brick collision and movement changes.
    void OnCollisionEnter2D(Collision2D other)
    { 
        if (_isBirdRestarting) return;
        Debug.Log($"birb test 2d collision {other.collider.name}");
        Vector2 vector2direction = Vector2.zero;

        var thing = other.GetContact(0);
        var contactpoint = _brickMap.NonHiddenTilemap.WorldToCell(thing.point);
        if (_brickMap.NonHiddenTilemap.HasTile(contactpoint))
        {
            var tilename = _brickMap.NonHiddenTilemap.GetTile<Tile>(contactpoint);
            Debug.Log($"birb hits tile ?{tilename} {thing.point}");
            _brickMap.DestroyBrick(contactpoint);
        }

        if (staticBrickFullReverse)
            counterThing++;
        if (staticBrickFullReverse)
            Debug.Log($"Hit Counter={counterThing} </color>");
        
        //NextDirection();
        _rigidbody2D.velocity = _traveldirection*speed;

    }

    void UpdateBirdFeelers()
    {
        if (_isBirdRestarting) return;
        Transform feelerToUse = _rightWallFeeler;
        if(_traveldirection==Vector2.left) feelerToUse = _leftWallFeeler;
        if(_traveldirection==Vector2.up) feelerToUse = _roofWallFeeler;
        if(_traveldirection==Vector2.down) feelerToUse = _groundWallFeeler;
        
        var rchit = Physics2D.RaycastAll(feelerToUse.position, Vector2.down, 0.05f,_collisionLayermask);

        if (rchit.Length > 0)
        {
            Debug.Log($"hit ={rchit[0].collider.name}");
            
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
                            NextDirection();
                        }
                    }
                    _brickMap.DestroyBrick(contactpoint);
                }
                else
                {
                 //   Debug.LogError($"<color=red> No tile! </color>");
                }
            }

            NextDirection();
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateBirdFeelers();
        _rigidbody2D.velocity = _traveldirection*speed;
        
    }
}
