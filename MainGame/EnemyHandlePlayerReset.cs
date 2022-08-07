using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;
using UnityEngine.SceneManagement;
using PoolBoss = DarkTonic.CoreGameKit.PoolBoss;

public class EnemyHandlePlayerReset : MonoBehaviour
{
    Vector3 storedStartPosition;
    void Awake()
    {
        var _player = GameObject.Find("Player").GetComponent<Player>();
        _player.OnPlayerReset += ResetEnemy;
        _player.OnPlayerLevelChange += OnPlayerLevelChange;
        storedStartPosition = gameObject.transform.position;
    }

    void OnPlayerLevelChange()
    {
        if(PoolBoss.IsReady)
            PoolBoss.Despawn(this.transform);
    }

    void ResetEnemy()
    {
        if(transform.parent!=null)
            transform.parent.position = storedStartPosition;
        else
            gameObject.transform.position = storedStartPosition;
        
    }
    
}
