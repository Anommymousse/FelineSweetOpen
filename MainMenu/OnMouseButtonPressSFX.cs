using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class OnMouseButtonPressSFX : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] AudioClip _ButtonSound;

    public void MakeTheButtonSound()
    {
        Debug.Log("Button called");
        //if(_ButtonSound != null)
        //{
            MasterAudio.PlaySound("Button");
           // GetComponent<AudioSource>()?.PlayOneShot(_ButtonSound);
        //}
    }
}
