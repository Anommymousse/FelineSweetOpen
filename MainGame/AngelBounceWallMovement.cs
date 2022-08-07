using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using UnityEngine;

public class AngelBounceWallMovement : MonoBehaviour
{
    public float pingpongSpeed = 10;
    float x_direction = 1.0f;
    float y_direction = 1.0f;

    Vector3 _startPosition;
    float _startDirection;

    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    BrickMap _brickMap;
    Collider2D _collider2D;
    
    
    void Awake()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = Vector2.zero;
        _spriteRenderer =  gameObject.GetComponent<SpriteRenderer>();
        var root = GameObject.Find("TilesBoss");
        _brickMap = root.GetComponentInChildren<BrickMap>();

        var _player = GameObject.Find("Player").GetComponent<Player>();
        //_player.OnPlayerReset += ResetAngel;
        _player.OnPlayerLevelChange += ResetAngel;
        _startPosition = gameObject.transform.position;
        _startDirection = x_direction;
    }

    void ResetAngel()
    {
        PoolBoss.Despawn(this.transform);
    }

    public void X_ChangeDirection()
    {
        x_direction *= -1;
        if (x_direction < 0)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    { 
        Debug.Log($"{other.collider.name}");
        Vector2 vector2direction = Vector2.zero;

        if (other.collider.name == "Player")
        {
            CollectAngel();
        }
        
        
        var closestPoint = other.collider.ClosestPoint(transform.position);
        closestPoint = closestPoint - (Vector2) transform.position;
        /*if(Mathf.Abs(closestPoint.x) > 0.001f)
            Debug.Log($"<color=green>xx </color> {other.collider.name} tp{transform.position} cp{closestPoint} ");
        else
            Debug.Log($"<color=red> {other.collider.name} tp{transform.position} cp{closestPoint} </color>");*/
        
        if (Mathf.Abs(closestPoint.x) > 0.001f)
        {
            X_ChangeDirection();    
        }
        else
        {
            y_direction *= -1f;
        }
    }

    void CollectAngel()
    {
        PoolBoss.SpawnInPool("AngelPickup", transform.position, Quaternion.identity);
        PoolBoss.Despawn(this.transform);
        MasterAudio.PlaySound("Positive Effect 1_4");

        var kittyFund = GameObject.Find("[BOOTSTRAP]").GetComponent<KittyFund>();
        kittyFund.AngelMoneyPlusPlus();
        

    }

    void HandleVelocity()
    {
        Vector3 targetVelocity = new Vector2( pingpongSpeed * x_direction, pingpongSpeed*0.5f * y_direction );
       _rigidbody2D.velocity = targetVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        HandleVelocity();
    }
}
