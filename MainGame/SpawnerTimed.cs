using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;

public class SpawnerTimed : MonoBehaviour
{
    public float spawnInterval = 5.0f;
    public string NameOfThingToSpawn;
    float timeToNextSpawn;

    bool stillSpawning = true;
    bool wasResetThisFrame = false;
    GameObject _storedplayer;
    Player _storedPlayer;
    SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void OnEnable()
    {
        stillSpawning = true;
        timeToNextSpawn = spawnInterval;
        StartCoroutine(SpawnEveryXSeconds());
        
        //? Problem on reenable?
        //var _player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _storedplayer = GameObject.Find("Player");
        _storedPlayer = _storedplayer.GetComponent<Player>();
        _storedPlayer.OnPlayerReset += ResetTimerToZero;
        _storedPlayer.OnPlayerLevelChange += OnPlayerLevelChange;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnDisable()
    {
        /*var sanitycheck = GameObject.Find("Player");
        if (sanitycheck == null)
        {
            Debug.Log("Player was null, weird!");
            return;
        }*/
        //var _player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        if (_storedplayer == null)
        {
            Debug.Log("Player null, weird!");
            return;
        }

        if (_storedPlayer == null)
        {
            Debug.Log("Player move null, weird!");
            return;
        }
        
        _storedPlayer.OnPlayerReset -= ResetTimerToZero;
        _storedPlayer.OnPlayerLevelChange -= OnPlayerLevelChange;
    }

    void OnPlayerLevelChange()
    {
        StopCoroutine(SpawnEveryXSeconds());
    }

    void ResetTimerToZero()
    {
        timeToNextSpawn = spawnInterval;
        wasResetThisFrame = true;
    }


    IEnumerator SpawnEveryXSeconds()
    {
        yield return null;
        while (stillSpawning)
        {
            yield return null;
            
            
            while (timeToNextSpawn > 0.0f)
            {
             
                _spriteRenderer.color = Color.red;
                if (timeToNextSpawn > 0.5f)
                    _spriteRenderer.color = Color.white;
                
                timeToNextSpawn -= Time.deltaTime;
                yield return null;
            }
            timeToNextSpawn = spawnInterval;
            if(wasResetThisFrame==false)
                PoolBoss.SpawnInPool(NameOfThingToSpawn, transform.position, Quaternion.identity);
            
            wasResetThisFrame = false;
        }
    }
}
