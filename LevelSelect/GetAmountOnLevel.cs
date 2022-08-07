using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAmountOnLevel : MonoBehaviour
{
    public string itemName;
    int _itemCount;
        void OnEnable()
        {
        /*    var difficultyGameObject = GameObject.Find("DifficultyObject");
            if (difficultyGameObject == null) return;
            var difficulty = difficultyGameObject.GetComponent<DifficultyLevel>().GetDifficulty();
            int thelevel = LevelLevelSelected.GetLevelStat();
            string hidden = "N";
            _itemCount = LevelItemCounter.GetItemCountForLevel(difficulty, thelevel.ToString(), hidden, itemName);
            Debug.Log($" {itemName} = {difficulty} {thelevel} {hidden} itemcount {_itemCount}");
            var textobj = GetComponent<Text>();
            textobj.text = "x" + _itemCount;*/
        }
}
