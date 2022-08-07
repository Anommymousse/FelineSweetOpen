using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorDifficultyChange : MonoBehaviour
{
    TMP_Text _difficultyTMPText;
    string[] Difficulties = {"Custom", "Custom"};
    //string[] Difficulties = {"Custom", "Normal"};

    int minimumLevel = 0;
    int maximumLevel = 1;
    static int currentLevel = 0;

    public static void AssignLevel(string difficultyToSet)
    {
        if (difficultyToSet.Contains("Custom")) currentLevel = 0;
        if (difficultyToSet.Contains("Normal")) currentLevel = 1;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        _difficultyTMPText= transform.Find("DifficultyText").GetComponent<TMP_Text>();
        //_difficultyTMPText = arrayTMPTexts;
        _difficultyTMPText.SetText(Difficulties[currentLevel]);
    }

    public void IncreaseLevel()
    {
        Debug.Log($"<color=red> increase level </color>");
        if (LevelLoader.runningTestMode == false)
        {
            if (currentLevel < maximumLevel)
                currentLevel++;
            _difficultyTMPText.SetText(Difficulties[currentLevel]);
            AutoLoadIfRequired();
        }
        Debug.Log($"<color=red> increase level end</color>");
    }

    void AutoLoadIfRequired()
    {
        var autoloadOn = GameObject.Find("AutoloadToggle").GetComponent<Toggle>().isOn;
        if (autoloadOn)
        {
            var go = GameObject.Find("LevelManager").GetComponent<LevelLoader>();
            PlayerPrefs.SetString("FSKey_Diff",Difficulties[currentLevel]);
            go.AssignCurrentLevelUItoPlayerPrefs();
            LevelLoader.ReloadLevel();
        }
    }

    public void DecreaseLevel()
    {
        Debug.Log($"<color=red> decrease level </color>");
        if (LevelLoader.runningTestMode == false)
        {
            if (currentLevel > minimumLevel)
                currentLevel--;
            Debug.Log($"<color=red> decrease level to {currentLevel}</color>");
            Debug.Log($"<color=red> decrease level to {Difficulties[currentLevel]}</color>");
            _difficultyTMPText.SetText(Difficulties[currentLevel]);
            AutoLoadIfRequired();
        }
        Debug.Log($"<color=red> decrease level end</color>");
    }

}
