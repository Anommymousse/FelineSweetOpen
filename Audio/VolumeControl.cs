using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string _channelVolume = "Master";
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _slider;
    [SerializeField] Toggle _toggle;
    float _defaultVolume = 0.8f;
    void Awake()
    {
        _slider.onValueChanged.AddListener(HandleSliderValueChanged);
        _toggle.onValueChanged.AddListener(HandleToggleValueChanged);
    }


    void Start() => _slider.value = PlayerPrefs.GetFloat(_channelVolume, _slider.value);
    void OnDisable() => PlayerPrefs.SetFloat(_channelVolume, _slider.value);
    void HandleSliderValueChanged(float sliderValue) => _mixer.SetFloat(_channelVolume, ValueToLogarithmicValue(sliderValue));
    void HandleToggleValueChanged(bool soundEnabled)
    {
        if (soundEnabled)
            _slider.value = _defaultVolume;
        else
            _slider.value = _slider.minValue;
    }

    float ValueToLogarithmicValue(float inputValue)
    {
        float _scaledvolume;
        if (inputValue > 0.001)
            _scaledvolume = Mathf.Log(inputValue)*20f;
        else
            _scaledvolume = -80.0f; //Minimum allowed range is -80db to +20db
        return _scaledvolume;
    }
    
}
