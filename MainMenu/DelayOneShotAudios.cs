using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayOneShotAudios : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip _audioClip;
    bool audioActive = false;
    // Start is called before the first frame update
    void Start()
    {
        audioActive = true;
        _audioSource = GetComponentInParent<AudioSource>();
    }

    public void ButtonPressedPlayAudio()
    {
        if (audioActive)
        {
            _audioSource.PlayOneShot(_audioClip);
            StartCoroutine(CoroutineWaitFor(_audioClip.length));
        }
    }

    IEnumerator CoroutineWaitFor(float waitTime)
    {
        audioActive = false;
        yield return new WaitForSeconds(waitTime);
        audioActive = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
