using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAddUnlockPowers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         SceneManager.LoadSceneAsync("PowersAndCharacterUnlockGameVersion",LoadSceneMode.Additive);
    }

}
