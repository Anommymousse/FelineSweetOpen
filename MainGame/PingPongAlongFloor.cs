using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;

public class PingPongAlongFloor : MonoBehaviour
{
    Transform _leftFeeler;
    Transform _rightFeeler;
    SpriteRenderer _spriteRenderer;
    
    int _collisionLayermask;
    
    bool _leftOrRight;
    float _direction = -1.0f;
    
    float _speedScale = 1.0f;
    public float speed = 0.003f;
    float safeSpeed = 12.5f;
    public float chargeSpeed = 50.0f;
    
    bool _isPingPongMode = true;
    
    Vector3 _storedStartPosition;
    Transform _storedLeftFeeler;
    Transform _storedRightFeeler;
    Transform _storedHammerDetails;
    Vector3 _storedHammerDetailsPosition;
    BrickMap _brickMapRef;

    Rigidbody2D _storedRigidbody2D;

    bool _resetOneFrame = false;
    
    void Start()
    {
        _brickMapRef = GameObject.Find("TilesBoss").GetComponent<BrickMap>();
        
        _leftFeeler = transform.Find("LeftFeeler");
        _rightFeeler = transform.Find("RightFeeler");
        
        _leftFeeler = AdjustFeelerForSpeed(_leftFeeler);
        _rightFeeler = AdjustFeelerForSpeed(_rightFeeler);
        
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");

        _isPingPongMode = true;
        
        var _player = GameObject.Find("Player").GetComponent<Player>();
        _player.OnPlayerReset += RestartEnemy;
        _storedStartPosition = gameObject.transform.position;
        _storedLeftFeeler = _leftFeeler;
        _storedRightFeeler = _rightFeeler;
        _storedHammerDetailsPosition = transform.localPosition;
        _storedRigidbody2D = GetComponent<Rigidbody2D>();
        _resetOneFrame = true;
        _storedRigidbody2D.useFullKinematicContacts = true;
    }

    void RestartEnemy()
    {
        gameObject.transform.position = _storedStartPosition;
        _leftFeeler = _storedLeftFeeler;
        _rightFeeler = _storedRightFeeler;
        _isPingPongMode = true;
        _direction = -1.0f;
        _resetOneFrame = true;
    }

    //Might be dicey = self?
    Transform AdjustFeelerForSpeed(Transform feeler)
    {
        Vector3 newFeelerPosition = feeler.position;
        float xdiff = feeler.position.x - gameObject.transform.position.x;
        _speedScale = speed / safeSpeed;
        if (_speedScale < 1.0f) _speedScale = 1.0f;
        float adjustedXDiff = xdiff * _speedScale;
        newFeelerPosition.x += adjustedXDiff;
        feeler.transform.position = newFeelerPosition;
        return feeler.transform;
    }

    void ChangeMovementToChargeMode()
    {
        
    }
    
    void UpdateChargeMovement()
    {
        
    }
    
    void UpdatePingPongMovement()
    {
        var leftFeelerPos = _leftFeeler.position;
        var rightFeelerPos = _rightFeeler.position;
        var position = gameObject.transform.position;
        var lefthit = Physics2D.Linecast(position, leftFeelerPos,_collisionLayermask);
        var righthit = Physics2D.Linecast(position, rightFeelerPos,_collisionLayermask);
        Debug.DrawLine(position,leftFeelerPos,Color.cyan);
        Debug.DrawLine(position,rightFeelerPos,Color.red);

        if (_resetOneFrame)
        {
            _storedRigidbody2D.velocity = Vector2.zero;
            _storedRigidbody2D.rotation = 0;
        }
        
        if ((righthit.collider == null) && (lefthit.collider == null))
        {
            Debug.Log("Destroyed!");
            //PoolBoss.Despawn(this.transform);
        }

        
        if (((_direction > 0) && (righthit.collider != null)) || ((lefthit.collider != null) && (_direction < 0)))
        {

            string thing="Blank";
            if ((_direction > 0) &&(righthit.collider != null))
                thing = righthit.collider.name;
            
            if ((_direction < 0) &&(lefthit.collider != null))
                thing = lefthit.collider.name;
            
            if( thing.Contains("Outside")|| thing.Contains("Invunerable"))
            {
                _direction *= -1;
            }
            else
            {
                position.x += speed * _direction * Time.deltaTime;
                _storedRigidbody2D.MovePosition(position);                
            }
        }
        else
        {
            _direction *= -1;
        }

    }

    void UpdateMovementType()
    {
     
    }

    void FixedUpdate()
    {
        if (_isPingPongMode)
        {
            UpdatePingPongMovement();
        }
        else
        {
            UpdateChargeMovement();
        }
        UpdateMovementType();
    }

    
}
