using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;


public class MoveMouseCursorFromGamePad : MonoBehaviour
{
    Mouse _systemMouse;
    //static float _mouseVertSpeed = 3.8f;
    //static float _mouseHorzSpeed = 3.8f;
    const float _defaultTo1atzeropoint5 = 2.0f;
    Controls _controls;

    static bool triggeredThisFrame = false;
    static bool handling_ButtonPress = false;
    GameObject objectWeHit;

    //static float mouseXmult = 1.0f;
    //static float mouseYmult = 1.0f;

    //static float gamePadTimeActive = 0.0f;
    //static bool gamePadChangeDirection;
    //static float gamePadLastXdirection; 
    //static float gamePadLastYdirection;

    Vector2 gamepadinput;

    // Start is called before the first frame update
    void Start()
    {
        _systemMouse = Mouse.current;
      //  mouseXmult = PlayerPrefs.GetFloat("GamepadSpdX", 0.5f);
      //  mouseYmult = PlayerPrefs.GetFloat("GamepadSpdY", 0.5f);
      //  mouseXmult *= _defaultTo1atzeropoint5;
      //  mouseYmult *= _defaultTo1atzeropoint5;
    }

    void OnEnable()
    {
        _controls = new Controls();
        _controls.UI.Enable();
        _controls.UI.Button.performed += ButtonOnperformed;
        //gamePadTimeActive = 0.0f;
        //gamePadChangeDirection = false;
        //gamePadLastXdirection = 1.0f;
        //gamePadLastYdirection = 1.0f;
    }

    void OnDisable()
    {
        _controls.UI.Button.performed -= ButtonOnperformed;
        _controls.UI.Disable();
    }

    float ScaleMovementOfCursorGamepad()
    {
        var scaleGamepadCursorSpeed = 1.0f;
        
        /*if (gamePadTimeActive > 1.0f)
            scaleGamepadCursorSpeed = 2.0f;
        if (gamePadTimeActive > 2.0f)
            scaleGamepadCursorSpeed = 4.0f;
        if (gamePadTimeActive > 4.0f)
            scaleGamepadCursorSpeed = 6.0f;
        if (gamePadTimeActive > 8.0f)
            scaleGamepadCursorSpeed = 8.0f;*/
        return scaleGamepadCursorSpeed;
    }
    
    void MoveMouseViaGamepad()
    {
     /*   var value = _controls.UI.Move.ReadValue<Vector2>();
        
        gamepadinput = value;
        
        if (((value.x > 0) && (gamePadLastXdirection < 0))||((value.x < 0) && (gamePadLastXdirection > 0)))
            gamePadChangeDirection = true;
        if (((value.y > 0) && (gamePadLastYdirection < 0))||((value.y < 0) && (gamePadLastYdirection > 0)))
            gamePadChangeDirection = true;

        if (gamePadChangeDirection)
            gamePadTimeActive = 0.0f;
        else
        {
            gamePadTimeActive += Time.deltaTime;
        }

        var scalemovement = ScaleMovementOfCursorGamepad();
            
        if((Mathf.Abs(value.x) > 0.01f)||(Mathf.Abs(value.y) > 0.01f))
        {
            value.x *= _mouseHorzSpeed * mouseXmult * scalemovement;
            value.y *= _mouseVertSpeed * mouseYmult * scalemovement;
            var cmos = Mouse.current.position.ReadValue();
            cmos += value;
            _systemMouse.WarpCursorPosition(cmos);
            InputState.Change(Mouse.current.position,cmos);
        }

        gamePadChangeDirection = false;*/
        //Debug.Log($" the value {cmos}");
    }

    public static List<RaycastResult> GetObjectsUnderCursor()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results;
    }
    
    IEnumerator OutsideThing()
    {
        Debug.Log($"outside thing called");
        if(handling_ButtonPress) yield return null;
        
        handling_ButtonPress = true;
        
        Debug.Log($"Handle press");
        
        var eventSystem = EventSystem.current;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        //RaycastHit[] hits;
        RaycastHit hit;
        
        Debug.Log($"Ray cast at ({Mouse.current.position.ReadValue()})");
        //var things = Physics.RaycastAll(ray.origin, transform.forward, 1.0F);
        var things = Physics.RaycastAll(ray);

        Debug.Log("Start hitthing");
        var objectsUnderCursor = GetObjectsUnderCursor();
        //Debug.Log($" hit count = {GetObjectsUnderCursor()}");
        foreach (var hitthing in objectsUnderCursor)
        {
            Debug.Log($"we hit {hitthing.gameObject.name} ");
        }
        Debug.Log("end hitthing");
        
        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log($"hit?");
            if (hit.collider != null)
            {
                Debug.Log($"hit collider for gamepad {hit.collider.name}");
                //   ExecuteEvents.Execute(hit.collider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
                //ExecuteEvents.Execute<IPointerClickHandler>(hit.collider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler); 
                ExecuteEvents.Execute(hit.collider.gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
                        
            }
        }
        else
        {
            Debug.Log($"No hit?");
        }
        Debug.Log($"Ray cast end");

        if(hit.collider!=null)
            if(hit.collider.gameObject!=null)
                ExecuteEvents.Execute(hit.collider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        
        handling_ButtonPress = false;
    }

    void ButtonOnperformed(InputAction.CallbackContext obj)
    {
        triggeredThisFrame = true;
    }


    // Update is called once per frame
    void Update()
    {
        MoveMouseViaGamepad();
        if (triggeredThisFrame)
        {
            triggeredThisFrame = false;
            StartCoroutine(OutsideThing());            
        }
    }
}
