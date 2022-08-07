using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseEnterSFX : MonoBehaviour
{
    
    [SerializeField] AudioClip _audioHoverSource;
    void Start()
    {        
    }
    void OnMouseEnter()
    {
        Debug.Log("Play hover sound");
        if(_audioHoverSource != null)
            GetComponent<AudioSource>()?.PlayOneShot(_audioHoverSource);
    }
}
