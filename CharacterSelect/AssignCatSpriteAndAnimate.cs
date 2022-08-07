using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AssignCatSpriteAndAnimate : MonoBehaviour
{
    //Assumes 1 sprite renderer
    [SerializeField] Sprite buttonsIdleSprite;
    [SerializeField] Sprite buttonsActiveSprite;
    [SerializeField] bool FlipSpriteState; 
    SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = buttonsIdleSprite;
    }

    // Update is called once per frame
    void Update()
    {
        var thing = GetComponent<IPointerMouseHandler>();
        if (thing.IsInside())
        {
            _spriteRenderer.sprite = buttonsActiveSprite;
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
