using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.Mouse;

public class ChangeMouseCursor : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    Camera _camera;
    public Vector3 mousePos3d;
    public Vector3 canvasLocalScale;
    public Vector3 mousePos3dLocalSpace;
    public static ChangeMouseCursor MouseCursorInstance { get; private set; } 
    /*void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }*/
    // Start is called before the first frame update
    /*void OnEnable()
    {
        return;
        _camera = Camera.main;
        canvasLocalScale = rectTransform.localScale;
        Cursor.visible = false;

        if (MouseCursorInstance == null)
        {
            MouseCursorInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }*/

    void Update()
    {
        return;
/*            mousePos3d = Mouse.current.position.ReadValue(); //ScreenSpace
            
            //Scale to reference
            mousePos3d.x = mousePos3d.x * (1920.0f / Screen.width); 
            mousePos3d.y = mousePos3d.y * (1080.0f / Screen.height);
            //To sprite local space -102ish to +102ish on X
            mousePos3dLocalSpace.x = (mousePos3d.x- 1920.0f/2) * canvasLocalScale.x ;
            mousePos3dLocalSpace.y = (mousePos3d.y- 1080.0f/2) * canvasLocalScale.y ;
            mousePos3dLocalSpace.z = 0;
            gameObject.transform.localPosition = mousePos3dLocalSpace;*/
    }

    void OnDisable()
    {
        
    }
}
