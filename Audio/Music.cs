using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Music : MonoBehaviour
{
    [SerializeField] AudioClip[] _musicList;
    AudioSource _currrentMusic;
    public static Music Instance { get; private set; }
    //int _mintrack = 0;
    int _maxtrack;
    int _currentTrack;
    void Awake()
    {
        //Singleton pattern - make and then destroy objects ?
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _currrentMusic = GetComponent<AudioSource>();
            _maxtrack = _musicList.Length-1;
            _currentTrack = SelectNextTrack(0, _maxtrack);
            PlayTrack(_currentTrack);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PlayTrack(int currentTrack)
    {
        if ((_currrentMusic != null) &&(_musicList.Length>0))
        {
            _currrentMusic.clip = _musicList[currentTrack];
            _currrentMusic.Play();
            StartCoroutine(SongsEndEvent(_currrentMusic, () => SetupNextTrack()));
        }
    }

    void SetupNextTrack()
    {
        //At the end of the song, move to next one.
        _currentTrack = SelectNextTrack(0, _maxtrack);
        PlayTrack(_currentTrack);
    }

    int SelectNextTrack(int minTrack, int maxtrack)
    {
        return Random.Range(minTrack, maxtrack);
    }

    IEnumerator SongsEndEvent(AudioSource audioSource, System.Action action)
    {
        yield return new WaitWhile((() => audioSource.isPlaying));
        action();
    }
    
}
