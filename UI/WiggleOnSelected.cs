using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WiggleOnHighlight : MonoBehaviour
{
    bool isInside = false;
    bool hasMenuBox = false;
    SetupMenuBox _setupMenuBox;
    public bool IsInside() => isInside;
    Button _button;
    // Start is called before the first frame update
    void Start()
    {
        _setupMenuBox = gameObject.GetComponent<SetupMenuBox>();
        hasMenuBox = false;
        if (_setupMenuBox != null)
        {
            hasMenuBox = true;
        }

        _button = gameObject.GetComponent<Button>();        
    }

    // Update is called once per frame
    void Update()
    {
        
        
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
}
