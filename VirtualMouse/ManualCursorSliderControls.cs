using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ManualCursorSliderControls : MonoBehaviour
{
    Slider _sliderSelfRef;
    RectTransform _sliderRectTransform;
    public ManualCursorMouseAndGamepad cursorControlRef;
    float _minx;
    float _miny;
    float _maxx;
    float _maxy;
    float _width;
    float _height;

    bool needRectTransStill;
    bool startup = false;
    bool doneTheCoordinates;

    bool _isSliderHighlighted;

    //bool _insideStateLastFrame = false;
    bool _hasMenuBox = false;
    SetupMenuBox _setupMenuBox;

    bool _IsAdjustingSliderValueActive;

    const string CursorID_X = "GamepadSpdX";
    const string CursorID_Y = "GamepadSpdY";
    float SpdX_Multiplier = 0.0f;
    float SpdY_Multiplier = 0.0f;

    public void LoadSpdMultipliers()
    {
        SpdX_Multiplier = PlayerPrefs.GetFloat(CursorID_X, 0.5f);
        SpdY_Multiplier = PlayerPrefs.GetFloat(CursorID_Y, 0.5f);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _sliderSelfRef = GetComponent<Slider>();
        _sliderRectTransform = GetComponent<RectTransform>();
        CreateGetButtonWidthHeight();
        needRectTransStill = true;
        _IsAdjustingSliderValueActive = false;

        _setupMenuBox = GetComponent<SetupMenuBox>();
        if (_setupMenuBox != null)
            _hasMenuBox = true;

        LoadSpdMultipliers();

        //GetButtonAreaInScreenCoordinates();//Note -: FUCKING UNITY MUST WAIT A FRAME BEFORE THE LAYOUT IS FIXED! Couldn't even do it in lateupdate...
    }

    void CreateGetButtonWidthHeight()
    {
        var rect = _sliderRectTransform.rect;
        _width = rect.width;
        _height = rect.height;
    }

    //Must wait a frame to call this cause unity is bloody daft af
    void GetSliderAreaInScreenCoordinates()
    {
        _minx = _sliderSelfRef.transform.position.x - _width / 2;
        _maxx = _sliderSelfRef.transform.position.x + _width / 2;
        _miny = transform.position.y - _height / 2;
        _maxy = transform.position.y + _height / 2;
    }

    bool DetectIfInSliderArea(float x, float y)
    {
        if ((x > _minx) && (x < _maxx) && (y > _miny) && (y < _maxy))
        {
            return true;
        }

        return false;
    }


    // Update is called once per frame
    void Update()
    {
        if (needRectTransStill)
        {
            if (startup)
            {
                GetSliderAreaInScreenCoordinates();
                needRectTransStill = false;
            }

            if (startup == false) startup = true;
            if (needRectTransStill) return;
        }


        //Only go on if we have a valid rect transform from the layout group(End Of Frame-as unity can't code either) 
        var cursorPosition = cursorControlRef.GetManualCursorCoords();

        HandleMovementOfSlider();

        if (DetectIfInSliderArea(cursorPosition.x, cursorPosition.y))
        {
            _isSliderHighlighted = true;
        }
        else
        {
            _isSliderHighlighted = false;
        }

        HandleSliderHighlightThings();
    }
    
    void HandleMovementOfSlider()
    {
        if ((_isSliderHighlighted) && (_IsAdjustingSliderValueActive == false))
        {
            if (cursorControlRef.IsCursorActivated())
            {
                _IsAdjustingSliderValueActive = true;
            }
        }
        
        if (!cursorControlRef.IsCursorActivated()) _IsAdjustingSliderValueActive = false;

        if (_IsAdjustingSliderValueActive)
        {
            //Magic number kludge?
            if (cursorControlRef.IsCursorMovingLeft())
            {
                _sliderSelfRef.value -= 1.0f * (1.0f+ SpdX_Multiplier) * Time.unscaledDeltaTime;
            }

            if (cursorControlRef.IsCursorMovingRight())
            {
                _sliderSelfRef.value += 1.0f * (1.0f+ SpdX_Multiplier) * Time.unscaledDeltaTime;
            }
        }
    }
    
    void SetUpWiggle(SetupMenuBox setupMenuBox)
    {
        string displayedText = setupMenuBox._menuText;
        if (!displayedText.StartsWith("<"))
        {
            MasterAudio.PlaySoundAndForget("UI_Click_Metallic_mono");
            setupMenuBox._menuText = "<wiggle a=.5>" + displayedText + "</wiggle>";
            setupMenuBox.UpdateText();
        }
    }

    string StripAnimationOut(string inputString)
    {
        if (!inputString.StartsWith("<")) return inputString;

        int partone = inputString.IndexOf(">");
        int parttwo = inputString.LastIndexOf("<");
        int length = parttwo - partone - 1;
        int firstText = partone + 1;
        return inputString.Substring(firstText, length);
    }

    void RemoveWiggle(SetupMenuBox setupMenuBox)
    {
        var oldText = _setupMenuBox._menuText;
        _setupMenuBox._menuText = StripAnimationOut(_setupMenuBox._menuText);

        if (oldText != _setupMenuBox._menuText)
            _setupMenuBox.UpdateText();
    }

    void HandleSliderHighlightThings()
    {
        if (_isSliderHighlighted)
        {
            if (_hasMenuBox)
                SetUpWiggle(_setupMenuBox);

            var testHandle = _sliderSelfRef.handleRect.gameObject.GetComponent<Image>();
            testHandle.color = Color.green;
        }
        else
        {
            if (_hasMenuBox)
                RemoveWiggle(_setupMenuBox);
            var testHandle = _sliderSelfRef.handleRect.gameObject.GetComponent<Image>();
            testHandle.color = Color.white;
        }
    }
    
}