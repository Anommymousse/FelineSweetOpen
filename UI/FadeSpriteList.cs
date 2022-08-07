using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

public class FadeSpriteList : MonoBehaviour
{
    FadeSpriteList Instance;
    [SerializeField] List<Sprite> _spriteList;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Sprite GetRandomFadeSprite()
    {
        int maxSprites = _spriteList.Count;
        if (maxSprites == 0) return null;
        int random = Random.Range(1, maxSprites) - 1;
        return ( _spriteList[(Random.Range(1, maxSprites) - 1)]);
    }
}
