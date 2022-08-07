using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTextUpdate : MonoBehaviour
{
    Player _playerRef;
    TMP_Text _tmpTextRef;
    
    void OnEnable()
    {
        var wht = GameObject.Find("Player");
        _playerRef = wht.GetComponent<Player>();
        _playerRef.OnPlayerLevelChange += UpdateTheText;
        _tmpTextRef = GetComponent<TMP_Text>();
        _tmpTextRef.text = LevelLevelSelected.GetLevelStat().ToString();
    }

    void UpdateTheText()
    {
        StartCoroutine(UpdateTheTextOnDelay());
    }

    IEnumerator UpdateTheTextOnDelay()
    {
        yield return new WaitForSeconds(0.5f);
        var GO = GameObject.Find("LevelManager");
        var levelloader = GO.GetComponent<LevelLoader>();
        var level = levelloader.GetLevelLevel();
        var levelnum = Int32.Parse(level);
        var levelDifficulty = levelloader.GetLevelDifficulty();
        if (levelDifficulty.ToLower().Contains("hidden"))
            _tmpTextRef.text = "Secret";
        else
            _tmpTextRef.text = (levelnum).ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
