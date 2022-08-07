using System;
using DarkTonic.PoolBoss;
using TMPro;
using UnityEngine;

public class CollectAStar : MonoBehaviour
{
    BrickMap _brickMapObject;
    Vector3Int starCellLocation;
    void Awake()
    {
        var temp = GameObject.Find("TilesBoss");
        _brickMapObject = temp.GetComponent<BrickMap>();
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            HandleCollectingStar();
            PoolBoss.Despawn(transform);
        }
    }
    
    void HandleCollectingStar()
    {
        starCellLocation = _brickMapObject.NonHiddenTilemap.WorldToCell(transform.position);
        int test = _brickMapObject.hiddenCandyCellsList.IndexOf(starCellLocation);
        if (test >= 0)
        {
            var starscount = GameObject.Find("StarsCount");
            if (starscount != null)
            {
                GenericUnlockAchievement.UnlockAchievement("StarsCollectedOne");
                
                var starCount = UpdatingKittyFund.GetCurrentKittyFund();
                starCount++;
                
                TMP_Text text = starscount.GetComponent<TMP_Text>();
                //int value = Int32.Parse(text.text);
                //value++;
                text.SetText(starCount.ToString());
                
                var starscountbright = GameObject.Find("StarsCountBright");
                TMP_Text text2 = starscountbright.GetComponent<TMP_Text>();
                text2.SetText(starCount.ToString());
                
                UpdatingKittyFund.AddStarToCurrentKittyFund(starCellLocation);
                if(starCount>8)
                    GenericUnlockAchievement.UnlockAchievement("StarsCollectedNine");
                    
            }
            _brickMapObject.hiddenCandyCellsList.RemoveAt(test);
            _brickMapObject.hiddenCandyCollectedList.RemoveAt(test);
            _brickMapObject.hiddenCandyActiveList.RemoveAt(test);
            _brickMapObject.hiddenCandyTilesList.RemoveAt(test);
        }
    }
}