using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditorReplaceMouseCursor : MonoBehaviour
{
    //[SerializeField] RectTransform rectTransform;
    //Camera _camera;

    public GameObject _spriteCursorGameObject;
    
    public Vector3 mousePos3d;
//    public Vector3 canvasLocalScale;
//    public Vector3 mousePos3dLocalSpace;
//    public static EditorReplaceMouseCursor MouseCursorInstance { get; private set; }
    
    public Texture2D OrignalMousePointerTexture;
    //public List<Texture2D> replacementTextureList;
    Texture2D _replacementPointerTexture;
    Texture2D _currentCursor;
    public BricksScriptable _BricksScriptable;

    public Texture2D GetCurrentCursor()
    {
        Debug.Log($"Current cursor returns {_currentCursor.name}");
        return _currentCursor;
    }

    bool alwaysfalse;
    // Start is called before the first frame update
    void Awake()
    {
        _currentCursor = OrignalMousePointerTexture;
        
        _spriteCursorGameObject = GameObject.Find("Cursor");
        
        //Cursor.visible = true;
        //StartCoroutine(SillyMouseTest());
    }
    
    IEnumerator SillyMouseTest()
    {
        yield return new WaitForSeconds(1);
        Cursor.SetCursor(OrignalMousePointerTexture, Vector2.zero, CursorMode.Auto);
        _currentCursor = OrignalMousePointerTexture;
    }

    public void SetnewMouseCursorScriptable(String nameToTry)
    {
        var foundSprite = _BricksScriptable.TiledBricksSprites.Find(d => d.name == nameToTry);
        if (!foundSprite) return;
        
        _spriteCursorGameObject.GetComponent<SpriteRenderer>().sprite = foundSprite;

        AdjustSpriteTo64x64ScalebasedOn128by128Factors(_spriteCursorGameObject.GetComponent<SpriteRenderer>());
    }

    void AdjustSpriteTo64x64ScalebasedOn128by128Factors(SpriteRenderer spriteRenderer)
    {
        var sprite = spriteRenderer.sprite;
        var ppu = sprite.pixelsPerUnit;
        var ppuScale =  ppu/128.0f;
        
        //We want 64x64, what do we have...
        var currentwidth = sprite.rect.width;
        var currentheight = sprite.rect.width;

        var factorXMultiply = ppuScale*(100.0f * 64.0f) / currentwidth;
        var factorYMultiply = ppuScale*(100.0f * 64.0f) / currentheight;

        Vector3 scale = new Vector3(factorXMultiply, factorYMultiply, 1);
        _spriteCursorGameObject.transform.DOScale(scale, 0.0f);

        _currentCursor.name = sprite.name;
    }


    public void SetnewMouseCursor(String nameToTry)
    {
        SetnewMouseCursorScriptable(nameToTry);
        return;
        /*
        String fullname = nameToTry;
        int index = replacementTextureList.FindIndex(d=> d.name == fullname);
        
        if (index > -1)
        {
            Debug.Log($"Set cursor to {replacementTextureList[index]} at index {index} ");
            _currentCursor = replacementTextureList[index];
            var width = replacementTextureList[index].width;
            var height = replacementTextureList[index].height;
            var newRect = new Rect(0, 0, width, height);
            var pivot = new Vector2(0.5f, 0.5f);

            var newSprite = Sprite.Create(replacementTextureList[index], newRect, pivot);
            _spriteCursorGameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
            
            Vector3 scale = new Vector3((64f/width)*128f, (64f/height)*128f, 1);
            _spriteCursorGameObject.transform.DOScale(scale, 0.0f);
            //Hardware version
            //Cursor.SetCursor(replacementTextureList[index], Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Debug.Log($"<color=red> Not found = {nameToTry} </color>");
        }*/
    }

}
