using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDifficultyModeInMenu : MonoBehaviour
{
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
    
}
