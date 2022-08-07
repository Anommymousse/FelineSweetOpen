using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voice;


public class VoicedVersionNumber : MonoBehaviour
{
    public VersionNumber.VersionNumber versionNumber; 
    // Start is called before the first frame update
    void Start()
    {
        WindowsVoice.Say($"Welcome to version {versionNumber.versionNumber}, hope you have fun");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
