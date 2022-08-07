using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoOptionsHandler : MonoBehaviour
{
    public GameObject ResolutionGameObject;
    public GameObject ToggleGameObjectTick;
    public GameObject ToggleGameObjectCross;
    

    int _currentResolutionSelected;
    Resolution[] _resolutions;
    bool _fullScreenActive;
    int _quality;
    
    public enum QualityLevel
    {
        Low,
        Medium,
        High,
    };
    
    //Resolution
    public int GetCurrentResolutionIndex() => _currentResolutionSelected;

    public void IncrementCurrentResolution()
    {
        if (_currentResolutionSelected < _resolutions.Length-1)
            _currentResolutionSelected++;
        SetScreenResolution(_currentResolutionSelected);
    }
    public void DecrementCurrentResolution()
    {
        if (_currentResolutionSelected > 1 )
            _currentResolutionSelected--;
        SetScreenResolution(_currentResolutionSelected);
    }
    
    //Quality
    public void IncrementQualityLevel()
    {
        if(_quality<2)
            _quality++;
    }
    public void DecrementQualityLevel()
    {
        if(_quality>0)
            _quality--;
    }

    public int GetQuality()
    {
        return _quality;
    }
    
    //Fullscreen
    public bool IsFullScreen()
    {
        return _fullScreenActive;
    }

    public void ToggleFullScreen()
    {
        _fullScreenActive = !_fullScreenActive;

        ToggleGameObjectCross.SetActive(false);
        ToggleGameObjectTick.SetActive(false);
        if (_fullScreenActive)
            ToggleGameObjectTick.SetActive(true);
        else
            ToggleGameObjectCross.SetActive(true);
    }

    public void SetIsFullScreenToggle(bool newValue)
    {
        _fullScreenActive = newValue;
    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        _resolutions = Screen.resolutions;
        _currentResolutionSelected = 0;
        
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
        }

        _currentResolutionSelected = currentResolutionIndex;
        //_currentResolutionSelected = _resolutions.Length - 1;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreeen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetScreenResolution(int screenResIndex)
    {
        Resolution resolution = _resolutions[screenResIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
        _currentResolutionSelected = screenResIndex;
        ResolutionGameObject.GetComponent<SetupMenuBox>()._menuText = $"{resolution.width}x{resolution.height}";
        ResolutionGameObject.GetComponent<SetupMenuBox>().UpdateText();
    }

    public Resolution GetCurrentResolution()
    {
        return _resolutions[_currentResolutionSelected];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}