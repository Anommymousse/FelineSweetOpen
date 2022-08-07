using System.Collections;
using System.Collections.Generic;
using Ricimi;
using UnityEngine;
using UnityEngine.InputSystem;

public class DemoAddEscQuit : MonoBehaviour
{
    public SceneTransition _sceneTransition;
    // Start is called before the first frame update
    void Start()
    {
            
    }

    bool QuitFunction()
    {
        if(Gamepad.current!=null)
            if (Gamepad.current.selectButton.wasPressedThisFrame) return true; 
        if (Keyboard.current.escapeKey.wasPressedThisFrame) return true;
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;

        if (QuitFunction())
        {
            _sceneTransition.PerformTransition();
        }
    }
}
