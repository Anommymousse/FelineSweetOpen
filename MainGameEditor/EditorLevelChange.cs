using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorLevelChange : MonoBehaviour
{
    TMP_Text levelText;

    public int minimumLevel = 1;
    public int maximumLevel = 999;
    // Start is called before the first frame update
    void Awake()
    {
        var arrayTMPTexts = GetComponentsInChildren<TMP_Text>();
        //Assumes one number in children.
        for (int i = 0; i < arrayTMPTexts.Length; i++)
        {
            var isNumeric = int.TryParse(arrayTMPTexts[i].text, out int n);
            if (isNumeric == true)
            {
                levelText = arrayTMPTexts[i];
            }
        }
        
    }

    public void IncreaseLevelBy10()
    {
        if (LevelLoader.runningTestMode == false)
        {
            var currentLevel = Int32.Parse(levelText.text);
            currentLevel += 10;
            if (currentLevel > maximumLevel - 1)
                currentLevel = maximumLevel - 1;
            levelText.SetText(currentLevel.ToString());
            AutoLoadIfRequired();
        }
    }
    
    public void IncreaseLevel()
    {
        if (LevelLoader.runningTestMode == false)
        {
            var currentLevel = Int32.Parse(levelText.text);
            if (currentLevel < maximumLevel)
                currentLevel++;
            levelText.SetText(currentLevel.ToString());
            AutoLoadIfRequired();
        }
    }
    
    void AutoLoadIfRequired()
    {
        var autoloadOn = GameObject.Find("AutoloadToggle").GetComponent<Toggle>().isOn;
        if (autoloadOn)
        {
            var go = GameObject.Find("LevelManager").GetComponent<LevelLoader>();
            go.AssignCurrentLevelUItoPlayerPrefs();
            LevelLoader.ReloadLevel();
        }
    }
    
    public void DecreaseLevel()
    {
        if (LevelLoader.runningTestMode == false)
        {
            var currentLevel = Int32.Parse(levelText.text);
            if (currentLevel > minimumLevel)
                currentLevel--;
            levelText.SetText(currentLevel.ToString());
            AutoLoadIfRequired();
        }
    }
    
    public void DecreaseLevelBy10()
    {
        if (LevelLoader.runningTestMode == false)
        {
            var currentLevel = Int32.Parse(levelText.text);
            currentLevel -= 10;
            if (currentLevel < minimumLevel)
                currentLevel = minimumLevel;
            levelText.SetText(currentLevel.ToString());
            AutoLoadIfRequired();
        }
    }

    
    
}
