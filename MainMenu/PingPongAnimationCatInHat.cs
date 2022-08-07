using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongAnimationCatInHat : MonoBehaviour
{
    [SerializeField] float MovementSpeed;
    [SerializeField] Transform _LeftSensor;
    [SerializeField] Transform _RightSensor;
    int _direction = -1;
    
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidbody2D;
    Vector3 _storePosition;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _storePosition = transform.position;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.velocity = new Vector2(_direction, _rigidbody2D.velocity.y);
        _spriteRenderer.flipX = (_direction<0);
        SensorScan(_direction > 0 ? _RightSensor : _LeftSensor);
        _storePosition.x = _storePosition.x + MovementSpeed* _direction * Time.deltaTime;
        _storePosition.y = transform.position.y;
        transform.position = _storePosition;
    }

    void SensorScan(Transform sensor)
    {
        Debug.DrawRay(sensor.position, Vector2.down * 3.0f, Color.red);
        var result = Physics2D.Raycast(sensor.position, Vector2.down, 3.0f);
        if (result.collider == null)
        {
            TurnAround();
        }
    }

    void TurnAround()
    {
        _direction = -_direction;
    }
    
}
