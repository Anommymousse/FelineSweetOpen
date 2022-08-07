using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScrollyTextArray : MonoBehaviour
{    
    [SerializeField] float _clipY = 275.0f;
    [SerializeField] float _spawnY = -576.0f;
    [SerializeField] float _YTextSpacing;
    [SerializeField] float _YScrollSpeed = 0.5f;
    CreditsTextData _creditsTextData;

    bool creditsActive = false;

    int _currentTextEntryProcessed;  //default is 0
    int _howManyLinesInFile;
    bool _EndOfEntries;
    float _Xoffset = 0.0f;
    float StartY = 0.0f;

    // Start is called before the first frame update
    void OnEnable()
    {
        //Grab the file text from json.
            _creditsTextData = gameObject.AddComponent<CreditsTextData>();
            _creditsTextData.LoadCredits();
            //Get length of the file in lines.
            _howManyLinesInFile = _creditsTextData.GetLineCount();

        //Initally set them up...
        //_EndOfEntries = false; //Boolean indicating end of grabbable stuffs.

        //Grab the TMpro panel texts so we can fill them in with the first entries.
        var _TMPtextArray = GetComponentsInChildren<TMP_Text>();
        int counter = 0;
        
        string currentTextEntry = " ";
        foreach (TMP_Text _tmpText in _TMPtextArray)
        {
            bool grabnew = (_tmpText.transform.childCount > 0);
            if (grabnew)
                currentTextEntry = GetNextEntry();

            //Calculate the y value
            int newYmult = Mathf.FloorToInt(counter / 2.0f);
            float newYPos = StartY - (newYmult * _YTextSpacing);
            Vector3 position = _tmpText.GetComponent<RectTransform>().position;
            _Xoffset = position.x;
            position.y = newYPos;
            _tmpText.rectTransform.SetPositionAndRotation(position, Quaternion.identity);
            //Set the text from the string array 
            TMP_Text _Text = _tmpText.GetComponent<TMP_Text>();
            _Text.SetText(currentTextEntry);
            counter++;
        }
    }

    void Update()
    {
        if (creditsActive)
        {
            var _childObjects = GetComponentsInChildren<ID_CreditTextParent>();
            foreach (ID_CreditTextParent _childObject in _childObjects)
            {
                var _RT = _childObject.GetComponent<RectTransform>();
                ScrollObjectUpwards(_RT);
                WrapObjectAtTop(_RT);
            }
        }
    }

    string GetNextEntry()
    {
        _currentTextEntryProcessed++;
        return (_creditsTextData.GetStringAtIndex(_currentTextEntryProcessed));
    }

    void ScrollObjectUpwards(RectTransform _object)
    {
        Vector3 _position = _object.position; 
        _position.y += _YScrollSpeed * Time.deltaTime;
        _object.SetPositionAndRotation(_position, Quaternion.identity);
    }

    void WrapObjectAtTop(RectTransform _object)
    {
        if (_currentTextEntryProcessed >= _howManyLinesInFile)
        {
            StartCoroutine(DeActivateCredits());
            return;
        }

        Vector3 _position = _object.localPosition; 
        if (_position.y > _clipY)
        {
            var thingy = StartCoroutine(FadeOutAndGetNewText(_position,_object));
        }
    }

    IEnumerator FadeOutAndGetNewText(Vector3 _position,RectTransform _object)
    {
        //Transform to world axis, use saved x offset as it's panel based
        _position.y = _spawnY;
        _position = _object.TransformVector(_position);
        _position.x = _Xoffset;
        _object.SetPositionAndRotation(_position, Quaternion.identity);

        //Set the new text for BOTH objects
        TMP_Text[] _ObjectsToUpdate = _object.GetComponentsInChildren<TMP_Text>();
        SetNewTextForChildren(_ObjectsToUpdate);
        yield return null;
    }

    IEnumerator DeActivateCredits()
    {
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
        this.enabled = false;
    }

    public void ToggleCredits()
    {
        creditsActive = !creditsActive;
        if (creditsActive == false)
        {
            ResetToOriginalRectPosistions();
           gameObject.SetActive(false);
           _currentTextEntryProcessed = 0;
        }
    }

    void ResetToOriginalRectPosistions()
    {
        var rectobject = GameObject.Find("CreditTextWithShadowA").GetComponent<RectTransform>();
        var pos = rectobject.transform.position;
        pos.y = 261f;
        rectobject.SetPositionAndRotation(pos, Quaternion.identity);
         rectobject = GameObject.Find("CreditTextWithShadowB").GetComponent<RectTransform>();
         pos = rectobject.transform.position;
        pos.y = 148f;
        rectobject.SetPositionAndRotation(pos, Quaternion.identity);
         rectobject = GameObject.Find("CreditTextWithShadowC").GetComponent<RectTransform>();
         pos = rectobject.transform.position;
        pos.y = 22f;
        rectobject.SetPositionAndRotation(pos, Quaternion.identity);
         rectobject = GameObject.Find("CreditTextWithShadowD").GetComponent<RectTransform>();
         pos = rectobject.transform.position;
        pos.y = -87f;
        rectobject.SetPositionAndRotation(pos, Quaternion.identity);
         rectobject = GameObject.Find("CreditTextWithShadowE").GetComponent<RectTransform>();
         pos = rectobject.transform.position;
        pos.y = -199f;
        rectobject.SetPositionAndRotation(pos, Quaternion.identity);
         rectobject = GameObject.Find("CreditTextWithShadowF").GetComponent<RectTransform>();
         pos = rectobject.transform.position;
        pos.y = -307f;
        rectobject.SetPositionAndRotation(pos, Quaternion.identity);
         rectobject = GameObject.Find("CreditTextWithShadowF (1)").GetComponent<RectTransform>();
         pos = rectobject.transform.position;
        pos.y = -424f;
        rectobject.SetPositionAndRotation(pos, Quaternion.identity);
         rectobject = GameObject.Find("CreditTextWithShadowF (2)").GetComponent<RectTransform>();
         pos = rectobject.transform.position;
        pos.y = -524f;
        rectobject.SetPositionAndRotation(pos, Quaternion.identity);
         rectobject = GameObject.Find("CreditTextWithShadowF (3)").GetComponent<RectTransform>();
         pos = rectobject.transform.position;
        pos.y = -656f;
        rectobject.SetPositionAndRotation(pos, Quaternion.identity);
        
    }

    void SetNewTextForChildren(TMP_Text[] objectsToUpdate)
    {
        string newText = GetNextEntry();
        foreach (var text in objectsToUpdate)
        {
            text.SetText(newText);
        }
    }
}