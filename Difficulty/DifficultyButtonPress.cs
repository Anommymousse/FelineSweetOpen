using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DifficultyButtonPress : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] string LevelString;
    DifficultyLevel _difficultyLevel;

    void Awake()
    {
        _difficultyLevel = GameObject.Find("DifficultyObject").GetComponent<DifficultyLevel>();
    }

    void OnEnable()
    {
        ButtonPressed();
    }

    public void ButtonPressed()
    {
        FadeController.FadeOut("CharacterSelect");
        _difficultyLevel.SetDifficulty(LevelString);
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        FadeController.FadeOut("CharacterSelect");
        _difficultyLevel.SetDifficulty(LevelString);
        //SceneManager.LoadScene("CharacterSelect");
    }
}

