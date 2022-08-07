using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartLevelButton : MonoBehaviour
{
    [SerializeField] string _levelName;

    //Same as a function to get.
    public string LevelName => _levelName;

    public void LoadLevel()     
    {
        ScoreSystem.ResetScore();
        FadeController.FadeOut(_levelName);
        //SceneManager.LoadScene(_levelName);
    }
    

}
