using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnMouseCursor : MonoBehaviour
{
    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
