using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongAnimationBirb : MonoBehaviour
{
    [SerializeField] float MovementSpeed;
    bool _goingLeft = true;
    int _width;
    SpriteRenderer _spriteRenderer;
    Vector3 _storePosition;
    Vector3 _startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _width = Screen.width;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _storePosition = transform.position;
        _startPosition = _storePosition;
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderer.flipX = _goingLeft;
        if (_goingLeft)
        {
            _storePosition.x = _storePosition.x - MovementSpeed * Time.deltaTime;
        }
        else
        {
            _storePosition.x = _storePosition.x + MovementSpeed * Time.deltaTime;
            if (_storePosition.x >= _startPosition.x)
            {
                _goingLeft = !_goingLeft;
                _storePosition.x = _startPosition.x;
                _storePosition.x--;
            }
        }

        transform.position = _storePosition;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided?");
        _goingLeft = !_goingLeft;
    }
}
