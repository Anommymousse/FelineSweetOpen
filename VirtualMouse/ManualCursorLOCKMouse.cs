using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualCursorLOCKMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("<color=red> start lock </color>");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    void OnDisable()
    {
        Debug.Log("<color=red> OnDisable lock called </color>");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
