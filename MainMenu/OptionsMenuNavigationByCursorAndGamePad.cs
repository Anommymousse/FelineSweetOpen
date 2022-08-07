using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AlteredRebindSystem;
using DG.Tweening;
using Ricimi;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public class OptionsMenuNavigationByCursorAndGamePad : MonoBehaviour
{
    public List<GameObject> menuItemsToNavigate;
    List<GameObject> subMenuGameObjects;
    public GameObject rebindUIobject;
    int _menuCount;
    int _currentOption;
    static int _selectsPerformed = 0;
    Controls _controls;

    Vector3 screenPointCenter;
    Vector3 localPointCenter;
    Vector3 normalPointCenter;

    //0- audio, 1- keyboard, 2-gamepad, 3-cursor spd, 4-video, 5-edtior, 6-custom, 7 back....
    
    public GameObject menuItem01;
    public GameObject menuItem02;
    public GameObject menuItem03;
    public GameObject menuItem04;
    public GameObject menuItem05;
    public GameObject menuItem06;
    public GameObject menuItem07;

    GameObject menu01;
    GameObject menu02;
    GameObject menu03;
    GameObject menu04;
    GameObject menu05;
    GameObject menu06;
//    GameObject menu07;
    
    bool firstpass;
    
    //State - MainMenu = false, submenu = true
    bool subMenuActive;

    void Log(string message)
    {
        StackFrame callStack = new StackFrame(1, true);
        var methodName = callStack.GetMethod();
        var linenumber = callStack.GetFileLineNumber();
        
        Debug.Log($"{message} from line {linenumber} function {methodName}");
    }

    
    GameObject PrepareSubmenuDisplay(GameObject menuItemIn)
    {
        Log($"-->Prepping submenu {menuItemIn.name}");
        
        GameObject menuOut;
        var mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        var rootcanvas = mainCanvas.rootCanvas;
        
        menuOut = Instantiate(menuItemIn, rootcanvas.transform, true);
        //Instantiate menus - have to be scaled as linking to canvas mults by 9+
        menuOut.transform.localPosition = Vector3.zero;
        menuOut.transform.DOScaleX(1.0f, 0.05f);
        menuOut.transform.DOScaleY(1.0f, 0.05f);

        var containsRebindUI = menuOut.GetComponentsInChildren<RebindActionUI>();
        if (containsRebindUI == null)
        {
            Log($"<-- Null Prepping submenu");
            return menuOut;
        }

        /*foreach (var subMenus in containsRebindUI)
        {
            Log($"Prepping submenu {subMenus.name}");
            subMenus.rebindOverlay = rebindUIobject;
            subMenus.rebindPrompt = subMenus.m_RebindText;
        }*/
        
        
        Log($"<--Prepping submenu");
        return menuOut;
    }
    void MenuStartUp()
    {

        Log("-->MenuStartUp Enter");
        
        menu01 = PrepareSubmenuDisplay(menuItem01);
        menu02 = PrepareSubmenuDisplay(menuItem02);
        menu03 = PrepareSubmenuDisplay(menuItem03);
        menu04 = PrepareSubmenuDisplay(menuItem04);
        menu05 = PrepareSubmenuDisplay(menuItem05);
        menu06 = PrepareSubmenuDisplay(menuItem06);
        //menu07 = PrepareSubmenuDisplay(menuItem07);
        
        subMenuGameObjects = new List<GameObject>();
        subMenuGameObjects.Add(menu01);
        subMenuGameObjects.Add(menu02);
        subMenuGameObjects.Add(menu03);
        subMenuGameObjects.Add(menu04);
        subMenuGameObjects.Add(menu05);
        subMenuGameObjects.Add(menu06);
        //subMenuGameObjects.Add(menu07);

        _menuCount = menuItemsToNavigate.Count;
        _currentOption = 0;
        SetActiveMenuPosition(_currentOption);
        Cursor.visible = false;
        
        firstpass = true;
        subMenuActive = false;
        Log("<--MenuStartUp Exit");
    }

    void SetActiveMenuPosition(int currentOption)
    {
        Log($"--> SetActiveMenuPosition enter setting to {currentOption}");
        Vector2 warpPosition = Screen.safeArea.center;
        screenPointCenter= Camera.main.WorldToScreenPoint(menuItemsToNavigate[currentOption].transform.position);
        Mouse.current.WarpCursorPosition(screenPointCenter);
        InputState.Change(Mouse.current.position, screenPointCenter);
        Log("<-- SetActiveMenuPosition exit");
    }

    void Start()
    {
        Log("--> Start");
        MenuStartUp();
        SetupPlayerControls();
        Log("<-- Start");
        //StartCoroutine(SwapPositionInMenuTest());
    }

    // Update is called once per frame
    void Update()
    {
        if (subMenuActive == false)
        {
            Mouse.current.WarpCursorPosition(screenPointCenter);
            InputState.Change(Mouse.current.position, screenPointCenter);
        }
    }

    
    void SetupPlayerControls()
    {
        //Controls Setup.
        _controls = new Controls();
        _controls.Frontend.Enable();
        
        _controls.Frontend.Left.performed += OnPlayerMovesLeft;
        _controls.Frontend.Up.performed += OnPlayerMovesUp;
        _controls.Frontend.Down.performed +=OnPlayerMovesDown;
        _controls.Frontend.Right.performed +=OnPlayerMovesRight;
        _controls.Frontend.Select.performed += OnPlayerSelects;
        _controls.Frontend.Esc.performed += OnPlayerEscapeOrBack;
    }

    void OnPlayerEscapeOrBack(InputAction.CallbackContext obj)
    {
        if (subMenuActive)
        {
            subMenuGameObjects[_currentOption].SetActive(false);
            subMenuActive = false;
            firstpass = true;
            Cursor.visible = false;
        }
        else
        {
            //Assumes last entry = back
            var scenetrans = menuItemsToNavigate[menuItemsToNavigate.Count - 1].gameObject
                .GetComponent<SceneTransition>();
            if(scenetrans!=null)
                scenetrans.PerformTransition();
        }
    }


    void SetCorrectSubMenuActive(int menuSelected)
    {
        Log($"Setting Active menu =<color=red> {menuSelected} firstpass={firstpass}</color>");
        
        if(!firstpass)
            if (menuSelected == _currentOption) return;
        
        if(subMenuGameObjects!=null)
            foreach (var menuItem in subMenuGameObjects)
            {
                menuItem.SetActive(false);
            }
            
        if(subMenuGameObjects!=null)
            subMenuGameObjects[menuSelected].SetActive(true);
        
        firstpass = false;
        subMenuActive = true; //?

    }
    void OnPlayerSelects(InputAction.CallbackContext obj)
    {
        Log($" Selected submenuact={subMenuActive} option={_currentOption}");
        _selectsPerformed++;
        Log($" OMNBCG Selects performed = {_selectsPerformed}");

        if (subMenuActive == false)
        {
            SetCorrectSubMenuActive(_currentOption);
        }
    }

    void OnPlayerMovesRight(InputAction.CallbackContext obj)
    {
    }

    void OnPlayerMovesDown(InputAction.CallbackContext obj)
    {
        Log($"Current position = {_currentOption}");
        if (!subMenuActive)
        {
            if (_currentOption < (menuItemsToNavigate.Count - 2))
                _currentOption++;
            SetActiveMenuPosition(_currentOption);
        }
    }
    
    void OnPlayerMovesLeft(InputAction.CallbackContext obj)
    {
    }

    void OnPlayerMovesUp(InputAction.CallbackContext obj)
    {
        if (!subMenuActive)
        {
            if (_currentOption > 0)
                _currentOption--;
            SetActiveMenuPosition(_currentOption);
        }
    }

    
    void OnDisable()
    {
        _controls.Frontend.Left.performed -= OnPlayerMovesLeft;
        _controls.Frontend.Up.performed -= OnPlayerMovesUp;
        _controls.Frontend.Down.performed -=OnPlayerMovesDown;
        _controls.Frontend.Right.performed -=OnPlayerMovesRight;
        _controls.Frontend.Select.performed -= OnPlayerSelects;

        _controls.Frontend.Disable();
    }
    

    IEnumerator SwapPositionInMenuTest()
    {
        int countToBreak = 0;
        yield return null;
        while (true)
        {
            yield return new WaitForSeconds(4.0f);

            _currentOption++;
            if (_currentOption > menuItemsToNavigate.Count - 1)
                _currentOption = 0;
            
            SetActiveMenuPosition(_currentOption);
            
            countToBreak++;
            if (countToBreak > 20) break;
        }
    }
}
