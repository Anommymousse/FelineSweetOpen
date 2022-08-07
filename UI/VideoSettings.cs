using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropDown;
    Resolution[] _resolutions;
    // Start is called before the first frame update
    void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        
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
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}