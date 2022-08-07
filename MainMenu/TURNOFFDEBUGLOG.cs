using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TURNOFFDEBUGLOG : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
        #else
            Debug.unityLogger.logEnabled = false;
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
