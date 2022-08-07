using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class PlaySoundOnButtonPress : MonoBehaviour
{
    public void PlayBackButtonSound()
    {
        MasterAudio.PlaySound("Button");
    }
    
  
}
