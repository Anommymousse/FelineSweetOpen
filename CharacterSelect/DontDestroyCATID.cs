using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyCATID : MonoBehaviour
{
    static int CatID;

    public int GetCatID() => CatID;  

    public void SetCatID(int newcatid)
    {
        CatID = newcatid;
    }
    
    public static DontDestroyCATID Instance { get; private set; }
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
