using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BootStrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Initilise()
    {
        var inputGameObject = new GameObject("[Input System]");
        //inputGameObject.AddComponent<ImpManager>();
        GameObject.DontDestroyOnLoad(inputGameObject);
        
        
    }
    
}
