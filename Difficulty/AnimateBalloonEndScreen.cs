using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DG.Tweening;
//using TreeEditor;
using UnityEngine;

public class AnimateBalloonEndScreen : MonoBehaviour
{
    //int _clickCount = 0;
    //bool _soundAlreadyPlaying = false;
    //bool balloonInBurstMode;
    
    float _localRotZ;
    //float _localRotSpd = 0.05f;
    float _localRotZmax=0.3f;
    float _localRotZdir = 1.0f;
    float _localRotTimeToComplete = 6.0f;
    float _heighty;
    float _heightSpd = 0.07f;
    float _heightymax = 4.0f;
    float _heightDirection = -1.0f;
    float _localheightTimeToComplete = 6.0f;
    float _wibblexSpeed=0.3f;
    float _wibblex;
    float _wibblexMax = 3.0f;
    float _wibbleDirection = -1.0f;
    float _localwibbleTimeToComplete = 3.5f;

    //float ogScale = 0.7f;
    //float scaleMin = 0.65f;
    //float scaleMax = 0.75f;
    //float scaleSpeed = 0.05f;
    //float currentScaleAdj = 0.0f;
    //float timedChange = 3.0f;
    //float scaledirection = 1.0f; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Reset()
    {
        _localRotZ = 0;
        _heighty = 0;
        _wibblex = 0;
      //  _clickCount = 0;
      //  balloonInBurstMode = false;
        //_soundAlreadyPlaying = false;

    }

    void Awake()
    {
        Reset();
    }

    void UpdateBalloonDirections()
    {
        _localRotZ += _localRotZdir*_localRotZmax * Time.deltaTime/_localRotTimeToComplete;
        _heighty += _heightDirection* _heightymax * Time.deltaTime / _localheightTimeToComplete;
        _wibblex += _wibbleDirection*_wibblexMax* Time.deltaTime/_localwibbleTimeToComplete;

        if (Mathf.Abs(_localRotZ) > _localRotZmax)
        {
            _localRotZ = _localRotZmax*_localRotZdir;
            _localRotZdir *= -1.0f;
        }
        
        if (Mathf.Abs(_heighty) > _heightymax)
        {
            _heighty = _heightymax*_heightDirection;
            _heightDirection *= -1.0f;
        }

        if (Mathf.Abs(_wibblex) > _wibblexMax)
        {
            _wibblex = _wibblexMax * _wibbleDirection;
            _wibbleDirection *= -1.0f;
        }

        var localpos = transform.localPosition;
        localpos.x += _wibblexSpeed * _wibbleDirection;
        localpos.y += _heightSpd * _heightDirection;
        transform.localPosition = localpos;

        /*var transformthing = transform.localScale;
        transformthing.x =ogScale + currentScaleAdj;*/
        
        //var localRot = transform.localRotation;
        //localRot.z +=  _localRotSpd * _localRotZdir;
        //transform.localRotation = localRot;

    }

    public void PlayCatSound()
    {
        Debug.Log($"Play cat soundfx");
        MasterAudio.PlaySound("Cat_Purr_01");
    }

    IEnumerator SoundResetTimer()
    {
        yield return new WaitForSeconds(1.0f);
        //_soundAlreadyPlaying = false;
    }

    void Update()
    {
        UpdateBalloonDirections();
    }
}
