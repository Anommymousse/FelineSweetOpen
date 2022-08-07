using System;
using DarkTonic.PoolBoss;
using TMPro;
using UnityEngine;

public class CollectACake : MonoBehaviour
{
    BrickMap _brickMapObject;
    Vector3Int cakeCellLocation;
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
            HandleCollectingCake();
            PoolBoss.Despawn(transform);
        }
    }

    void HandleCollectingCake()
    {
        cakeCellLocation = _brickMapObject.NonHiddenTilemap.WorldToCell(transform.position);
        int test = _brickMapObject.hiddenCandyCellsList.IndexOf(cakeCellLocation);
        if (test >= 0)
        {
            var cakecount = GameObject.Find("CakeCount");
            if (cakecount != null)
            {
                TMP_Text text = cakecount.GetComponent<TMP_Text>();
                int value = Int32.Parse(text.text);
                value++;
                text.SetText(value.ToString());
                
                var starscountbright = GameObject.Find("CakeCountBright");
                TMP_Text text2 = starscountbright.GetComponent<TMP_Text>();
                text2.SetText(value.ToString());
                
                UpdatingKittyFund.AddCakeToCurrentKittyFund(cakeCellLocation);
            }
            _brickMapObject.hiddenCandyCellsList.RemoveAt(test);
            _brickMapObject.hiddenCandyCollectedList.RemoveAt(test);
            _brickMapObject.hiddenCandyActiveList.RemoveAt(test);
            _brickMapObject.hiddenCandyTilesList.RemoveAt(test);
        }
    }
}