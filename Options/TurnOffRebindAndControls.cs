using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffRebindAndControls : MonoBehaviour
{
    [SerializeField] GameObject _rebindOverlay;
    [SerializeField] GameObject _gamepad;
    [SerializeField] GameObject _keyboard;

    void Awake()
    {
        _rebindOverlay.SetActive(false);
        if(_gamepad!=null)
            _gamepad.SetActive(false);
        if(_keyboard!=null)
            _keyboard.SetActive(false);
    }
}
