using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffMouseCursor : MonoBehaviour
{
    static bool mouseVisibleState;

    public static  bool IsMouseVisible() => mouseVisibleState; 
    
    //int count = 0;
    void OnEnable()
    {
        /*Cursor.visible = false;*/
        mouseVisibleState = false;
    }

    void OnDisable()
    {
        //Cursor.visible = true;
        mouseVisibleState = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseVisibleState == false)
        {
            /*if (Cursor.visible)
            {
                Debug.Log($"Unity messing up {count}");   
                Cursor.visible = false;
                count++;
                return;
            }*/
        }

        if (mouseVisibleState)
        {
            if (Cursor.visible == false)
            {
                Cursor.visible = true;
            }
        }


    }
}
