using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupMenuBox : MonoBehaviour
{
    [SerializeField] int _widthPerChar = 38;
    [SerializeField] Vector3 _Textoffset = new Vector3(-0.1f, 0.1f,0);
    [SerializeField] Vector3 _TextShadowoffset = new Vector3(0.1f, -0.1f,0);
    [SerializeField] public string _menuText=" ";
    [SerializeField] int _textHeight = 100;
    [SerializeField] bool _ignoreValidate = false;

    public void UpdateText()
    {
        OnValidate();
    }

    public void SetWidth(int newwidth)
    {
        _widthPerChar = newwidth;
    }

    int GetLengthWithoutAnimations(string inputString)
    {
        var textGreaterThanPosition = inputString.IndexOf(">");
        var textLessThanPosition = inputString.LastIndexOf("<");
        
        if(textGreaterThanPosition<1)
        return inputString.Length;

        int textLength = textLessThanPosition - textGreaterThanPosition-1;
        return textLength;
    }

    //This should update the text on the button automatically and scale both the children.
    void OnValidate()
    {
        if (_ignoreValidate) return;
        //Setup the width.
        //int stringlength = _menuText.Length;
        int stringlength = GetLengthWithoutAnimations(_menuText);
        if(stringlength<=0)
        {
            _menuText = " "; //Default to 1 space, for safety
            stringlength = 1;
        }

        int totalBoxWidth = stringlength * _widthPerChar;
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta= new Vector2(totalBoxWidth, _textHeight);
        
        //Expect 2 entries one for shadow.
        var _TMPtextArray = GetComponentsInChildren<TMP_Text>();
        
        if(_TMPtextArray != null)
        {            
            if (_TMPtextArray.Length == 2)
            {
                var text_index0 = _TMPtextArray[0].GetComponentsInChildren<TMP_Text>();
                var text_index1 = _TMPtextArray[1].GetComponentsInChildren<TMP_Text>();
                
                if((text_index0==null) && (text_index1==null))
                {
                    Debug.Log("Expected 2 TMPtexts one child of the other");
                }
                else
                {
                    //Null entry placed on top - non-shadow
                    if(text_index0 == null)
                    {
                        _TMPtextArray[0].transform.position = rectTransform.position + _Textoffset;
                        _TMPtextArray[1].transform.position = rectTransform.position + _TextShadowoffset;
                        _TMPtextArray[0].SetText(_menuText);
                        _TMPtextArray[1].SetText(_menuText);
                        
                    }
                    else
                    {
                        _TMPtextArray[1].transform.position = rectTransform.position + _Textoffset;
                        _TMPtextArray[0].transform.position = rectTransform.position + _TextShadowoffset;
                        _TMPtextArray[0].SetText(_menuText);
                        _TMPtextArray[1].SetText(_menuText);
                    }
                }
            }
            else
            {
                Debug.Log($"Expected 2 TMPtexts found {_TMPtextArray.Length}");                
            }
        }

    }
}
