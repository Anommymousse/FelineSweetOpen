using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static MyExtensions.MyExtensions;

public class ManualCursorMouseAndGamepad : MonoBehaviour
{
    const float _defaultWidth = 1920.0f;
    const float _defaultHeight = 1040.0f;
    
    Mouse _currentMouse;
    PlayerInput _playerInput;
    public GameObject _cursorImageToControl;
    Vector2 _cursorPosition;
    Vector2 _cursorPositionScreenSpace;
    public float _scaleSpeed_x=3.0f;
    public float _scaleSpeed_y=3.0f;
    public float _padding=25.0f;

    public Vector2 GetManualCursorCoords()
    {
        return _cursorPositionScreenSpace;
    }

    public bool WasCursorActivatedThisFrame() => WasCursorPressedThisFrame();
    public bool IsCursorActivated() => IsCursorPressed();

    public bool IsCursorMovingLeft() => IsMouseOrPadGoingLeft();
    public bool IsCursorMovingRight() => IsMouseOrPadGoingRight();

    const string CursorID_X = "GamepadSpdX";
    const string CursorID_Y = "GamepadSpdY";

    const float RaiseSpeedABit = 3.0f; 
    
    static float SpdX_Multiplier = 1.0f;
    static float SpdY_Multiplier = 1.0f;
    
    public static void SetNewXSpeedMultiplier(float newMult)
    {
        SpdX_Multiplier = newMult*RaiseSpeedABit;
        Log($"xspeed set {SpdX_Multiplier}");
    }
    public static void SetNewYSpeedMultiplier(float newMult)
    {
        SpdY_Multiplier = newMult*RaiseSpeedABit;
        Log($"yspeed set {SpdY_Multiplier}");
    }

    public void LoadSpdMultipliers()
    {
        SpdX_Multiplier = PlayerPrefs.GetFloat(CursorID_X, 0.5f) * RaiseSpeedABit;
        SpdY_Multiplier = PlayerPrefs.GetFloat(CursorID_Y, 0.5f) * RaiseSpeedABit;
    }
    
    bool IsMouseOrPadGoingLeft()
    {
        if(Gamepad.current!=null)
            if (Gamepad.current.leftStick.left.isPressed) return true;
        if (_currentMouse.delta.x.ReadValue() < -0.1f) return true;
        return false;
    }
    
    bool IsMouseOrPadGoingRight()
    {
        if(Gamepad.current!=null)
            if (Gamepad.current.leftStick.right.isPressed) return true;
        if (_currentMouse.delta.x.ReadValue() > 0.1f) return true;
        return false;
    }

    bool IsMouseOrPadHeldDown()
    {
        if(Gamepad.current!=null)
            if (Gamepad.current.buttonSouth.isPressed) return true;
        if (_currentMouse.leftButton.isPressed) return true;

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentMouse = Mouse.current;
        _playerInput = PlayerInput.GetPlayerByIndex(0);

        _cursorPosition = new Vector2(0, 0);
        LoadSpdMultipliers();
    }

    bool IsCursorPressed()
    {
        if(Gamepad.current!=null)
            if (Gamepad.current.aButton.isPressed) return true;
        if (_currentMouse.leftButton.isPressed) return true;
        return false;
    }

    bool WasCursorPressedThisFrame()
    {
        if(Gamepad.current!=null)
            if (Gamepad.current.aButton.wasPressedThisFrame) return true;
        if (_currentMouse.leftButton.wasPressedThisFrame) return true;
        return false;
    }

    //Move position by gamepad
    //Move position by mouse as normal
    
    void MoveCursorByGamepad()
    {
        var xAdj=0.0f;
        var yAdj=0.0f;
        if (Gamepad.current == null) return;
        
        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        if (Mathf.Abs(deltaValue.x) > 0.01f)
        {
            xAdj = _scaleSpeed_x * deltaValue.x *SpdX_Multiplier * 120.0f * Time.unscaledDeltaTime;
        }
        if (Mathf.Abs(deltaValue.y) > 0.01f)
        {
            yAdj = _scaleSpeed_y * deltaValue.y *SpdY_Multiplier * 120.0f * Time.unscaledDeltaTime;
        }

        _cursorPosition.x += xAdj;
        _cursorPosition.y += yAdj;
    }

    void MoveCursorByMouse()
    {
        var xAdj=0.0f;
        var yAdj=0.0f;
        Vector2 deltaValue = Mouse.current.delta.ReadValue();
        if (Mathf.Abs(deltaValue.x) > 0.01f)
        {
            xAdj = _scaleSpeed_x * deltaValue.x;
        }
        if (Mathf.Abs(deltaValue.y) > 0.01f)
        {
            yAdj = _scaleSpeed_y * deltaValue.y;
        }

        _cursorPosition.x += xAdj;
        _cursorPosition.y += yAdj;
    }

    void ClampTheCursorToScreen()
    {
        _cursorPosition.x = Mathf.Clamp(_cursorPosition.x, _padding-(_defaultWidth/2.0f), _defaultWidth/2.0f   - _padding);
        _cursorPosition.y = Mathf.Clamp(_cursorPosition.y, _padding-(_defaultHeight/2.0f), _defaultHeight/2.0f - _padding);
    }

    void UpdateTheCursorPosition()
    {
        //Screen to local?
        _cursorImageToControl.transform.localPosition = _cursorPosition;
        _cursorPositionScreenSpace = _cursorPosition;
        _cursorPositionScreenSpace.x += _defaultWidth / 2.0f;
        _cursorPositionScreenSpace.y += _defaultHeight / 2.0f;
        //Now scale to actual screen?
        _cursorPositionScreenSpace.x *= Screen.width / _defaultWidth;
        _cursorPositionScreenSpace.y *= Screen.height / _defaultHeight;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        MoveCursorByGamepad();
        MoveCursorByMouse();
        ClampTheCursorToScreen();
        UpdateTheCursorPosition();

        if (WasCursorPressedThisFrame())
        {
            //To screen cooridnates
            var adjCursor = _cursorPosition;
            
            Log($"<color=red> Pre Adj = {_cursorPosition} </color>");
            
            adjCursor.x += _defaultWidth / 2.0f;
            adjCursor.y += _defaultHeight / 2.0f;
            
            Log($"Adjusted Button was pressed at ${adjCursor}");
            Log($"<color=green> Screen space = {_cursorPositionScreenSpace} </color>");
            
            Log($"Mouse cursor visibilty = {Cursor.visible} lockstate = {Cursor.lockState.ToString()}");
        }
    }
}
