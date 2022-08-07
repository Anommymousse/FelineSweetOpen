using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovementAtTable : MonoBehaviour
{
    public SpriteRenderer StillFrame;
    public SpriteRenderer NomFrame;

    float _timeToStartNom = 5.0f;
    float _timeInNom = 0.75f;
    float _nextNom = 0.0f;
    bool _nomMode = false;
    float _nomTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _nextNom = Random.Range(2.0f, 6.0f) + Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Switch to nom...
        //Switch to nonnom

        //nommode-> nonnom
        if (_nomMode)
        {
            if (Time.time > _nomTimer)
            {
                _nomMode = false;
            }
        }
        
        //nonnom -> nommode
        if (Time.time > _nextNom)
        {
            _nomMode = true;
            _nextNom = _timeToStartNom + Time.time + Random.Range(1.0f, 4.0f);
            _nomTimer = Time.time + _timeInNom;
        }
        
        if (_nomMode)
        {
            StillFrame.color = Color.clear;
            NomFrame.color = Color.white;
        }
        else
        {
            StillFrame.color = Color.white;
            NomFrame.color = Color.clear;
        }
        
    }
}
