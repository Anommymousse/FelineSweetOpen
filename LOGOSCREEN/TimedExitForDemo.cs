using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimedExitForDemo : MonoBehaviour
{
    float _displayLogoTimer = 3.0f;
    AsyncOperation _checkforMainMenuLoaded;
    Scene _sceneRef;
    void OnEnable()
    {
        Debug.Log($"Entered demo  enable");
        //Need to start loading in main menu
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(WaitForLevelToBeReady());
    }

    IEnumerator WaitForLevelToBeReady()
    {
        yield return new WaitForSeconds(_displayLogoTimer);
        //_checkforMainMenuLoaded = SceneManager.LoadSceneAsync("Scenes/Logo",LoadSceneMode.Single);
        _checkforMainMenuLoaded = SceneManager.LoadSceneAsync("Scenes/MainMenu",LoadSceneMode.Single);
        yield return null;
        while(!_checkforMainMenuLoaded.isDone)
        {
            yield return null;
        }
    }

    bool _isRebindingActive = false;
    bool _waskeyredfined = false;

    float _maxWaitTimer = 5.0f;
    IEnumerator RebindingExample()
    {
        if(_isRebindingActive) yield break;
        _isRebindingActive = true;

        float waitUntilThisTime = _maxWaitTimer + Time.time;
        
        //Do Function stuff
        while (Time.time < waitUntilThisTime)
        {
            //Do something that reads/processes the keys.
            
            //If the rebind function has done the business
            if(_waskeyredfined)
            {
                _isRebindingActive = false;
                yield break; //Quit the loop
            }
            
            yield return null; //waits a frame
        }

        yield return new WaitForSeconds(0.1f);
        _isRebindingActive = false; //Technically not needed
    }

    void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        FadeController.FadeOutAsync();
        _sceneRef = arg0;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
