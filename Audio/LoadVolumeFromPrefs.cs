using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class LoadVolumeFromPrefs : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    float _defaultvolume = 0.8f;
    
    // Start is called before the first frame update
    void Start()
    {
        InitialiseChannelVolume("Master");
        InitialiseChannelVolume("Music");
        InitialiseChannelVolume("Sound FX");
    }

    void InitialiseChannelVolume(string channelID)
    {
        float volumeLevel = PlayerPrefs.GetFloat(channelID, _defaultvolume);
        Debug.Log($"Channel {channelID} with {volumeLevel}");
        if (volumeLevel < 0) volumeLevel = 0;
        audioMixer.SetFloat(channelID, ValueToLogarithmicValue(volumeLevel));
    }

    float ValueToLogarithmicValue(float inputValue)
    {
        float scaledvolume;
        if (inputValue > 0.001)
            scaledvolume = Mathf.Log(inputValue) * 20.0f;
        else
            scaledvolume = -80.0f; //Minimum allowed range is -80db to +20db
        return scaledvolume;
    }
}
