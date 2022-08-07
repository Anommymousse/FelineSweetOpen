using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IPointerMouseHandler : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    bool isInside = false;
    bool hasMenuBox = false;
    SetupMenuBox _setupMenuBox;
    public bool IsInside() => isInside;

    Button _button;
    //[SerializeField] AudioClip _audioHoverSource;

    void Start()
    {
        StartCoroutine(DelayASecond());
        _setupMenuBox = gameObject.GetComponent<SetupMenuBox>();
        hasMenuBox = false;
        if (_setupMenuBox != null)
        {
            hasMenuBox = true;
        }

        _button = gameObject.GetComponent<Button>();

    }

    IEnumerator DelayASecond()
    {
        yield return new WaitForSecondsRealtime(0.1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Play hover sound");
        isInside = true;
        
        if (hasMenuBox)
        {
            string displayedText = _setupMenuBox._menuText;
            if (!displayedText.StartsWith("<"))
            {
                MasterAudio.PlaySoundAndForget("UI_Click_Metallic_mono");
                _setupMenuBox._menuText = "<wiggle a=.5>" + displayedText + "</wiggle>";
                _setupMenuBox.UpdateText();
            }
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

    public void OnPointerExit(PointerEventData eventData)
    {
        isInside = false;
        if (hasMenuBox)
        {
            _setupMenuBox._menuText = StripAnimationOut(_setupMenuBox._menuText);
            //_setupMenuBox._menuText = _initialMenuBoxText;
            _setupMenuBox.UpdateText();
        }
        
        
        //Crash on exit from main game using gamepad
        if(UnityEngine.EventSystems.EventSystem.current)
            if( UnityEngine.EventSystems.EventSystem.current.alreadySelecting == true )
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
        Debug.Log("Exit button");
    }

    void OnDisable()
    {
        isInside = false;
        if (hasMenuBox)
        {
            _setupMenuBox._menuText = StripAnimationOut(_setupMenuBox._menuText);
            _setupMenuBox.UpdateText();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Play hover sound");
        isInside = true;

        /*if(_audioHoverSource != null)
           GetComponent<AudioSource>()?.PlayOneShot(_audioHoverSource);*/
        
        MasterAudio.PlaySoundAndForget("UI_Click_Metallic_mono");

        if (hasMenuBox)
        {
            string displayedText = _setupMenuBox._menuText;
            if (!displayedText.StartsWith("<"))
            {
                _setupMenuBox._menuText = "<wiggle a=.5>" + displayedText + "</wiggle>";
                _setupMenuBox.UpdateText();
            }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isInside = false;
        if (hasMenuBox)
        {
            _setupMenuBox._menuText = StripAnimationOut(_setupMenuBox._menuText);
            _setupMenuBox.UpdateText();
        }

        /*if( UnityEngine.EventSystems.EventSystem.current.alreadySelecting == true )
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
        Debug.Log("Exit button");*/
    }
}
