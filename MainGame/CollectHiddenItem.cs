using System;
using DarkTonic.PoolBoss;
using TMPro;
using UnityEngine;

public class CollectHiddenItem : MonoBehaviour
{
    BrickMap _brickMapObject;
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
            HandleCollectingHiddenItem();
            PoolBoss.Despawn(transform);
        }
    }

    void HandleCollectingHiddenItem()
    {
        var cellLocation = _brickMapObject.NonHiddenTilemap.WorldToCell(transform.position);
        int test = _brickMapObject.hiddenCandyCellsList.IndexOf(cellLocation);
        if (test >= 0)
        {
            var itemcount = GameObject.Find("CakeCount");
            var itemcountbright = GameObject.Find("CakeCountBright");
            if (itemcount != null)
            {
                TMP_Text text = itemcount.GetComponent<TMP_Text>();
                int value = Int32.Parse(text.text);
                value++;
                text.SetText(value.ToString());
                itemcountbright.GetComponent<TMP_Text>().SetText(value.ToString());
            }            
            _brickMapObject.hiddenCandyCellsList.RemoveAt(test);
            _brickMapObject.hiddenCandyCollectedList.RemoveAt(test);
            _brickMapObject.hiddenCandyActiveList.RemoveAt(test);
            _brickMapObject.hiddenCandyTilesList.RemoveAt(test);
        }
    }
    
}