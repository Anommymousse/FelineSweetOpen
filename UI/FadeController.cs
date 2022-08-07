using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FadeController : MonoBehaviour
{
    FadeSpriteList _fadeSpriteList; 
    //bool _isModeFadeOut;
    //bool _fadeInProgress;
    float _scaleMin = 1f;
    float _scaleMax = 2000f;
    RectTransform _rectTransformRef;
    SpriteRenderer _spriteRendererRef;
    static FadeController _instance;

    static bool setUpComplete = false;
    static bool _maingameStallFade = false;

    public void SetMainGameStallOn()
    {
        _maingameStallFade = true;
    }
    
    public static void SetMainGameStallOff()
    {
        _maingameStallFade = false;
    }

    
    void Awake()
    {
        var go = GameObject.Find("LOGOBOOTUP");
        
        if (go == null)
        {
            go = GameObject.Find("[BOOTSTRAP]");
            _fadeSpriteList = go.GetComponent<FadeSpriteList>();
        }
        else
        {
            _fadeSpriteList = go.GetComponent<FadeSpriteList>();
        }

        _rectTransformRef = GetComponent<RectTransform>();
        _rectTransformRef.DOScale(Vector3.one, 0.01f);
        //_fadeInProgress = false;
        //_isModeFadeOut = true;
        _spriteRendererRef = GetComponent<SpriteRenderer>();
        _instance = this;
        
        _rectTransformRef.DOScale(Vector3.one*_scaleMax, 0.01f);
        setUpComplete = true;
        FadeIn();
    }

    public static void FadeOutAsync()
    {
        _instance.StartCoroutine(_instance.DoFadeOutAsync());
    }

    IEnumerator DoFadeOutAsync()
    {
        //_fadeInProgress = true;
        
        _spriteRendererRef.color = Color.white;
        RandomlyAssignFaderImage();
        Vector3 scale=Vector3.one * _scaleMax;
        _rectTransformRef.DOScale(scale, 1.5f);
        yield return new WaitForSeconds(1.5f);
        //_isModeFadeOut = false;
        
        //_fadeInProgress = false;
    }

    public static void FadeOut(string sceneToLoad)
    {
        if (_instance == null)
            Debug.Log($" null instance? {setUpComplete}");
        else
            _instance.StartCoroutine(_instance.DoFadeOut(sceneToLoad));
    }
    
    public void FadeIn()
    {
        if (_instance != null)
        {
            var go = GameObject.Find("LevelManager");
            if (go != null) 
                _maingameStallFade = true;
            else
                _maingameStallFade = false;
            
            
            _instance.StartCoroutine(_instance.DoFadeIn());
        }
        else
        {
            Debug.Log($" null instance? {setUpComplete}");
        }
    }

    
    void RandomlyAssignFaderImage()
    {
        _spriteRendererRef.sprite = _fadeSpriteList.GetRandomFadeSprite();
    }
    
    IEnumerator DoFadeIn()
    {
        //_fadeInProgress = true;

        for (int w = 0; w < 10; w++)
        {
            if (_maingameStallFade)
            {
                Debug.Log($"<color=green> Main game stall turn{w} </color>");
                yield return new WaitForSeconds(0.5f);
            }
        }

        _spriteRendererRef.color = Color.white;
        RandomlyAssignFaderImage();
        Vector3 scale=Vector3.one* _scaleMin;
        _rectTransformRef.DOScale(scale, 1.0f);
        yield return new WaitForSeconds(1.0f);
        //_isModeFadeOut = true;
        _spriteRendererRef.color = Color.clear;
        
        //_fadeInProgress = false;
    }

    IEnumerator DoFadeOut(string sceneToLoad)
    {
        //_fadeInProgress = true;
        
        _spriteRendererRef.color = Color.white;
        RandomlyAssignFaderImage();
        Vector3 scale=Vector3.one * _scaleMax;
        _rectTransformRef.DOScale(scale, 1.5f);
        yield return new WaitForSeconds(1.5f);
        //_isModeFadeOut = false;
        
        //_fadeInProgress = false;
        
        Debug.Log($"Scene to load = {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }
}
