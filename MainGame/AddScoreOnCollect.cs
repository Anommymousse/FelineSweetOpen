using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;

public class AddScoreOnCollect : MonoBehaviour
{
    [SerializeField] public int ScoreToAdd = 0;
    // Start is called before the first frame update
    InGameScoreSetter _theScore;
    void Awake()
    {
        _theScore = GameObject.Find("Score").GetComponentInChildren<InGameScoreSetter>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            _theScore.AddToScore(ScoreToAdd);
            PoolBoss.Despawn(transform);
        }
    }
}