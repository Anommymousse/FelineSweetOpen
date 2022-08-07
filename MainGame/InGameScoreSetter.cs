using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using TMPro;
using UnityEngine;

public class InGameScoreSetter : MonoBehaviour
{
    [SerializeField] TMP_Text _tmpText;
    [SerializeField] TMP_Text _tmpText2;
    
    int score = 0;

    // Start is called before the first frame update
    public void SetScore(int newScore)
    {
        score = newScore;
        _tmpText.SetText($"{score}");
        if(_tmpText2!=null)
            _tmpText2.SetText(_tmpText.text);
    }

    public void AddToScore(int ScoreToAdd)
    {
        score += ScoreToAdd;
        _tmpText.SetText($"{score}");
        if(_tmpText2!=null)
            _tmpText2.SetText(_tmpText.text);
    }

    
    // Update is called once per frame
}
