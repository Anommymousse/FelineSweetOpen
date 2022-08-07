using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using static MyExtensions.MyExtensions;
public class BubbleCatController : MonoBehaviour
{
    public float energyLeft = 100.0f;
    public float speedForward = 30.0f;
    public float horizontalSpeed = 20.0f;
    public Rigidbody bubbleCatRb;
    public GameObject playerInputObject;

    public Transform _graphicsForBall;
    
    PlayerInput _playerInput;
    float _left;
    float _right;
    float _down;
    float _escape;

    float horizontalMove;
    bool _jumpDownThisFrame;
    bool _canJump;


    void DisablePlayerControlEvents()
    {
        _playerInput.actions["Jump"].performed -= PlayerJump;
        _playerInput.actions["Esc"].performed -= EscapePressed;
        _playerInput.actions["BuildDown"].performed -= PlayerBuildBlockDown;
        _playerInput.actions["Dash"].performed -= PlayerDash;
    }

    void PlayerDash(InputAction.CallbackContext obj)
    {
    }

    void PlayerBuildBlockDown(InputAction.CallbackContext obj)
    {
    }

    void EscapePressed(InputAction.CallbackContext obj)
    {
    }

    void PlayerJump(InputAction.CallbackContext obj)
    {
        _jumpDownThisFrame = true;
    }

    void SetupPlayerControls()
    {
        //Controls Setup.
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Left"].performed += context => _left = context.ReadValue<float>();
        _playerInput.actions["Left"].canceled += context => _left = 0.0f;
        _playerInput.actions["Right"].performed += context => _right = context.ReadValue<float>();
        _playerInput.actions["Right"].canceled += context => _right = 0.0f;
        _playerInput.actions["Jump"].performed += PlayerJump;
        _playerInput.actions["Esc"].performed += EscapePressed;
        _playerInput.actions["BuildDown"].performed += PlayerBuildBlockDown;
        _playerInput.actions["Dash"].performed += PlayerDash;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        bubbleCatRb.velocity = Vector3.forward * speedForward;
        
    }

    void OnEnable()
    {
        SetupPlayerControls();
    }

    void OnDisable()
    {
        //DisablePlayerControlEvents();
    }

    bool OnGroundDetect()
    {
        var things = Physics.RaycastAll(gameObject.transform.position, Vector3.down, 5.0f);
        return things.Length > 0;
    }
    
    void UpdateCanJump(bool onGround)
    {
        if (!onGround) _canJump = false;
        if (bubbleCatRb.velocity.y < 0) _canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_left>0.01f) horizontalMove = -1;
        if (_right>0.01f) horizontalMove = 1;
        if ((_left > 0.01f) && (_right > 0.01f)) horizontalMove = 0;
        
        UpdateCanJump(OnGroundDetect());

        /*if (_jumpDownThisFrame == true)
        {
            Log($" jump status = {_jumpDownThisFrame} at {Time.time}");
        }*/
    }

    void FixedUpdate()
    {
        var currentAngles = _graphicsForBall.localRotation.eulerAngles;
        currentAngles.y += 5f;
        _graphicsForBall.localRotation.Set(currentAngles.x,currentAngles.y,currentAngles.z,1.0f);
        
        var currentvel = bubbleCatRb.velocity;
        currentvel.x = horizontalMove * horizontalSpeed;
        bubbleCatRb.velocity = currentvel;

        if (currentvel.z < 0)
        {
            currentvel.z = 0;
            bubbleCatRb.AddForce(0, 0f, 40);
        }

        
        //bubbleCatRb.AddForce(20f*horizontalMove,0,0);

        if (_jumpDownThisFrame && _canJump)
        {
            Log($" jump attempt at {Time.time}");
            bubbleCatRb.AddForce(0, 10000f, 0);
        }
        Log($" vel = {bubbleCatRb.velocity} ");

        _jumpDownThisFrame = false;
    }
}
