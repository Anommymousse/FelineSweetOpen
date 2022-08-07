using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCursorSimpleTickProcessing : MonoBehaviour
{
    bool _IsFullScreen;
    GameObject _tickRef; //assumes it can be off/on
    GameObject _crossRef; 
    // Start is called before the first frame update
    void Start()
    {
        _IsFullScreen = Screen.fullScreen;
        UpdateTickObjects();
    }

    void UpdateTickObjects()
    {
        _tickRef.SetActive(_IsFullScreen);
        _crossRef.SetActive(!_IsFullScreen);
    }

    public void ToggleTick()
    {
        _IsFullScreen = !_IsFullScreen;
        Screen.fullScreen = _IsFullScreen;
        UpdateTickObjects();
    }
    
}
