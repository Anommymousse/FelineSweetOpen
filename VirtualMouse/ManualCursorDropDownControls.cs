using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ManualCursorDropDownControls : MonoBehaviour
{
    TMP_Dropdown _dropDownSelfRef;
    RectTransform _dropDownRectTransform;
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


    // Start is called before the first frame update
    void Start()
    {
        _dropDownSelfRef = GetComponent<TMP_Dropdown>();
        _dropDownRectTransform = GetComponent<RectTransform>();
        CreateGetButtonWidthHeight();
        needRectTransStill = true;
        _IsAdjustingSliderValueActive = false;

        _setupMenuBox = GetComponent<SetupMenuBox>();
        if (_setupMenuBox != null)
            _hasMenuBox = true;

        //GetButtonAreaInScreenCoordinates();//Note -: FUCKING UNITY MUST WAIT A FRAME BEFORE THE LAYOUT IS FIXED! Couldn't even do it in lateupdate...
    }

    void CreateGetButtonWidthHeight()
    {
        var rect = _dropDownRectTransform.rect;
        _width = rect.width;
        _height = rect.height;
    }

    //Must wait a frame to call this cause unity is bloody daft af
    void GetSliderAreaInScreenCoordinates()
    {
        _minx = _dropDownSelfRef.transform.position.x - _width / 2;
        _maxx = _dropDownSelfRef.transform.position.x + _width / 2;
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
            if (cursorControlRef.IsCursorMovingLeft())
            {
                //_dropDownSelfRef.value -= 0.03f;
            }

            if (cursorControlRef.IsCursorMovingRight())
            {
                //_dropDownSelfRef.value += 0.03f;
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

            //var testHandle = _dropDownSelfRef.handleRect.gameObject.GetComponent<Image>();
            //testHandle.color = Color.green;
        }
        else
        {
            if (_hasMenuBox)
                RemoveWiggle(_setupMenuBox);
            //var testHandle = _dropDownSelfRef.handleRect.gameObject.GetComponent<Image>();
            //testHandle.color = Color.white;
        }
    }
    
}