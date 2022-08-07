using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class EnemyPingPong : MonoBehaviour
{
    public float pingpongSpeed = 20;
    public bool isLeftRight=true;
    float direction = 1.0f;

    Vector3 _startPosition;
    public float startDirection=1.0f;

    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    BrickMap _brickMap;

    Collider2D _collider2D;

    Player _playerRef;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = Vector2.zero;
        _spriteRenderer =  gameObject.GetComponent<SpriteRenderer>();
        var root = GameObject.Find("TilesBoss");
        _brickMap = root.GetComponentInChildren<BrickMap>();

        _playerRef = GameObject.Find("Player").GetComponent<Player>();
        _playerRef.OnPlayerReset += ResetEnemy;
        _startPosition = gameObject.transform.position;

        var getlLocalScale = gameObject.transform.localScale;
        if(getlLocalScale.x <0)
            getlLocalScale.x *= -1f;
        gameObject.transform.localScale = getlLocalScale; // problem?
        
        if (Mathf.Epsilon.Equals(startDirection) )
        {
            direction = 1.0f;
        }
        else
        {
            
            direction = startDirection;
        }
    }
    
    void OnDisable()
    {
        
        if (!_playerRef) return;
        if(_playerRef)
            _playerRef.OnPlayerReset -= ResetEnemy;
    }

    void ResetEnemy()
    {
        gameObject.transform.position = _startPosition;
        direction = startDirection;
        
        var getlLocalScale = gameObject.transform.localScale;
        if(getlLocalScale.x <0)
            getlLocalScale.x *= -1f;
        gameObject.transform.localScale = getlLocalScale; // problem?
        
    }

    public void SwapDirection()
    {
        if (_spriteRenderer == null)
        {
            var getlLocalScale = gameObject.transform.localScale;
            getlLocalScale.x *= -1f;
            gameObject.transform.localScale = getlLocalScale; // problem?
            
            if(gameObject.transform.localScale.x < 0)
                direction = -1f;
            else
                direction = 1f;
            return;
        }
        
        direction *= -1;
        if (direction < 0)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
    }

    
    void OnCollisionEnter2D(Collision2D other)
    { 
        Debug.Log($"{other.collider.name}");
        Vector2 vector2direction = Vector2.zero;

        var thing = other.GetContact(0);
        Vector3 pointOfContact = thing.point;

        if (isLeftRight)
        {
            if (direction < 0)
                pointOfContact.x -= 0.4f;
            else
                pointOfContact.x += 0.4f;
        }
        else
        {
            if (direction < 0)
                pointOfContact.y -= 0.4f;
            else
                pointOfContact.y += 0.4f;
        }

        _brickMap.DestroyBrick(pointOfContact);         
        
        SwapDirection();
    }
    
    void HandleVelocity()
    {
        if (isLeftRight)
        {
            Vector3 targetVelocity = new Vector2(pingpongSpeed * direction, 0);
            _rigidbody2D.velocity = targetVelocity;
        }
        else
        {
            Vector3 targetVelocity = new Vector2(0,pingpongSpeed * direction);
            _rigidbody2D.velocity = targetVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleVelocity();
    }

}
