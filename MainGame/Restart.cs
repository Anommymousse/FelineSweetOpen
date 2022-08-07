using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public event Action CalledOnRestart;
    // Start is called before the first frame update
    public void SignalRestart()
    {
        CalledOnRestart?.Invoke();
    }
    
}


