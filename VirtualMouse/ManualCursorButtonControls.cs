using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ManualCursorButtonControls : MonoBehaviour
{
    Button _buttonSelfRef;
    RectTransform _buttonRectTransform;
    public ManualCursorMouseAndGamepad cursorControlRef;
    public bool scaledCat=false;
    public float pitchchange=0.0f;
    float _minx; 
    float _miny;
    float _maxx;
    float _maxy;
    float _widthButton;
    float _heightButton;

    bool needRectTransStill;
    bool startup = false;
    bool doneTheCoordinates;

    bool _isButtonHighlighted;

    //bool _insideStateLastFrame = false;
    bool _hasMenuBox = false;
    SetupMenuBox _setupMenuBox;

    Vector2 initialScreenPosition;

    // Start is called before the first frame update
    void Start()
    {
        _buttonSelfRef = GetComponent<Button>();
        _buttonRectTransform = GetComponent<RectTransform>();
        CreateGetButtonWidthHeight();
        needRectTransStill = true;

        _setupMenuBox = GetComponent<SetupMenuBox>();
        if (_setupMenuBox != null)
            _hasMenuBox = true;

        initialScreenPosition = Vector2.zero;

        if (cursorControlRef==null)
        {
            //Attempt fix
            var cursorObj = GameObject.Find("Cursor");
            if (cursorObj != null)
                cursorControlRef = cursorObj.GetComponent<ManualCursorMouseAndGamepad>();
            else
            {
                Debug.LogError("Cursor not in scene");
            }
        }
    
    
    //GetButtonAreaInScreenCoordinates();//Note -: FUCKING UNITY MUST WAIT A FRAME BEFORE THE LAYOUT IS FIXED! Couldn't even do it in lateupdate...
    }

    void CreateGetButtonWidthHeight()
    {
        var rect = _buttonRectTransform.rect;
        _widthButton = rect.width;
        _heightButton= rect.height;
    }

    //Must wait a frame to call this cause unity is bloody daft af
    void GetButtonAreaInScreenCoordinates()
    {
        Canvas[] c = GetComponentsInParent<Canvas>();
        Canvas topmost = c[c.Length-1];

        float scaledWidth = _widthButton* Screen.width/ 1920.0f;
        float scaledHeight = _heightButton* Screen.height/ 1080.0f;
        
        if (topmost.renderMode is RenderMode.WorldSpace or RenderMode.ScreenSpaceCamera)
        {
            var narray = new Vector3[4];
            
            //World points- note-: direct rect version doesn't work as it's scaled by viewport.
            _buttonRectTransform.GetWorldCorners(narray);
            
            //Log($"narray world = {narray[0]} x {narray[1]} x {narray[2]} x {narray[3]} ");

            //world -> Screen
            for (int i = 0; i < 4; i++)
            {
                narray[i] = Camera.main.WorldToScreenPoint(narray[i]);
            }
            //Log($"narray screen raw= {narray[0]} x {narray[1]} x {narray[2]} x {narray[3]} ");
            
            //Sort screen points to mins and maxs
            _minx = narray[0].x;
            _maxx = narray[0].x;
            _miny = narray[0].y;
            _maxy = narray[0].y;
            for (int i = 1; i < 4; i++)
            {
                if (narray[i].x < _minx) _minx = narray[i].x;
                if (narray[i].y < _miny) _miny = narray[i].y;
                if (narray[i].x > _maxx) _maxx = narray[i].x;
                if (narray[i].y > _maxy) _maxy = narray[i].y;
            }
            
        }
        else
        {
            Vector3 positionBase;
            if (_buttonSelfRef == null)
            {
                positionBase = gameObject.transform.position;
            }
            else
                positionBase = _buttonSelfRef.transform.position;

            if (gameObject.name.Contains("Play Button"))
            {
                positionBase = new Vector3(1000, 210, 0);
                positionBase.x = positionBase.x * Screen.width/ 1920.0f;
                positionBase.y = positionBase.y * Screen.height / 1080.0f;
            }

            if (gameObject.name.Contains("CloseButton"))
            {
                positionBase = new Vector3(1580, 867, 0);
                positionBase.x = positionBase.x * Screen.width/ 1920.0f;
                positionBase.y = positionBase.y * Screen.height / 1080.0f;
            }

            //Log($" positionBase {positionBase} og width {_widthButton} {_heightButton} scaled {scaledWidth} {scaledHeight} for {gameObject.name}");
            
            //Scale position base...
            
            
            //Log($"Non-worldspace or screenspace");
            _minx = positionBase.x - scaledWidth / 2;
            _maxx = positionBase.x + scaledWidth / 2;
            _miny = positionBase.y - scaledHeight / 2;
            _maxy = positionBase.y + scaledHeight / 2;
            initialScreenPosition = transform.position;
        }
        
        //Log($"({_minx},{_miny})->({_maxx},{_maxy}) Based on {scaledWidth}x{scaledHeight} for {gameObject.name}");
    }

    Vector2 CurrentScreenCoordinate()
    {
        Canvas[] c = GetComponentsInParent<Canvas>();
        Canvas topmost = c[c.Length-1];

        Vector2 currentScreenPos;

        if (topmost.renderMode is RenderMode.WorldSpace or RenderMode.ScreenSpaceCamera)
        {
            currentScreenPos = Camera.main.WorldToScreenPoint(_buttonSelfRef.transform.position);
        }
        else
        {
            currentScreenPos = transform.position;
        }
        
        return currentScreenPos;
    }

    bool DetectIfInButtonArea(float x,float y)
    {
        if ((x > _minx) && (x < _maxx) && (y > _miny) && (y < _maxy))
        {
            return true;
        }
        return false;
    }

    bool IsTestModeActive()
    {
        //Mmmmm tasty spaget
        var buttonObj1 = GameObject.Find("TestButtonText");
        if (buttonObj1 is null) return false;
        var text1 = buttonObj1.GetComponent<TMP_Text>();
        if (text1.text.Contains("Stop")) return true;
        return false;
    }



    
    // Update is called once per frame
    void Update()
    {
        if (needRectTransStill)
        {
            if (startup)
            {
                GetButtonAreaInScreenCoordinates();
                needRectTransStill = false;
            }

            if (startup == false) startup = true;
            if(needRectTransStill) return;
        }
        
        //Only go on if we have a valid rect transform from the layout group(End Of Frame-as unity can't code either) 
        if (cursorControlRef == null)
        {
            Debug.LogError($"cursorRef == null ");
            return;
        }
        var cursorPosition = cursorControlRef.GetManualCursorCoords();

        
        if (DetectIfInButtonArea(cursorPosition.x, cursorPosition.y))
        {
            _isButtonHighlighted = true;
            
            //Log($"{gameObject.name} highlighted");

            //Can't press in editor mode.
            if (IsTestModeActive())
            {
                if (_buttonSelfRef.name.Contains("Test") is false)
                {
                    //Log($"button[{_buttonSelfRef.name}] not activated as test mode");
                    return;
                }

            }

            //Check if button pressed while highlighted.
            if (cursorControlRef.WasCursorActivatedThisFrame())
            {
                var getAnimatedbutt = GetComponent<AnimatedButton>();
                if (getAnimatedbutt != null)
                {
                    //Log($"Animated button[{getAnimatedbutt.name}] activated");
                    getAnimatedbutt.onClick.Invoke();
                }
                else
                {
                    
                    if (_buttonSelfRef != null)
                    {
                        //Log($"button[{_buttonSelfRef.name}] activated");
                        if(_buttonSelfRef.interactable) //Make sure we can click it.
                            _buttonSelfRef.onClick.Invoke();
                    }
                }
            }
        }
        else
        {
            //Not Hightlight mode
            _isButtonHighlighted = false;
        }

        HandleButtonHighlightThings();

    }
    
    void SetUpWiggle(SetupMenuBox setupMenuBox)
    {
        string displayedText = _setupMenuBox._menuText;
        if (!displayedText.StartsWith("<"))
        {
            //MasterAudio.PlaySoundAndForget("UI_Click_Metallic_mono");
            MasterAudio.PlaySoundAndForget("Meows #1",default, 1.0f - (pitchchange / 100.0f));
            _setupMenuBox._menuText = "<wiggle a=.5>" + displayedText + "</wiggle>";
            _setupMenuBox.UpdateText();
        }
    }
    
    string StripAnimationOut(string inputString)
    {
        if (!inputString.StartsWith("<")) return inputString;

        int partone = inputString.IndexOf(">");
        int parttwo = inputString.LastIndexOf("<");
        int length = parttwo - partone-1;
        int firstText = partone + 1;
        return inputString.Substring(firstText, length);
    }

    void RemoveWiggle(SetupMenuBox setupMenuBox)
    {
        var oldText = _setupMenuBox._menuText;
        _setupMenuBox._menuText = StripAnimationOut(_setupMenuBox._menuText);
        
        if(oldText!= _setupMenuBox._menuText)
            _setupMenuBox.UpdateText();
    }

    void HandleButtonHighlightThings()
    {
        if (_isButtonHighlighted)
        {
            if (_hasMenuBox)
                SetUpWiggle(_setupMenuBox);
        }
        else
        {
            if (_hasMenuBox)
                RemoveWiggle(_setupMenuBox);
        }
    }
}
