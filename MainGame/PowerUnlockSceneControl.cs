using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUnlockSceneControl : MonoBehaviour
{
    static GameObject _canvas;
    void Start()
    {
        _canvas = this.gameObject;
        _canvas.SetActive(false);
    }

    public static void EnablePowerUnlockScene()
    {
        _canvas.SetActive(true);
    }

    public static void DisablePowerUnlockScene()
    {
        _canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
