#define DEMO
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class TimedExitAndLoad : MonoBehaviour
{
    //float _displayLogoTimer = 0.5f;
    static bool notready = true;
    AsyncOperation _checkforMainMenuLoaded;
    Scene _sceneRef;
    public GameObject pressAnyKeyObject;
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
        #if DEMO
        Debug.Log($"Entered logo  enable");
        //Need to start loading in main menu
        
        InputSystem.onAnyButtonPress.CallOnce(_ => SomeFunction()); 
            //Debug.Log("Thing") ); 
        #endif
        
        StartCoroutine(WaitForLevelToBeReady());
    }

    IEnumerator WaitForLevelToBeReady()
    {
        yield return null;
        
        #if DEMO
        while (notready)
        {
            yield return new WaitForSeconds(0.01f);
        }
        #else
            yield return new WaitForSeconds(1.0f);
        #endif
        pressAnyKeyObject.SetActive(false);
        
        //_checkforMainMenuLoaded = SceneManager.LoadSceneAsync("Scenes/DemoPreScreen",LoadSceneMode.Single);
        
        _checkforMainMenuLoaded = SceneManager.LoadSceneAsync("Scenes/MainMenu",LoadSceneMode.Single);
        while(!_checkforMainMenuLoaded.isDone)
        {
            yield return null;
        }
    }

    public static void SomeFunction()
    {
        notready = false;
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
