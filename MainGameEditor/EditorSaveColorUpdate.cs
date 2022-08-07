using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorSaveColorUpdate : MonoBehaviour
{
    Button _buttonRef;
    Image _imageRef;
    EditorScanTileMapsForTilename _saveFlagObjectReference;
    public SpriteRenderer _spriteRendererRef;
    
    // Start is called before the first frame update
    void Awake()
    {
        _buttonRef = GetComponent<Button>();
        _imageRef = GetComponent<Image>();
        _saveFlagObjectReference = GameObject.Find("Checks").GetComponent<EditorScanTileMapsForTilename>();
        _saveFlagObjectReference.UpdateSaveStatus += UpdateSaveStatus;
    }
    
    bool IsTestModeActive()
    {
        //Mmmmm tasty spaget
        var buttonObj1 = GameObject.Find("TestButtonText");
        var text1 = buttonObj1.GetComponent<TMP_Text>();
        if (text1.text.Contains("Stop")) return true;
        return false;
    }

    
    void UpdateSaveStatus()
    {
        if ((_saveFlagObjectReference.IsSaveEnabled)||(IsTestModeActive()))
        {
            _buttonRef.interactable = true;
            _spriteRendererRef.color = Color.white;
            _imageRef.color = Color.green;
        }
        else
        {
            _buttonRef.interactable = false;
            _spriteRendererRef.color = Color.grey;
            _imageRef.color = Color.grey;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
