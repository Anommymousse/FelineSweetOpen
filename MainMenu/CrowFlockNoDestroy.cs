using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowFlockNoDestroy : MonoBehaviour
{
    public static CrowFlockNoDestroy Instance { get; private set; }
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
