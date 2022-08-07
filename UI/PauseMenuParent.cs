using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuParent : MonoBehaviour
{
    static bool _isPaused;
    Controls _controls;
    float _escape;

    public static bool isPaused() => _isPaused;
    // Start is called before the first frame update
    void Start()
    {
        _isPaused = false;
    }

    void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
        }
        _controls.Gameplay.Esc.performed += context => _escape = context.ReadValue<float>();
        _controls.Gameplay.Esc.canceled += context => _escape = 0.0f;
        
        _controls.Gameplay.Enable();

    }

    public static void PauseTheGame()
    {
        _isPaused = true;
        Time.timeScale = 0.0f;
    }
    
    void UnPauseTheGame()
    {
        _isPaused = false;
        var canvasforMenus = GetComponentInChildren<Canvas>();
        canvasforMenus.enabled = false;
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (_escape > 0)
        {
            if (_isPaused == false)
            {
                PauseTheGame();
            }
            else
            {
                UnPauseTheGame();
            }
        }



    }
}


