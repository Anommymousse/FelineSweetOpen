using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageTicks : MonoBehaviour
{
    bool AsUnityHasProblemsWithToggles;
    
    [SerializeField] Toggle _vsyncOffTogggle;
    [SerializeField] Toggle _vsync1Togggle;
    [SerializeField] Toggle _vsync2Togggle;
    [SerializeField] TMP_Text _tmpTextRef;
    [SerializeField] TMP_Text _tmpTextRef2;

    void Start()
    {
        ToggleOffEverything();
    }

    void SetVsyncAndTargetFrameRate(int frameRate,int vsyncNumber)
    {
        Application.targetFrameRate = frameRate;
        QualitySettings.vSyncCount = vsyncNumber;
        
    }
    
    public void ToggleOffEverything()
    {
        Debug.Log("Toggle off everything");
        /*_vsyncOffTogggle.SetIsOnWithoutNotify(false);
        _vsync1Togggle.SetIsOnWithoutNotify(false);
        _vsync2Togggle.SetIsOnWithoutNotify(false);
        _targetframerate30fpsToggle.SetIsOnWithoutNotify(false);
        _targetframerate60fpsToggle.SetIsOnWithoutNotify(false);
        _targetframerateDefaultToggle.SetIsOnWithoutNotify(false);*/
        /*foreach (var toggle in toggleList)
        {
            toggle.isOn = false;
        }*/
    }
    
    
    public void Set60FPS()
    {
        _tmpTextRef.SetText("60 FPS");
        _tmpTextRef2.SetText("60 FPS");
        SetTargetFrameRate(60);
    }

    public void TurnOnDefaultFPS()
    {
        _tmpTextRef.SetText("Default");
        _tmpTextRef2.SetText("Default");
        SetTargetFrameRate(-1);
    }

    public void TurnOn30FPS()
    {
        _tmpTextRef.SetText("30 FPS");
        _tmpTextRef2.SetText("30 FPS");
        SetTargetFrameRate(30);
    }

    void ToggleFPS(Toggle toggleID, int fps)
    {
        Debug.Log($"Toggle {toggleID.name} {fps}");
        if (toggleID.isOn == false)
        {
            ToggleOffEverything();
            
            Debug.Log($"Toggle set on {toggleID.name}");
            toggleID.SetIsOnWithoutNotify(true);
            _vsyncOffTogggle.SetIsOnWithoutNotify(true);
            SetVsyncAndTargetFrameRate(fps, 0);
        }
        else
        {
            toggleID.SetIsOnWithoutNotify(false);
        }
    }
    
    public void ToggleVsyncOffOn()
    {
        Debug.Log("Toggle Vsoff");
        if (_vsyncOffTogggle.isOn)
            _vsyncOffTogggle.SetIsOnWithoutNotify(false);
        else
            _vsyncOffTogggle.SetIsOnWithoutNotify(true);
    }
    
    public void ToggleVsync1On()
    {
        Debug.Log("Toggle Vs1");
        ToggleOffEverything();
        _vsync1Togggle.SetIsOnWithoutNotify(true);
        SetVsyncAndTargetFrameRate(-1, 1);
    }

    public void ToggleVsync2On()
    {
        Debug.Log("Toggle Vs2");
        ToggleOffEverything();
        _vsync2Togggle.SetIsOnWithoutNotify(true);
        SetVsyncAndTargetFrameRate(-1, 2);
    }

    
    public void SetTargetFrameRate(int frameRate)
    {
        Application.targetFrameRate = frameRate;
    }

    public void VsyncSet(int number)
    {
        QualitySettings.vSyncCount = number;
    }
    
}
