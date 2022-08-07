using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;
using static MyExtensions.MyExtensions;

//KEY ONLY!
public class CollectByPlayer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Player"))
        {
            //var player = GameObject.FindWithTag("Player").GetComponent<Player>();
            Player.PlayerGetsKey();
            //player.OnPlayerGetsKey.Invoke();
            PoolBoss.Despawn(transform);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name.Contains("Player"))
        {
            Log($" <color=red> {gameObject.name} OCE2d </color>");
            PoolBoss.Despawn(transform);
        }
    }
}
