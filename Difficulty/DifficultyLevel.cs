using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyLevel : MonoBehaviour
{
    public static DifficultyLevel Instance { get; private set; }
    // Start is called before the first frame update
    static string _difficultyLevel = "Normal";

    public void SetDifficulty(string newDifficulty)
    {
        _difficultyLevel = newDifficulty;
        PlayerPrefs.SetString("DifficultyPPID", _difficultyLevel);
    }
    
    public static string GetDifficulty() => _difficultyLevel;
    public string GetDifficultyNS() => _difficultyLevel;

    public static bool IsCustomMode() => _difficultyLevel.Contains("Custom");
    
    
    public string difficultySelected = "Easy";
    string easy_difficultySelected = "Easy";
    string Medium_difficultySelected = "Medium";
    
    public void SaveDifficultyEasy()
    {
        difficultySelected = easy_difficultySelected;
    }

    public void SaveDifficultyMedium()
    {
        difficultySelected = Medium_difficultySelected;
    }

    void Awake()
    {
        //Singleton pattern - make and then destroy objects ?
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }        
    
    
}
