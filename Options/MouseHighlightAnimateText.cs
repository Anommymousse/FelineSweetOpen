using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHighlightAnimateText : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    TMP_Text[] _tmpTexts;
    string _originalString;
    Button _subButton; 
    void Start()
    {
        _tmpTexts = GetComponentsInChildren<TMP_Text>();
        _originalString = _tmpTexts[0].text;
        var subButtonList = GetComponentsInChildren<Button>();
        foreach (var buttonid in subButtonList)
        {
            if (buttonid.name.Contains("Rebind"))
            {
                _subButton = buttonid;
            }
        }
    }

    void OnDisable()
    {
        if (_originalString is not null)
        {
            _tmpTexts[0].SetText(_originalString);
            _tmpTexts[1].SetText(_originalString);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var textString = "<bounce a=.8>" + _tmpTexts[0].text + "</bounce>";
        _tmpTexts[0].SetText(textString);
        _tmpTexts[1].SetText(textString);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tmpTexts[0].SetText(_originalString);
        _tmpTexts[1].SetText(_originalString);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        _subButton.onClick.Invoke();
    }
}
