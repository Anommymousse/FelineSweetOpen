using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AssignCatSpriteAndAnimateUpdated : MonoBehaviour
{
    //Assumes 1 sprite renderer
    [SerializeField] Sprite buttonsIdleSprite;
    [SerializeField] Sprite buttonsActiveSprite;
    [SerializeField] bool FlipSpriteState;
    [SerializeField] ManualCursorMouseAndGamepad _refManualCursorMouseAndGamepad;
    BubbleDetector _bubbleDetectorRef; 
    
    SpriteRenderer _spriteRenderer;
    Vector3[] _worldSpaceCorners;
    Vector4 _minmaxButtonScreenSpace;
    // Start is called before the first frame update
    void Start()
    {
        _bubbleDetectorRef = GetComponentInParent<BubbleDetector>();
        
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = buttonsIdleSprite;

        var buttontemp = GetComponent<RectTransform>();

        _worldSpaceCorners = new Vector3[4];
        buttontemp.GetWorldCorners(_worldSpaceCorners);

        var temp = GetMinMaxOf(_worldSpaceCorners);
        
        //Debug.Log($"Name {gameObject.name} min({temp.x},{temp.y}) --> max({temp.z},{temp.w}) worldspace");

        List<Vector3> screenCoords= new List<Vector3>();
        foreach (var value in _worldSpaceCorners)
        {
            screenCoords.Add(Camera.main.WorldToScreenPoint(value));
        }

        //Checked ok
        _minmaxButtonScreenSpace = GetMinMaxOf(screenCoords);
        
        //Debug.Log($"Name {gameObject.name} min({minmax.x},{minmax.y}) --> max({minmax.z},{minmax.w}) screenspace");
        
    }


    Vector4 GetMinMaxOf(List<Vector3> arrayOfPoints)
    {
        Vector4 rv=Vector4.zero;
        if (arrayOfPoints.Count <= 0) return rv;
        
        //min
        rv.x = arrayOfPoints[0].x;
        rv.y = arrayOfPoints[0].y;
        //max
        rv.z = arrayOfPoints[0].x;
        rv.w = arrayOfPoints[0].y;
        for (int i = 1; i < arrayOfPoints.Count; i++)
        {
            if (rv.x > arrayOfPoints[i].x) rv.x = arrayOfPoints[i].x;
            if (rv.y > arrayOfPoints[i].y) rv.y = arrayOfPoints[i].y;
            
            if (rv.z < arrayOfPoints[i].x) rv.z = arrayOfPoints[i].x;
            if (rv.w < arrayOfPoints[i].y) rv.w = arrayOfPoints[i].y;
        }

        return rv;
    }

    Vector4 GetMinMaxOf(Vector3[] arrayOfPoints)
    {
        Vector4 rv=Vector4.zero;
        if (arrayOfPoints.Length <= 0) return rv;
        
        //min
        rv.x = arrayOfPoints[0].x;
        rv.y = arrayOfPoints[0].y;
        //max
        rv.z = arrayOfPoints[0].x;
        rv.w = arrayOfPoints[0].y;
        for (int i = 1; i < arrayOfPoints.Length; i++)
        {
            if (rv.x > arrayOfPoints[i].x) rv.x = arrayOfPoints[i].x;
            if (rv.y > arrayOfPoints[i].y) rv.y = arrayOfPoints[i].y;
            
            if (rv.z < arrayOfPoints[i].x) rv.z = arrayOfPoints[i].x;
            if (rv.w < arrayOfPoints[i].y) rv.w = arrayOfPoints[i].y;
        }

        return rv;
    }

    bool IsInsideButton(Vector2 pointToTest)
    {
        if ((pointToTest.x > _minmaxButtonScreenSpace.x) && (pointToTest.x < _minmaxButtonScreenSpace.z))
            if ((pointToTest.y > _minmaxButtonScreenSpace.y) && (pointToTest.y < _minmaxButtonScreenSpace.w))
                return true;
        
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        var cursorCoords= _refManualCursorMouseAndGamepad.GetManualCursorCoords();

        if (IsInsideButton(cursorCoords))
        {
            _spriteRenderer.sprite = buttonsActiveSprite;
            if(_bubbleDetectorRef)
                _bubbleDetectorRef.SetInside();
        }
        else
        {
            _spriteRenderer.sprite = buttonsIdleSprite;
        }
    }

    void OnValidate()
    {
        if (buttonsIdleSprite != null)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _spriteRenderer.sprite = buttonsIdleSprite;
            _spriteRenderer.flipX = FlipSpriteState;
        }
    }
}
