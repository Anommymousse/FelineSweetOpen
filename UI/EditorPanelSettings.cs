using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorPanelSettings : MonoBehaviour
{
    
    public void StartUpEditor()
    {
        Debug.Log("Attempt to goto editor");
        SceneManager.LoadScene("LevelEditor");
    }
    
}