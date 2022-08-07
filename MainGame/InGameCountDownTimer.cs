using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using TMPro;
using UnityEngine;

public class InGameCountDownTimer : MonoBehaviour
{
    [SerializeField] TMP_Text _tmpText;
    [SerializeField] TMP_Text _tmpText2;
    Color _originalColor;
    bool _isTimerLow;
    bool _isTimerVeryLow;
    bool _hasTimeReachedZero;
    static public float _timerAmountLeft = 100;
    public float TimeAllowedForTheLevel = 90;
    public float TimerLowThreshold = 14;
    public float TimerVeryLowTheshold = 7;
    public float _startTimerForLevel;
    PlaySoundResult _warningTimerSFX;

    public bool HasTimeReachedZero => _hasTimeReachedZero;
    public static float TimerAmountLeft => _timerAmountLeft;

    static float GetTimer()
    {
        return _timerAmountLeft;
    }

    // Start is called before the first frame update
    void Start()
    {
        _tmpText.SetText("" + TimerAmountLeft);
        _originalColor = _tmpText.color;
        if(_tmpText2!=null)
            _tmpText2.SetText(_tmpText.text);
        _isTimerLow = false;
        _isTimerVeryLow = false;
        _hasTimeReachedZero = false;
        var _player = FindObjectOfType<Player>();
        _player.OnPlayerReset += ResetTimer;
        _player.OnPlayerLevelChange += ResetTimer;
        _startTimerForLevel = TimeAllowedForTheLevel;
    }

    void OnDisable()
    {
        //Kill the noise.
        if(_warningTimerSFX!=null)
            if(_warningTimerSFX.ActingVariation!=null)
                _warningTimerSFX.ActingVariation.Stop();

        var _player = FindObjectOfType<Player>();
        if (_player)
        {
            _player.OnPlayerReset -= ResetTimer;
            _player.OnPlayerLevelChange -= ResetTimer;
        }
    }

    void ResetTimer()
    {
        _timerAmountLeft = TimeAllowedForTheLevel;
        _isTimerLow = false;
        _isTimerVeryLow = false;
        _tmpText.color = _originalColor;
        _tmpText.ForceMeshUpdate();
        _hasTimeReachedZero = false;
    }

    public static void AddToTimer(int amountToAdd)
    {
        _timerAmountLeft += amountToAdd;
    }

    void UpdateTimer()
    {
        _timerAmountLeft -= Time.deltaTime;
        
        int closestAmount = (int) TimerAmountLeft;
        _tmpText.SetText("" + closestAmount);
        if(_tmpText2!=null)
            _tmpText2.SetText(_tmpText.text);

        if (_timerAmountLeft > TimerLowThreshold)
        {
            _tmpText2.color = Color.cyan;
        }

        if (_timerAmountLeft <= 0.0f)
        {
            if (_hasTimeReachedZero == false)
            {
                var player = FindObjectOfType<Player>();
                player.TimeKillThePlayer();
            }

            _hasTimeReachedZero = true;
            return;
        }

        if (_isTimerLow)
        {
            
            if (_timerAmountLeft > TimerLowThreshold)
            {
                _tmpText2.color = Color.blue;
                _isTimerLow = false;
                _isTimerVeryLow = false;
            }
            
            if (TimerAmountLeft < TimerVeryLowTheshold)
            {
                if (_isTimerVeryLow)
                    return;
                MasterAudio.PlaySound("TimeWarning");
                _isTimerVeryLow = true;
            }

            return;
        }

        if (TimerAmountLeft < TimerLowThreshold)
        {
            _warningTimerSFX = MasterAudio.PlaySound("TimeWarningLooped");
            _tmpText2.color = Color.red;
            _isTimerLow = true;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }
}
