using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    VideoPlayer _videoplayer;
    bool _pauseModeEnabled;
    bool _startup;
    void OnEnable()
    {
        _videoplayer = GetComponent<VideoPlayer>();
        _pauseModeEnabled = false;
        _startup = true;
        StartCoroutine(WaitASec());
    }

    IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(1.0f);
        _startup = false;
    }


    void QuitPlayingVideo()
    {
        _videoplayer = GetComponent<VideoPlayer>();
        _videoplayer.Stop();
        gameObject.SetActive(false);
    }
    
    

    void Update()
    {
        if (_startup) return;
        
        //Three modes...
        //1) Keyboard and Pad Active
        //2) Keyboard Only
        //3) Pad only
        //4) none...
        
        

        //No more esc if not plugged in...
        //if (Gamepad.current is null) return;
        //if (Keyboard.current is null) return;

        if ((Gamepad.current is null)&&(Keyboard.current is null)) return;

/*        if (Gamepad.current is null)
        {
            //Keyboard only
            if(Keyboard.current.escapeKey.wasPressedThisFrame)
                QuitPlayingVideo();
            return;
        }

        if (Keyboard.current is null)
        {
            //Gamepad only
            if(Gamepad.current.selectButton.wasPressedThisFrame)
            {
                QuitPlayingVideo();
            }
            return;
        }*/
        
        //Both
/*        if(Gamepad.current.selectButton.wasPressedThisFrame||Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            QuitPlayingVideo();
        }*/

        if (WasEscapePressedThisFrame())
        {
            QuitPlayingVideo();
        }

        //if (Gamepad.current.startButton.wasPressedThisFrame||Keyboard.current.spaceKey.wasPressedThisFrame) 
        if(WasPausePressedThisFrame())
        {
            _videoplayer = GetComponent<VideoPlayer>();

            if (_videoplayer.isPlaying)
            {
                _pauseModeEnabled = true;
                _videoplayer.Pause();
            }
            else
            {
                _pauseModeEnabled = false;
                _videoplayer.Play();
            }
        }

        if ((!_videoplayer.isPlaying)&&(_pauseModeEnabled is false))
        {
            QuitPlayingVideo();    
        }

        

    }

    bool WasEscapePressedThisFrame()
    {
        bool wasPressed = false;

        if (Gamepad.current is not null)
        {
            if (Gamepad.current.selectButton.wasPressedThisFrame)
                wasPressed = true;
        }

        if (Keyboard.current is null) return wasPressed;
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            wasPressed = true;
        }

        return wasPressed;
    }

    bool WasPausePressedThisFrame()
    {
        bool pausedWasPressed = false;

        if (Gamepad.current is not null)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
                pausedWasPressed = true;
        }

        if (Keyboard.current is null) return pausedWasPressed;
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            pausedWasPressed = true;
        }

        return pausedWasPressed;
    }
}
