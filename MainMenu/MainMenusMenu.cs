using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenusMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public GameObject firstButton;

    void SetNewSelection(GameObject newSelectedOption)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(newSelectedOption);
    }
    
    
   
}
