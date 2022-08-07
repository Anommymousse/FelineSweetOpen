using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//static means it's callable from anywhere and only one of it
public static class ScoreSystem 
{
    public static event Action<int> OnScoreChanged;
    public static event Action<int> NewHighScore;

    //long, uint
    static int _score;
    static int _highscore;    

    public static int GetHighScore => _highscore; //Getter
    public static int GetScore => _score; //Getter

    public static void ResetScore() => _score = 0;

    public static void LoadHighScore()
    {
        string key = "HighScore";
        _highscore = PlayerPrefs.GetInt(key);
    }

    public static void Add(int points)
    {
        //Remember to invoke the event!        
        _score += points;
        OnScoreChanged?.Invoke(_score);

        //One problem keeps invoking the new highscore repeatedly rather than once at death/exit.
        if(_score > _highscore)
        {
            _highscore = _score;
            NewHighScore?.Invoke(_score);            
        }
    }
}
