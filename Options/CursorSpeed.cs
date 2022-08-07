using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorSpeed : MonoBehaviour
{
    [SerializeField] string _channelID = "GamepadSpdX";
    [SerializeField] Slider _slider;
    //float _oldSliderValue = 0.5f;
    float _defaultValue = 0.5f;
    float _Speed = 0.5f;
    static bool _fnDisabled = false;

    void OnDisable()
    {
        PlayerPrefs.SetFloat(_channelID, _slider.value);
        _fnDisabled = false;
    }

    void Awake() => SetSliderValue(PlayerPrefs.GetFloat(_channelID, _defaultValue));

    public void SetSliderValue(float sliderValue)
    {
        //Disable to stop infinite calls...
        //Set slider position, Set volume, Set Tick
        if (_fnDisabled) return;
        _fnDisabled = true;
        
        //Should not happen but if file corruption 
        if (sliderValue < 0.05f) sliderValue = 0.05f;
        _slider.value = sliderValue;

        //NB. Default value is multiplied inside move to maintain 1.0 for default of 0.5
        if (_channelID.Contains("X"))
            ManualCursorMouseAndGamepad.SetNewXSpeedMultiplier(_slider.value);
        else
            ManualCursorMouseAndGamepad.SetNewYSpeedMultiplier(_slider.value);
        
        _fnDisabled = false;
    }

    void Start()
    {
        Debug.Log("Start!");
        
        _Speed =  PlayerPrefs.GetFloat(_channelID, _defaultValue);
        SetSliderValue(_Speed);
    }
    
    

}
