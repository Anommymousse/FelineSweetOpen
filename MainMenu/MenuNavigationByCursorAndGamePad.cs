using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class MenuNavigationByCursorAndGamePad : MonoBehaviour
{
    public List<GameObject> menuItemsToNavigate;
    int _menuCount;
    int _currentOption;
    Controls _controls;

    enum MenuMode{
        MainMode,
        AudioMode,
        KeyboardMode,
        GamepadMode,
        VideoMode,
        EditorMode,
        CustomMode,
        BackMode
    }

    enum MenuEntryType{
        BasicButton,
        VolumeSliderWithTick,
        VolumeSliderNoTick,
        Tick,
        DropDown,
        GamepadButtonWithReset,
        KeyboardButtonWithReset
    }

    struct BasicButton
    {
        public string _submenuEntry;
    }
    
    struct SubMenuItem
    {
        public int _subMenuIndex;
        public BasicButton _basicButton;
        public MenuEntryType _menuEntryType;
        public float _volume;
        public bool _tick;
        public Dropdown _dropdown;
    }

    List<List<SubMenuItem>> _menuItems;

    List<SubMenuItem> _audioControls;
    List<SubMenuItem> _keyboardControls;
    List<SubMenuItem> _videoControls;
    List<SubMenuItem> _gamepadControls;
    List<SubMenuItem> _editorControls;
    List<SubMenuItem> _miscControls;

    int _menuSubIndex = 0;
    int _menuSubIndexMax;
    MenuMode _menuMode;

    Vector3 screenPointCenter;
    Vector3 localPointCenter;
    Vector3 normalPointCenter;

    void CreateAllMenus()
    {
        _menuItems = new List<List<SubMenuItem>>();
        _audioControls = CreateAudioControls();
        _keyboardControls = CreateControlsKeyboard();
        _videoControls = CreateVideoControls();
        _gamepadControls = CreateControlsGamepad();
        _editorControls = CreateEditorControls();
        //_miscControls = CreateMiscControls();
        
        _menuItems.Add(_audioControls);
        _menuItems.Add(_keyboardControls);
        _menuItems.Add(_gamepadControls);
        _menuItems.Add(_videoControls);
        _menuItems.Add(_editorControls);
        //_menuItems.Add(_miscControls);
        
    }

    List<SubMenuItem> CreateAudioControls()
    {
        _audioControls = new List<SubMenuItem>();
        
        SubMenuItem temp = new SubMenuItem();
        
        temp._basicButton._submenuEntry = "Master Volume";
        temp._menuEntryType = MenuEntryType.VolumeSliderWithTick;
        temp._subMenuIndex = 0;
        temp._volume = 70.0f;
        temp._tick = true;
        _audioControls.Add(temp);
        
        temp._basicButton._submenuEntry = "Music Volume";
        temp._subMenuIndex = 1;
        temp._volume = 70.0f;
        temp._tick = true;
        _audioControls.Add(temp);
        
        temp._basicButton._submenuEntry = "SFX Volume";
        temp._subMenuIndex = 2;
        _audioControls.Add(temp);
        
        return _audioControls;
    }

    List<SubMenuItem> CreateControlsKeyboard()
    {
        _keyboardControls = new List<SubMenuItem>();
        
        SubMenuItem temp = new SubMenuItem();
        temp._basicButton._submenuEntry = "Left";
        temp._subMenuIndex = 0;
        temp._menuEntryType = MenuEntryType.BasicButton;
        _keyboardControls.Add(temp);
        temp._basicButton._submenuEntry = "Right";
        temp._subMenuIndex = 1;
        _keyboardControls.Add(temp);
        temp._basicButton._submenuEntry = "Build";
        temp._subMenuIndex = 2;
        _keyboardControls.Add(temp);
        temp._basicButton._submenuEntry = "Build Down";
        temp._subMenuIndex = 3;
        _keyboardControls.Add(temp);
        temp._basicButton._submenuEntry = "Jump";
        temp._subMenuIndex = 4;
        _keyboardControls.Add(temp);
        temp._basicButton._submenuEntry = "Dash";
        temp._subMenuIndex = 5;
        _keyboardControls.Add(temp);
        temp._basicButton._submenuEntry = "Spell";
        temp._subMenuIndex = 6;
        _keyboardControls.Add(temp);

        return _keyboardControls;
    }

    List<SubMenuItem> CreateControlsGamepad()
    {
        _gamepadControls = new List<SubMenuItem>();
        
        SubMenuItem temp = new SubMenuItem();
        temp._basicButton._submenuEntry = "Left";
        temp._subMenuIndex = 0;
        temp._menuEntryType = MenuEntryType.BasicButton;
        _gamepadControls.Add(temp);
        temp._basicButton._submenuEntry = "Right";
        temp._subMenuIndex = 1;
        _gamepadControls.Add(temp);
        temp._basicButton._submenuEntry = "Build";
        temp._subMenuIndex = 2;
        _gamepadControls.Add(temp);
        temp._basicButton._submenuEntry = "Build Down";
        temp._subMenuIndex = 3;
        _gamepadControls.Add(temp);
        temp._basicButton._submenuEntry = "Jump";
        temp._subMenuIndex = 4;
        _gamepadControls.Add(temp);
        temp._basicButton._submenuEntry = "Dash";
        temp._subMenuIndex = 5;
        _gamepadControls.Add(temp);
        temp._basicButton._submenuEntry = "Spell";
        temp._subMenuIndex = 6;
        _gamepadControls.Add(temp);

        return _gamepadControls;
    }
    
    List<SubMenuItem> CreateVideoControls()
    {
        _videoControls = new List<SubMenuItem>();
        
        SubMenuItem temp = new SubMenuItem();
        temp._basicButton._submenuEntry = "Video";
        temp._menuEntryType = MenuEntryType.DropDown;
        _videoControls.Add(temp);
        
        temp._basicButton._submenuEntry = "Quality";
        temp._menuEntryType = MenuEntryType.DropDown;
        _videoControls.Add(temp);
        
        temp._basicButton._submenuEntry = "Fullscreen";
        temp._menuEntryType = MenuEntryType.Tick;
        _videoControls.Add(temp);

        return _videoControls;
    }
    
    
    List<SubMenuItem> CreateEditorControls()
    {
        _editorControls = new List<SubMenuItem>();
        
        SubMenuItem temp = new SubMenuItem();
        temp._basicButton._submenuEntry = "Start Editor";
        temp._subMenuIndex = 0;
        temp._menuEntryType = MenuEntryType.BasicButton;
        _editorControls.Add(temp);

        return _editorControls;
    }

    List<SubMenuItem> CreateMiscControls()
    {
        _miscControls = new List<SubMenuItem>();
        
        SubMenuItem temp = new SubMenuItem();
        temp._basicButton._submenuEntry = "Default";
        temp._subMenuIndex = 0;
        temp._menuEntryType = MenuEntryType.BasicButton;
        _miscControls.Add(temp);

        temp._basicButton._submenuEntry = "30 FPS";
        temp._subMenuIndex = 1;
        temp._menuEntryType = MenuEntryType.BasicButton;
        _miscControls.Add(temp);

        temp._basicButton._submenuEntry = "60 FPS";
        temp._subMenuIndex = 1;
        temp._menuEntryType = MenuEntryType.BasicButton;
        _miscControls.Add(temp);
        
        return _miscControls;
    }

    void MenuStartUp()
    {
        _menuCount = menuItemsToNavigate.Count;
        _currentOption = 0;
        SetActiveMenuPosition(_currentOption);
        Cursor.visible = false;
        _menuMode = MenuMode.MainMode;

        _menuItems = new List<List<SubMenuItem>>();
        CreateAllMenus();

    }
    
    void SetActiveMenuPosition(int currentOption)
    {
        Vector2 warpPosition = Screen.safeArea.center;
        screenPointCenter= Camera.main.WorldToScreenPoint(menuItemsToNavigate[currentOption].transform.position);
        Mouse.current.WarpCursorPosition(screenPointCenter);
        InputState.Change(Mouse.current.position, screenPointCenter);

    }

    void Start()
    {
        MenuStartUp();
        SetupPlayerControls();
        //StartCoroutine(SwapPositionInMenuTest());
    }

    // Update is called once per frame
    void Update()
    {
        Mouse.current.WarpCursorPosition(screenPointCenter);
        InputState.Change(Mouse.current.position, screenPointCenter);
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
    }

    void OnPlayerSelects(InputAction.CallbackContext obj)
    {
        MenuMode currentMainMenuMode = (MenuMode)_currentOption;
        Debug.Log("On player selects!");

        /*return;        

        Debug.Log($" MenuMode = {_currentOption}");
        
        if (currentMainMenuMode == MenuMode.AudioMode)
        {
            _menuMode = MenuMode.AudioMode;
            _menuSubIndex = 0;
            _menuSubIndexMax = 2;
            return;
        }
        if (currentMainMenuMode == MenuMode.KeyboardMode)
        {
            _menuMode = MenuMode.KeyboardMode;
            return;
        }

        if (currentMainMenuMode == MenuMode.GamepadMode)
        {
            _menuMode = MenuMode.GamepadMode;
            return;
        }
        
        if (currentMainMenuMode == MenuMode.VideoMode)
        {
            _menuMode = MenuMode.VideoMode;
            return;
        }
        if (currentMainMenuMode == MenuMode.EditorMode)
        {
            _menuMode = MenuMode.EditorMode;
            return;
        }
        
        if (currentMainMenuMode == MenuMode.BackMode)
        {
            _menuMode = MenuMode.BackMode;
            return;
        }
        
        if (_menuMode == MenuMode.MainMode) return;*/
        
    }

    void OnPlayerMovesRight(InputAction.CallbackContext obj)
    {
    }

    
    void OnPlayerMovesDown(InputAction.CallbackContext obj)
    {
        Debug.Log($" Move Down MenuMode = {_currentOption} = {_menuMode} menu count = {menuItemsToNavigate.Count}");
        switch (_menuMode)
        {
            case MenuMode.MainMode:
                    if (_currentOption < menuItemsToNavigate.Count - 1)
                        _currentOption++;
                    SetActiveMenuPosition(_currentOption);
                 break;
            case MenuMode.AudioMode:
                if (_menuSubIndex < _menuSubIndexMax - 1)
                    _menuSubIndex++;
                //SetActiveSubMenuPosition(_menuSubIndex);
                break;
            case MenuMode.KeyboardMode:
                break;
            case MenuMode.GamepadMode:
                break;
            case MenuMode.VideoMode:
                break;
            case MenuMode.CustomMode:
                break;
            case MenuMode.BackMode:
                break;
            default:
                break;
        }
        if (_menuMode == MenuMode.MainMode)
        {
        }
    }
    
    void OnPlayerMovesLeft(InputAction.CallbackContext obj)
    {
    }

    void OnPlayerMovesUp(InputAction.CallbackContext obj)
    {
        if (_currentOption > 0)
            _currentOption--;
        SetActiveMenuPosition(_currentOption);
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
