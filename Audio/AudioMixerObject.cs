using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerObject : MonoBehaviour
{
    [SerializeField] string _channelID = "Master";
    [SerializeField] Toggle _isSoundActive;
    [SerializeField] Slider _slider; 
    [SerializeField] AudioMixer _mixer;
    float _oldSliderValue = 0.8f;
    float _defaultVolume = 0.8f;
    float _volumeLevel = 0.8f;
    static bool _fnDisabled = false;
    static bool _fnToggleDisabled = false;

    void OnDisable() => PlayerPrefs.SetFloat(_channelID,_slider.value);
    void Awake() => SetSliderVolume(PlayerPrefs.GetFloat(_channelID, _defaultVolume));

    public void SetSliderVolume(float sliderVolumeLevel)
    {
        //Disable to stop infinite calls...
        //Set slider position, Set volume, Set Tick
        if (_fnDisabled) return;
        _fnDisabled = true;
        
        //Should not happen but if file corruption 
        if (sliderVolumeLevel < 0) sliderVolumeLevel = 0;
        _slider.value = sliderVolumeLevel;
        
        _mixer.SetFloat(_channelID,ValueToLogarithmicValue(sliderVolumeLevel) );
        if (sliderVolumeLevel > 0)
            HandleToggleValueChange(true);
        
        _fnDisabled = false;
    }

    void Start()
    {
        Debug.Log("Start!");
        if(_isSoundActive!=null)
            _isSoundActive.onValueChanged.AddListener(HandleToggleValueChange);
        
        _volumeLevel =  PlayerPrefs.GetFloat(_channelID, _defaultVolume);
        SetSliderVolume(_volumeLevel);
    }

    void HandleToggleValueChange(bool _newToogleValue)
    {
        if (_fnToggleDisabled) return;
        _fnToggleDisabled = true;
        
        if (_newToogleValue==false)
        {
            //We have sound -> no sound
            _oldSliderValue = _slider.value;
            SetSliderVolume(0.0f);
            _isSoundActive.isOn = false;
        }
        else
        {
            //No sound -> sound
            SetSliderVolume(_oldSliderValue);
            _isSoundActive.isOn = true;
        }
        
        _fnToggleDisabled = false;
    }
    
    float ValueToLogarithmicValue(float inputValue)
    {
        float _scaledvolume;
        if (inputValue > 0.001)
            _scaledvolume = Mathf.Log(inputValue) * 20.0f;
        else
            _scaledvolume = -80.0f; //Minimum allowed range is -80db to +20db
        return _scaledvolume;
    }


}
