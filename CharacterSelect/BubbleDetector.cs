using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleDetector : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] SpriteRenderer _unlockSprite;
    bool spriteListInitialised;
    List<Sprite> _spritesList;
    int _CatID = 0;

    void Awake()
    {
        spriteListInitialised = false;
        var parentList = GetComponentInParent<PowerListForCharacterSelection>()?.GetList();
        if (parentList != null)
        {
            spriteListInitialised = true;
            _spritesList = parentList;
        }
        _CatID = GetComponent<BubbleUnlock>().GetCatID();
    }

    public void SetInside()
    {
        if(spriteListInitialised==false) return;

        if(_CatID>0)
            _unlockSprite.sprite = _spritesList[_CatID-1];
        
        if((_CatID==3)||(_CatID==7)||(_CatID==11)) 
            _unlockSprite.sprite = _spritesList[(int) Power.PowerName.None];
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(spriteListInitialised==false) return;

        if(_CatID>0)
            _unlockSprite.sprite = _spritesList[_CatID-1];
        
        if((_CatID==3)||(_CatID==7)||(_CatID==11)) 
            _unlockSprite.sprite = _spritesList[(int) Power.PowerName.None]; 

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(spriteListInitialised==false) return;
    }
}
