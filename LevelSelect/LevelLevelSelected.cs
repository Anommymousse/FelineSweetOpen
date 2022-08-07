using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLevelSelected : MonoBehaviour
{
    public static LevelLevelSelected Instance { get; private set; }
    // Start is called before the first frame update
    static int _level = 1;

    public static void SetLevel(int newLevel) => _level = newLevel;
    
    public void SetLevelAlt(int newLevel) => _level = newLevel;
    public int GetLevel() => _level;
    
    public static int GetLevelStat() => _level;
    
    void Awake()
    {
        //Singleton pattern - make and then destroy objects ?
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }        
    
    
}
