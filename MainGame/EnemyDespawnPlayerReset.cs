using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;

public class EnemyDespawnPlayerReset : MonoBehaviour
{
    void Awake()
    {
        var _player = GameObject.Find("Player").GetComponent<Player>();
        _player.OnPlayerReset += DespawnEnemy;
        _player.OnPlayerLevelChange += DespawnEnemy;
    }
    
    void DespawnEnemy()
    {
        PoolBoss.Despawn(this.transform);
    }
    
}
