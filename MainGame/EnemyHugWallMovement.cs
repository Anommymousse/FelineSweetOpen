using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;

public class EnemyHugWallMovement : MonoBehaviour
{
    [SerializeField] bool isClockwise = true;
    float speed = 4.0f;
    
    float _defaultspeedrate = 1.25f;
    float _feelerScale = 1.0f;
    
    Transform _leftFeelerTransform;
    Transform _rightFeelerTransform;
    bool _wasLeftFeelerActive;
    bool _wasRightFeelerActive;
    bool _isLeftFeelerActive;
    bool _isRightFeelerActive;
    bool _findWallMode = true;

    int _collisionLayermask;
    Vector3 _storedStartPosition;
    Quaternion _storedStartRotation;

    void Start()
    {
        _storedStartPosition = transform.position;
        _storedStartRotation = transform.rotation;
        
        _collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");
        var _player = GameObject.Find("Player").GetComponent<Player>();
        _player.OnPlayerReset += ResetEnemy;
        _player.OnPlayerLevelChange += OnPlayerLevelChange;
        
        SetStatus();
    }

    void Awake()
    {
        
    }

    void SetStatus()
    {
        _leftFeelerTransform = gameObject.transform.Find("LeftFeeler").transform;
        _rightFeelerTransform = gameObject.transform.Find("RightFeeler").transform;
        _wasLeftFeelerActive = false;
        _isLeftFeelerActive = false;
        _wasRightFeelerActive = true;
        _isRightFeelerActive = true;
        _findWallMode = true;
        _feelerScale = speed / _defaultspeedrate;
        if (_feelerScale < 1.0) _feelerScale = 1.0f;
        if (isClockwise == false) _feelerScale = -_feelerScale;
        transform.position = _storedStartPosition;
        transform.rotation = _storedStartRotation;
    }

    void OnPlayerLevelChange() => PoolBoss.Despawn(this.transform);

    void ResetEnemy()
    {
        SetStatus();
    }

    void Rotate90Clockwise()
    {
        if(isClockwise)
            gameObject.transform.Rotate(0, 0, -90);
        else
            gameObject.transform.Rotate(0, 0, 90);
    }

    void RotateMinus90()
    {
        if(isClockwise)
            gameObject.transform.Rotate(0, 0, 90);
        else
            gameObject.transform.Rotate(0, 0, -90);
    }

    bool ActiveFeelGoneInActive()
    {
        if (_wasLeftFeelerActive != _isLeftFeelerActive) return true;
        if (_wasRightFeelerActive != _isRightFeelerActive) return true;
        return false;
    }

    bool InactiveFeelerActivated()
    {
        if (_wasLeftFeelerActive == false && _isLeftFeelerActive) return true;
        if (_wasRightFeelerActive == false && _isRightFeelerActive) return true;
        return false;
    }

    Vector3 GetAdjustedFeelerPosition(Vector3 feelerPosition)
    {
        var centerposition = gameObject.transform.position;
        var difference = feelerPosition-centerposition;
        return  difference * _feelerScale + centerposition;
    }

    Vector2 GetXandYmultipliers()
    {
        Vector2 XandYMult;
        var xrotation = gameObject.transform.eulerAngles.z;
        XandYMult = Vector2.right;
        if ((Mathf.Abs(xrotation) > 89f)&&(Mathf.Abs(xrotation)< 91f )) XandYMult = Vector2.up;
        if ((Mathf.Abs(xrotation) > 179f) && (Mathf.Abs(xrotation) < 181f)) XandYMult = Vector2.left;
        if ((Mathf.Abs(xrotation) > 269f) && (Mathf.Abs(xrotation) < 271f)) XandYMult = Vector2.down;
        
        return XandYMult;
    }
    
    Vector2 GetXandYmultipliersAntiClockwise()
    {
        Vector2 XandYMult;
        var xrotation = gameObject.transform.eulerAngles.z;
        XandYMult = Vector2.left;
        if ((Mathf.Abs(xrotation) > 89f)&&(Mathf.Abs(xrotation)< 91f )) XandYMult = Vector2.down;
        if ((Mathf.Abs(xrotation) > 179f) && (Mathf.Abs(xrotation) < 181f)) XandYMult = Vector2.right;
        if ((Mathf.Abs(xrotation) > 269f) && (Mathf.Abs(xrotation) < 271f)) XandYMult = Vector2.up;
        return XandYMult;
    }

    void CalculateNewFeelerCollisions()
    {
        var position = gameObject.transform.position;
        
        var leftFeelerPos = GetAdjustedFeelerPosition(_leftFeelerTransform.position);
        var rightFeelerPos = GetAdjustedFeelerPosition(_rightFeelerTransform.position);
        var lefthit = Physics2D.Linecast(position, leftFeelerPos,_collisionLayermask);
        var righthit = Physics2D.Linecast(position, rightFeelerPos,_collisionLayermask);
        Debug.DrawLine(position,leftFeelerPos,Color.cyan);
        Debug.DrawLine(position,rightFeelerPos,Color.red);
        _isLeftFeelerActive = false;
        _isRightFeelerActive = false;
        if (lefthit.collider != null) _isLeftFeelerActive = true;
        if (righthit.collider != null) _isRightFeelerActive = true;
    }

    void FixedUpdate()
    {
        CalculateNewFeelerCollisions();
        
        var o = gameObject;
        var rotation = o.transform.eulerAngles.z;
        Vector3 currentPosition = o.transform.position;
        
        if (InactiveFeelerActivated())
        {
            RotateMinus90();
            CalculateNewFeelerCollisions();
            _findWallMode = false;
        }
        else
        {
            if (_findWallMode)
            {
                if(isClockwise)
                    currentPosition.x += speed * Time.deltaTime * 1.0f;
                else
                    currentPosition.x -= speed * Time.deltaTime * 1.0f;
                currentPosition.y += speed * Time.deltaTime * 0.0f;
                gameObject.transform.position = currentPosition;
                _wasLeftFeelerActive = _isLeftFeelerActive;
                _wasRightFeelerActive = _isRightFeelerActive;
                return;
            }

            if (ActiveFeelGoneInActive())
            {
                Rotate90Clockwise();
                CalculateNewFeelerCollisions();
                _wasLeftFeelerActive = _isLeftFeelerActive;
                _wasRightFeelerActive = _isRightFeelerActive;
                _findWallMode = false;
            }
            else
            {
                if ((_isLeftFeelerActive == false) && (_isRightFeelerActive == false))
                {
                    if ((_wasLeftFeelerActive == false) && (_wasRightFeelerActive == false))
                    {
                        _findWallMode = true;
                    }
                    else
                    {
                        Rotate90Clockwise();
                        _findWallMode = false;
                    }
                }
            }
        }
        
        Vector2 xandYmultipliers = GetXandYmultipliers();
        if (isClockwise==false)
            xandYmultipliers *= -1.0f;
        currentPosition.x += speed * Time.deltaTime * xandYmultipliers.x;
        currentPosition.y += speed * Time.deltaTime * xandYmultipliers.y;
        gameObject.transform.position = currentPosition;
        
        _wasLeftFeelerActive = _isLeftFeelerActive;
        _wasRightFeelerActive = _isRightFeelerActive;
    }
}
