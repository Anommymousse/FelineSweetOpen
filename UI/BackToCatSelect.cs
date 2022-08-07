using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class BackToCatSelect : MonoBehaviour
{
    bool _backToMenu = false; 
    public void MainMenuTransistion()
    {
        //Debug.Log("fade to MainMenu");
        //FadeController.FadeOut("MainMenu");
        _backToMenu = true;
    }

    void LateUpdate()
    {
        if (_backToMenu)
        {
//            PoolBoss.DestroyAllItemsAndReleaseMemory();
            SceneManager.LoadScene("CharacterSelect");
        }
    }
}
