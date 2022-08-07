using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PausePanelManager : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    //bool _pauseMenuActive;
    [SerializeField] GameObject _rebindScreen;
    [SerializeField] GameObject _leftKeyboard;
    [SerializeField] GameObject _rightKeyboard;
    static int counterp = 0;

    void Awake()
    {
        
        _panel.SetActive(false);
        //_panel.SetActive(true);
        //_pauseMenuActive = false; 
        _rebindScreen.SetActive(false); 
        _rightKeyboard.SetActive(false);
        _leftKeyboard.SetActive(false);
        counterp = 0;
        Cursor.visible = true;
    }
    
    void OnDisable()
    {
        DeActivatePauseMenu();
    }

    public void DeActivatePauseMenu()
    {
        counterp++;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        
        Debug.Log($"Deactivate pause {counterp} visible off");
        
        //_pauseMenuActive = false;
        _panel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ActivatePauseMenu()
    {
        counterp++;
        
        _panel.SetActive(true);
        Debug.Log($"activate pause {counterp} visible On");
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PauseMenuToggle()
    {
        if(Time.timeScale==0)
            DeActivatePauseMenu();
        else
            ActivatePauseMenu();
    }
}
