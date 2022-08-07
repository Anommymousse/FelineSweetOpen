using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AutoSpriteShadow : MonoBehaviour
{
    [SerializeField] GameObject sourceObjectToShadow;
    [SerializeField] Vector3 shadowOffset;
    [SerializeField] bool TurnOffScaling;
    GameObject _shadowSprite;
    // Start is called before the first frame update
    void Start()
    {
        _shadowSprite = new GameObject();
        _shadowSprite.transform.position = sourceObjectToShadow.transform.position + shadowOffset;
        var spriteRenderer = _shadowSprite.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sourceObjectToShadow.GetComponent<SpriteRenderer>().sprite;
        _shadowSprite.layer = 16;
        spriteRenderer.sortingLayerName = "Foreground";
        float alphaColor = 228.0f / 255.0f;
        spriteRenderer.color = new Color(0f, 0f, 0f, alphaColor);
        if(TurnOffScaling==false)
            _shadowSprite.transform.localScale = sourceObjectToShadow.transform.localScale;
        _shadowSprite.transform.parent = sourceObjectToShadow.transform;
        _shadowSprite.name = "JustAShadow";
    }

    void OnValidate()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        
    }
}
