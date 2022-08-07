using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StandAloneInGameUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetSceneByName("LevelManagerEasy").isLoaded == true)
            gameObject.SetActive(false);
    }

}
