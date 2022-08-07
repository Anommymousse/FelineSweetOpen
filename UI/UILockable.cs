using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILockable : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {        
        var startButton = GetComponent<UIStartLevelButton>();
        string key = startButton.LevelName + "Unlocked";
        int unlocked = PlayerPrefs.GetInt(key, 0); //0 = default anyways

        if (unlocked == 0)
            gameObject.SetActive(false);
    }

    [ContextMenu("Clear unlock")]
    void ClearLevelUnlock()
    {
        var startButton = GetComponent<UIStartLevelButton>();
        string key = startButton.LevelName + "Unlocked";
        PlayerPrefs.DeleteKey(key);
    }
}
