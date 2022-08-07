using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSpriteList : MonoBehaviour
{
    [SerializeField] List<Sprite> playerSpriteIdle;
    [SerializeField] List<Sprite> playerSpriteSide;
    
    // Start is called before the first frame update

    Sprite GetPlayerIdleSprite(int catID)
    {
        if (catID < playerSpriteIdle.Count)
        {
            return playerSpriteIdle[catID];
        }
        return null;
    }

    Sprite GetPlayerSideSprite(int catID)
    {
        if (catID < playerSpriteSide.Count)
        {
            return playerSpriteSide[catID];
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
