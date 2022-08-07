using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorStopCommandDontSaveAgain : MonoBehaviour
{
    public TMP_Text _refText;
    public EditorSaveButtonPress _refEditorSaveButtonPress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PressOnlyIfButtonReadsTest()
    {
        var buttonString = _refText.text;
        if (buttonString.ToLower().Contains("test"))
        {
            Debug.Log($"Saving game, not running yet");
            _refEditorSaveButtonPress.ButtonPressed();
        }
        else
        {
            Debug.Log($"No save game already running");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
